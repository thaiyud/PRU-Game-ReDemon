using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrigger : MonoBehaviour
{
	private HashSet<GameObject> playersInFlame = new HashSet<GameObject>();
	[SerializeField] private float damageInterval = 1f; // Time between consecutive damage ticks
	private int damageAmount;

	public void SetDamageAmount(int amount)
	{
		damageAmount = amount;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playersInFlame.Add(other.gameObject);
			StartCoroutine(DealDamageOverTime(other.gameObject));
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playersInFlame.Remove(other.gameObject);
		}
	}

	IEnumerator DealDamageOverTime(GameObject player)
	{
		while (playersInFlame.Contains(player)) // Keep dealing damage while the player is in the flame
		{
			player.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount, this.transform);
			yield return new WaitForSeconds(damageInterval); // Wait for the specified damage interval before applying damage again
		}
	}
}
