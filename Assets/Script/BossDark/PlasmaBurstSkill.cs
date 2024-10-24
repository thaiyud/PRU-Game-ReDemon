using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBurstSkill : MonoBehaviour
{
	
	[SerializeField] private float moveSpeed = 10f; 
	[SerializeField] private float skillDuration = 3f;
	[SerializeField] private int damage = 1;

	private Animator animator;
	private Rigidbody2D rb;

	public bool IsActingComplete { get; private set; }

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	public void Run()
	{
		IsActingComplete = false;
		if (rb != null)
		{
			rb.simulated = true;
		}
		StartCoroutine(Slash());
	}

	private void DisablePhysics()
	{
		if (rb != null)
		{
			rb.simulated = false;
		}
	}
	private IEnumerator Slash()
	{
		if (PlayerController3.Instance != null && transform.parent != null)
		{
			Vector3 startPosition = transform.parent.position;
			Vector3 targetPosition = PlayerController3.Instance.transform.position;

			Vector3 direction = (targetPosition - startPosition).normalized;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			// Set the rotation of the slash effect
			transform.rotation = Quaternion.Euler(0, 0, angle);

			transform.position = startPosition;


			animator.SetFloat("AnimationSpeed", 1f / skillDuration);
			animator.SetTrigger("StartAnimation");

			while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
			{
				// Move towards the target position
				transform.position = Vector3.MoveTowards(
					transform.position,
					targetPosition,
					moveSpeed * Time.deltaTime
				);

				AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
				if (stateInfo.IsName("PlasmaBurstSkill") && stateInfo.normalizedTime >= 1f)
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
