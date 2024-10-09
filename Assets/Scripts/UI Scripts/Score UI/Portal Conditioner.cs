using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalConditioner : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = false;
        boss = GameObject.FindGameObjectWithTag("Boss");

    }

    // Update is called once per frame
    void Update()
    {
        if (boss != null)
        {
            EnemyHealth bossHealth = boss.GetComponent<EnemyHealth>();
            if (bossHealth.currentHealth <= 0 && bossHealth != null)
            {
                boxCollider2D.isTrigger = true;
            }
        }
        else
        {
            boxCollider2D.isTrigger = true;
        }
    }
}
