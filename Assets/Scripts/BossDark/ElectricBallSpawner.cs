using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBallSpawner : MonoBehaviour
{

	[Header("Bullet Attributes")]
	public GameObject bullet;
	[SerializeField] private float bulletLife = 3f;
	[SerializeField] private float speed = 8f;
	[SerializeField] private int damage = 1;

	[Header("Spawner Attributes")]
	[SerializeField] private float firingRate = 1f; 
	[SerializeField] private int projectilesPerBurst = 10; 
	[SerializeField] private float angleSpread = 360f; 
	[SerializeField] private float burstDuration = 2f; // Duration to shoot before stopping

	public bool IsActingComplete { get; private set; }

	public void Run() // Add this method
	{
		StartCoroutine(ShootRoutine());
	}

	void Update()
	{
		transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 1f);
	}

	private IEnumerator ShootRoutine()
	{
		float elapsedTime = 0f;
		IsActingComplete = false;


		while (elapsedTime < burstDuration)
		{
			Fire();
			yield return new WaitForSeconds(firingRate);
			elapsedTime += firingRate;
		}

		yield return new WaitForSeconds(3f);
		IsActingComplete = true;
	}

	private void Fire()
	{
		Vector2 playerPosition = PlayerController3.Instance.transform.position;
		Vector2 targetPosition = new Vector2(playerPosition.x, playerPosition.y - 0.5f);


		Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

		// Calculate the initial angle for the burst
		float initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - (angleSpread / 2);
		float angleStep = angleSpread / (projectilesPerBurst - 1);

		for (int i = 0; i < projectilesPerBurst; i++)
		{
			// Calculate the rotation for each bullet
			Quaternion bulletRotation = Quaternion.Euler(0, 0, initialAngle + (i * angleStep));
			GameObject spawnedBullet = Instantiate(bullet, transform.position, bulletRotation);
			spawnedBullet.GetComponent<ElectricBall>().speed = speed;
			spawnedBullet.GetComponent<ElectricBall>().bulletLife = bulletLife;
			spawnedBullet.GetComponent<ElectricBall>().damageAmout = damage;
		}
	}
}
