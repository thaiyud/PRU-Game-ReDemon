using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{

    [SerializeField] private GameObject heartPreFab, staminaPrefab;
    public void DropItems()
    {

        float dropChance = Random.Range(0f, 1f);
        
        if (dropChance <= 0.5f)
        {
            int randomNum = Random.Range(1, 4);

            switch (randomNum)
            {
                case 1:
                    Instantiate(heartPreFab, transform.position, Quaternion.identity);
                    break;
                case 2:
                    int randomNumOfStamina = Random.Range(1, 4);
                    for (int i = 0; i < randomNumOfStamina; i++)
                    {
                        Instantiate(staminaPrefab, transform.position, Quaternion.identity);
                    }
                    break;
                case 3:
                    Instantiate(heartPreFab, transform.position, Quaternion.identity);
                    Instantiate(staminaPrefab, transform.position, Quaternion.identity);
                    break;
            }
        }
    }
}
