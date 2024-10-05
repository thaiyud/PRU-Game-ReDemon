using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenSkill : MonoBehaviour
{
    public int Damage { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController3>())
		{
			PlayerHealth playerHealth = PlayerController3.Instance.GetComponent<PlayerHealth>();
			Debug.Log("take dmg from the shuriken: " + Damage);
			playerHealth.TakeDamage(Damage, this.transform);
		}
	}
}
