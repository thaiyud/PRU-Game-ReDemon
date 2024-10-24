using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float collisionDelay = 0.1f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;
    private bool isColliding = false; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack || isColliding) { return; }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        if (moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = (targetPosition - rb.position).normalized;
    }

    public void StopMoving()
    {
        moveDir = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isColliding)
        {
            StartCoroutine(ChangeDirectionAfterCollision());
        }
    }

    private IEnumerator ChangeDirectionAfterCollision()
    {
        isColliding = true;
        yield return new WaitForSeconds(collisionDelay);
        moveDir = new Vector2(-moveDir.x, -moveDir.y);
        isColliding = false;
    }
}
