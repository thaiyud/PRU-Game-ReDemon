﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleChest : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>())
        {
            GetComponent<PickupSpawner>().DropItems();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
