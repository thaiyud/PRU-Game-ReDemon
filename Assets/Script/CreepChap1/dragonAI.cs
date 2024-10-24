using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : MonoBehaviour
{
    [SerializeField] int XorY = 0;
    private Vector2 moveDirection;
    private DragonPathFinding dragonPathfinding;

    private void Awake()
    {
        dragonPathfinding = GetComponent<DragonPathFinding>();
        moveDirection = GetInitialDirection();
    }

    private void Update()
    {
        dragonPathfinding.MoveTo(moveDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveDirection = -moveDirection;  
    }

    private Vector2 GetInitialDirection()
    {
        if (XorY == 0)
        {
            return Vector2.up;  
        }
        else
        {
            return Vector2.right;  
        }
    }
}
