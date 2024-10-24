using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string tagPlayer = "Player";
    public List<Collider2D> detectGameObjs = new List<Collider2D>();

    public Collider2D col;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == tagPlayer)
        {
            detectGameObjs.Add(collider);
        }
        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == tagPlayer)
        {
            detectGameObjs.Remove(collider);
        }
    }
}
