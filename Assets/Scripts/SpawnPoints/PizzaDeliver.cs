using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PizzaDeliver : MonoBehaviour
{
    [SerializeField] private GameReferences references;
    [HideInInspector] public int currentPizzas = 0;
    [HideInInspector] public int totalPizzas;
    private GameObject player;
    private SpawnPoints spawnPoint;
    private EnemySpawn spawnEnemy;
    private GameObject deliverHere;
    public Enemy1 enemy1;
    public AudioSource deliver;
    private GameObject winUI;
    public AudioSource win;
    [HideInInspector]
    public int rounds = 0;
    //public TextMeshPro repartirText;

    private void Start()
    {
        //Assignamos las referencias
        player = references.playerr;
        deliverHere = references.deliverHere;
        winUI = references.winUI;
        spawnPoint = references.SpawnSystem.GetComponent<SpawnPoints>();
        spawnEnemy = references.GetComponent<EnemySpawn>();
        totalPizzas = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
                {
                    Debug.Log("Temps màxim a reduir: " + spawnEnemy.timeReduceMax);
                    Debug.Log("Multiplicador: " + spawnEnemy.timeReduce);
                    Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns);
                    Debug.Log("Enemigos totales: " + spawnEnemy.enemyMax1);
                    Debug.Log("Vida enemigo: " + enemy1.Health);
                    totalPizzas = 5;
                }
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

                    enemy1.Health += 7.5f;
                    spawnEnemy.timeBetweenSpawns -= 1.5f;
                    spawnEnemy.timeReduceMax -=1.5f;
                    spawnEnemy.enemyMax1 += 5;
                    spawnEnemy.timeReduce -= 0.05f;
                    spawnEnemy.enemyCount1 = 0;
                    Debug.Log("Temps màxim a reduir: " + spawnEnemy.timeReduceMax);
                    Debug.Log("Multiplicador: " + spawnEnemy.timeReduce);
                    Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns);
                    Debug.Log("Enemigos totales: " + spawnEnemy.enemyMax1); 
                    Debug.Log("Vida enemigo: " + enemy1.Health);
                }
                rounds++;
                
                //Debug.Log("Enemy max: " + spawnEnemy.enemyMax1);

                currentPizzas = 0;
            }
            else { deliver.Play(); currentPizzas++; }
                
            spawnPoint.respawn(deliverHere);
        }
    }
}
