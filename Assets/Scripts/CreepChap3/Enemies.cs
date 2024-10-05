using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Enemies : MonoBehaviour
{

    //private readonly float damage = 1;
    //public float knockbackForce = 1f;
    public float moveSpeed = 5f;
    private bool isFacingRight = true;
    public DetectionZone detectionZone;
    Rigidbody2D rb;

    //public float Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(detectionZone != null && detectionZone.detectGameObjs.Count > 0)
        {
            Vector2 direction = (detectionZone.detectGameObjs[0].transform.position - transform.position).normalized;
            rb.AddForce((moveSpeed * 10) * Time.deltaTime * direction);
            
            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

     void Flip()
    {
        // Đảo ngược hướng quay mặt
        isFacingRight = !isFacingRight;

        // Lật đối tượng theo trục X
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
