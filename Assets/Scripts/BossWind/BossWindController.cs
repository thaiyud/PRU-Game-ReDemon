using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindController : MonoBehaviour
{
	[SerializeField] private float health = 100f;
	[SerializeField] private CycloneSkill cycloneSkill;
	[SerializeField] private ShurikenSpawn shurikenSpawn;


	private SpriteRenderer mySprireRenderer;
	private Animator myAnimator;


	readonly int ATTACK_HASH = Animator.StringToHash("Attack");

	private void Awake()
	{
		myAnimator = GetComponent<Animator>();
		mySprireRenderer = GetComponent<SpriteRenderer>();	
	}

	private void Start()
	{
		StartCoroutine(ManageActivation());
	}

	public void Update()
	{
		AdjustFacingDirection();
	}


	private IEnumerator ManageActivation()
	{
		yield return new WaitForSeconds(1f);
		while (health > 0)
		{
			myAnimator.SetTrigger(ATTACK_HASH);
			yield return new WaitForSeconds(0.5f);

			if (!cycloneSkill.gameObject.activeInHierarchy)
			{
				cycloneSkill.gameObject.SetActive(true);
			}

			cycloneSkill.Run();

			yield return new WaitUntil(() => cycloneSkill.IsActingComplete);

			cycloneSkill.gameObject.SetActive(false);

			yield return new WaitForSeconds(cycloneSkill.skillCD);

			
			myAnimator.SetTrigger(ATTACK_HASH);
			yield return new WaitForSeconds(0.5f);


			shurikenSpawn.Run();
			yield return new WaitUntil(() => shurikenSpawn.isActionComplete); 
		}
	}


	private void AdjustFacingDirection()
	{
		if (PlayerController3.Instance != null)
		{
			if (PlayerController3.Instance.transform.position.x < transform.position.x)
			{
				mySprireRenderer.flipX = true;
			}
			else
			{
				mySprireRenderer.flipX = false;
			}
		}
	}
}
