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
    public EnemySpawn spawnEnemy;
    public AudioSource deliver;
    public GameObject winUI;
    public GameObject player;
    public AudioSource win;
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
                    if (rounds == 4)
                    {
                        player.GetComponent<PlayerMoveJump>().enabled = false;
                        winUI.SetActive(true);
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        win.Play();
                    }
                        currentPizzas = 0;
                 

                    spawnEnemy.timeBetweenSpawns -= 2;
                    spawnEnemy.timeReduceMax = spawnEnemy.timeBetweenSpawns - 2;
                    spawnEnemy.enemyMax1 += 10;
                    spawnEnemy.timeReduce -= 0.05f;
                }
                rounds++;
                
                Debug.Log("Enemy max: " + spawnEnemy.enemyMax1);

                currentPizzas = 0;
            }
            else { deliver.Play(); currentPizzas++; }
                

            spawnPoint.respawn(deliverHere);
        }
    }
}
