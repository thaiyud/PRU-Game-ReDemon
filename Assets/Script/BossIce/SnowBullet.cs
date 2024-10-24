using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBullet : MonoBehaviour
{
	public float bulletLife = 1f;  // Defines how long before the bullet is destroyed
	public float speed = 5f;        // Speed of the bullet
	private Vector2 spawnPoint;     // Where the bullet was spawned
	private float timer = 0f;       // Timer to track bullet lifetime
	private Vector2 direction;       // Direction of the bullet movement

	[SerializeField] private GameObject snowballVFX;
	[SerializeField] private int damageAmout;

	// Start is called before the first frame update
	void Start()
	{
		spawnPoint = transform.position; // Store the initial spawn point
		direction = transform.right; // Set the initial direction based on rotation
	}

	// Update is called once per frame
	void Update()
	{
		if (timer > bulletLife)
			Destroy(this.gameObject);

		timer += Time.deltaTime;
		transform.position += (Vector3)(direction * speed * Time.deltaTime); // Move bullet
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController3>())
		{
			// Instantiate the VFX at the collision point
			PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
			Instantiate(snowballVFX, transform.position, Quaternion.identity);
			Destroy(this.gameObject); // Optionally destroy the bullet
			playerHealth.TakeDamage(damageAmout,this.transform);
		}
		else
		{
			// Reflect the bullet's direction upon collision with other objects
			Vector2 normal = collision.contacts[0].normal; // Get the contact normal
			Vector2 reflectedDirection = Vector2.Reflect(transform.right, normal); // Reflect the direction
																		  // Log the reflection details
			direction = reflectedDirection;
			transform.up = reflectedDirection;
		}
	}
}
