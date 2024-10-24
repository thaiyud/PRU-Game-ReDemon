using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] GameObject teleport;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
void Update()
{
    if (teleport != null)
    {
        Teleport teleportComponent = teleport.GetComponent<Teleport>();
        if (teleportComponent != null)
        {
            transform.position = teleportComponent.GetTelePoint().position;
        }
        else
        {
            return;
        }
    }
    else
    {
        return;
    }
}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Teleport"))
		{
			teleport = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Teleport"))
		{
			teleport = null;
		}
	}
}

