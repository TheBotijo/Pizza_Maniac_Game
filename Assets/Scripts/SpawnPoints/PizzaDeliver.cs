using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class PizzaDeliver : MonoBehaviour
{
    public int currentPizzas = 0;
    public int totalPizzas;
    public SpawnPoints spawnPoint;
    public GameObject deliverHere;
    [HideInInspector]
    public int rounds = 0;
    //public TextMeshPro repartirText;

    private void Start()
    {
        totalPizzas = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (currentPizzas == totalPizzas - 1)
            {
                spawnPoint.entregadas = true;
                currentPizzas++;
            }
            else if (currentPizzas == totalPizzas)
            {
                spawnPoint.entregadas = false;
                if (rounds == 0)
                    totalPizzas = 5;
                else
                {
                    if (rounds == 1)
                        totalPizzas = 3;
                    if (rounds == 2)
                        totalPizzas = 2;
                    if (rounds == 3)
                        totalPizzas = 1;

                }
                rounds++;
                currentPizzas = 0;
            }
            else
                currentPizzas++;

            spawnPoint.respawn(deliverHere);
        }
    }
}
