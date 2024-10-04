using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSkill : MonoBehaviour
{
	private Animator animator;
	[SerializeField] private float moveSpeed = 12f; 
	[SerializeField] private float skillDuration = 2f;
	[SerializeField] private int damage = 1;
	
	public bool IsActingComplete { get; private set; }

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void Run()
	{
		IsActingComplete = false;
		StartCoroutine(Slash());
	}

	private IEnumerator Slash()
	{
		if (PlayerController3.Instance != null && transform.parent != null)
		{
			Vector3 startPosition = transform.parent.position;
			Vector3 targetPosition = PlayerController3.Instance.transform.position;

			Vector3 direction = (targetPosition - startPosition).normalized;
			float facingDirection = direction.x > 0 ? -1 : 1;

			transform.position = startPosition;
			transform.localScale = new Vector3(facingDirection, transform.localScale.y, transform.localScale.z);

			animator.SetFloat("AnimationSpeed", 1f / skillDuration);
			animator.SetTrigger("SlashAnimation");

			while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
			{
				// Move towards the target position
				transform.position = Vector3.MoveTowards(
					transform.position,
					targetPosition,
					moveSpeed * Time.deltaTime
				);

				AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
				if (stateInfo.IsName("Slash") && stateInfo.normalizedTime >= 1f)
				{
					break;
				}

				yield return null; // Wait for the next frame
			}

			IsActingComplete = true;
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController3>())
		{
			PlayerHealth playerHealth = PlayerController3.Instance.GetComponent<PlayerHealth>();
			playerHealth.TakeDamage(damage, this.transform);
		}
	}
}
