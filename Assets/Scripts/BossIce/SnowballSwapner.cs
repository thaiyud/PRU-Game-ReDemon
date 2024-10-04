using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
	enum SpawnerType { Straight, Spin }

	[Header("Bullet Attributes")]
	public GameObject bullet;
	public float bulletLife = 1f;
	public float speed = 1f;

	[Header("Spawner Attributes")]
	[SerializeField] private SpawnerType spawnerType;
	[SerializeField] private float firingRate = 1f; // Time between bursts
	[SerializeField] private float attackRange;
	[SerializeField] private int projectilesPerBurst = 3; // Number of projectiles per burst
	[SerializeField] private float angleSpread = 30f; // Angle spread for the burst
	[SerializeField] private float burstDuration = 3f; // Duration to shoot before stopping

	//private float timer = 0f;
	public event Action OnShootingComplete; // Event to notify when shooting is complete
	public bool IsShootingComplete { get; private set; } // Property to track if shooting is complete

	public void Run() // Add this method
	{
		StartCoroutine(ShootRoutine());
	}

	void Update()
	{
		if (spawnerType == SpawnerType.Spin)
			transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 1f);
	}

	private IEnumerator ShootRoutine()
	{
		float elapsedTime = 0f; // Track the elapsed time
		IsShootingComplete = false; // Reset the shooting status


		while (elapsedTime < burstDuration) // Continue shooting for the specified duration
		{
			Fire(); // Call the Fire method
			yield return new WaitForSeconds(firingRate); // Wait for the specified firing rate
			elapsedTime += firingRate; // Increment elapsed time
		}

		// Optionally wait before the next burst starts
		yield return new WaitForSeconds(3f); // Wait for 3 seconds before the next burst starts

		// Invoke the event after the shooting routine is complete
		OnShootingComplete?.Invoke();
		IsShootingComplete = true; // Set the shooting complete status
								   //StartCoroutine(ShootRoutine()); // Restart the shooting routine
	}

	private void Fire()
	{
		Vector2 playerPosition = PlayerController3.Instance.transform.position; // Get the player's position
		Vector2 targetPosition = new Vector2(playerPosition.x, playerPosition.y - 0.5f); // Adjust to center (modify offset as needed)

		if (Vector2.Distance(transform.position, targetPosition) <= attackRange)
		{
			// Calculate the direction to the target
			Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

			// Calculate the initial angle for the burst
			float initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - (angleSpread / 2);
			float angleStep = angleSpread / (projectilesPerBurst - 1);

			for (int i = 0; i < projectilesPerBurst; i++)
			{
				// Calculate the rotation for each bullet
				Quaternion bulletRotation = Quaternion.Euler(0, 0, initialAngle + (i * angleStep));
				GameObject spawnedBullet = Instantiate(bullet, transform.position, bulletRotation);
				spawnedBullet.GetComponent<SnowBullet>().speed = speed;
				spawnedBullet.GetComponent<SnowBullet>().bulletLife = bulletLife;
			}
		}
	}
}