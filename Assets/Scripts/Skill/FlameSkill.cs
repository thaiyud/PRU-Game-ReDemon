using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSkill : MonoBehaviour
{
	[SerializeField] private GameObject flamePrefab;
	[SerializeField] private Transform bossTransform;
	[SerializeField] private int numberOfFlames = 40;
	[SerializeField] private float spacing = 1.5f;
	[SerializeField] private float flameLifetime = 3f;
	[SerializeField] private int damageAmount = 1;
	public float damageInterval = 1f; // Time between consecutive damage ticks

	private HashSet<GameObject> playersInFlame = new HashSet<GameObject>();

	public bool IsActingComplete { get; private set; }

	public void Run()
	{
		IsActingComplete = false;
		StartCoroutine(SpawnFlames());
	}

	IEnumerator SpawnFlames()
	{
		Vector3 bossPosition = bossTransform.position;
		int flamesRemaining = numberOfFlames;

		for (int i = 0; i < numberOfFlames; i++)
		{
			float flameX = bossPosition.x + (i % 2 == 0 ? (i / 2) * spacing : -(i / 2) * spacing);

			Vector3 flamePosition = new Vector3(flameX, bossPosition.y, bossPosition.z);

			GameObject flame = Instantiate(flamePrefab, flamePosition, Quaternion.identity);
			FlameTrigger flameTrigger = flame.GetComponent<FlameTrigger>();
			flameTrigger.SetDamageAmount(damageAmount); // Set the damage amount

			Animator flameAnimator = flame.GetComponent<Animator>();
			if (flameAnimator != null)
			{
				flameAnimator.SetTrigger("StartAnimation");
			}
			else
			{
				Debug.Log("error at the animation");
			}

			StartCoroutine(DestroyFlameAfterTime(flame, flameLifetime, () =>
			{
				flamesRemaining--;
				if (flamesRemaining == 0)
				{
					IsActingComplete = true;
				}
			}));
		}
		yield return null;
	}

	IEnumerator DestroyFlameAfterTime(GameObject flame, float lifetime, System.Action onDestroy)
	{
		yield return new WaitForSeconds(lifetime);

		Destroy(flame);
		onDestroy?.Invoke();
	}	
}
