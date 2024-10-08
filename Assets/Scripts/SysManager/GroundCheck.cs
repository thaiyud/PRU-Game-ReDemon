using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;
    public bool onIce;
    private BoxCollider2D box;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask iceLayerMask;
    public float timeSinceLeftGround;
    private void Awake()
    {
        //box = GameObject.Find("Izzy").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isOnGround())
        {
            isGrounded = true;
            if (isOnIce())
            {
                onIce = true;
            }
            else
            {
                onIce = false;
            }
        }
        else
        {
            isGrounded = false;
            timeSinceLeftGround = Time.deltaTime;
        }
    }
    public bool isOnGround()
    {
        float extraHeight = 0.05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(box.bounds.center,box.bounds.size - new Vector3(0.1f, 0f, 0f),0f,Vector2.down,extraHeight,groundLayerMask);
        Color rayColor = Color.green;

        Debug.DrawRay(box.bounds.center + new Vector3(box.bounds.extents.x, 0), Vector2.down * (box.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, 0), Vector2.down * (box.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, box.bounds.extents.y + extraHeight), Vector2.right * (box.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
    }

    public bool isOnIce()
    {
        float extraHeight = 0.05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            box.bounds.center,
            box.bounds.size - new Vector3(0.1f, 0f, 0f),
            0f,
            Vector2.down,
            extraHeight,
            iceLayerMask
        );

        return raycastHit.collider != null;
    }

}
