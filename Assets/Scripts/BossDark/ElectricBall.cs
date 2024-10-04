using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
{
	public float bulletLife { get; set; }
	public float speed { get; set; }
	public int damageAmout { get; set; }
	private Vector2 spawnPoint;
	private float timer = 0f;
	private Vector2 direction;
	[SerializeField] private GameObject electricBallVFX;

	void Start()
	{
		spawnPoint = transform.position;
		direction = transform.right;
		// Set initial rotation to match direction
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	void Update()
	{
		if (timer > bulletLife)
		{
			Destroy(this.gameObject);
			return;
		}

		timer += Time.deltaTime;
		transform.position += (Vector3)(direction * speed * Time.deltaTime);

		transform.Rotate(0, 0, 360 * Time.deltaTime); // spin once per second
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<PlayerController3>(out var player))
		{
			if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
			{
				Instantiate(electricBallVFX, transform.position, Quaternion.identity);
				playerHealth.TakeDamage(damageAmout, this.transform);
				Destroy(this.gameObject);
			}
		}
		else
		{
			// Handle bounce
			Vector2 normal = collision.contacts[0].normal;
			direction = Vector2.Reflect(direction, normal);

			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle);
		}
	}
}