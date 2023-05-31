using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class PizzaDeliver : MonoBehaviour
{
    public int pizzasEntregadas = 0;
    public int currentPizzas;
    public int totalPizzas;
    public SpawnPoints spawnPoint;
    public GameObject deliverHere;
    [HideInInspector]
    public int rounds = 0;
    //public TextMeshPro repartirText;

    private void Start()
    {
        totalPizzas = 10;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(currentPizzas == totalPizzas)
            {

                if (rounds == 0)
                    totalPizzas = 10;
                if (rounds == 1)
                    totalPizzas = 5;
                if (rounds == 2)
                    totalPizzas = 3;
                if (rounds == 3) 
                    totalPizzas = 1;

                rounds++;
            }
            else
            {
                pizzasEntregadas++;
                currentPizzas++;
                Debug.Log("Pizza entregada");
            }

            spawnPoint.pizzas = currentPizzas;
            spawnPoint.respawn(deliverHere);
        }
    }
}
