using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenSpawn : MonoBehaviour
{
	[SerializeField] private GameObject shurikenPrefab;  
	[SerializeField] private int damage = 1;
	[SerializeField] private int projectile = 3;        
	[SerializeField] private float spawnRadius = 7f;     
	[SerializeField] private float minRadius = 4f;
	[SerializeField] private float duration = 5f;        // Duration after which shurikens are destroyed


	private List<GameObject> spawnedShurikens = new List<GameObject>(); 

	public bool isActionComplete { get; private set; }
	[SerializeField] private ShurikenSkill shurikenSkill;

	public void Run()
	{
		//shurikenPrefab.GetComponent<ShurikenSkill>().Damage = damage;
		isActionComplete = false;
		//shurikenSkill.Damage = damage;
		//Debug.Log("damage from the spawn: " + damage);
		//Debug.Log("damage from the skill: " + shurikenSkill.Damage);
		StartCoroutine(SpawnShurikens());
	}

	private IEnumerator SpawnShurikens()
	{
		if (PlayerController3.Instance != null)
		{
			for (int i = 0; i < projectile; i++)
			{
				Vector3 playerPosition = PlayerController3.Instance.transform.position;

				Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
				float randomDistance = UnityEngine.Random.Range(minRadius, spawnRadius);

				Vector3 randomPosition = playerPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * randomDistance;


				GameObject shuriken = Instantiate(shurikenPrefab, randomPosition, Quaternion.identity);
				shuriken.GetComponent<ShurikenSkill>().Damage = damage;
				spawnedShurikens.Add(shuriken);
			}
		}

		yield return new WaitForSeconds(duration);

		foreach (GameObject shuriken in spawnedShurikens)
		{
			if (shuriken != null)
			{
				Destroy(shuriken);
			}
		}

		spawnedShurikens.Clear();

		isActionComplete = true;
	}
}
