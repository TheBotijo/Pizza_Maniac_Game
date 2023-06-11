using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int totalSpawnPoints;
    [SerializeField] List<GameObject> spawnPointList;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public int enemyMax1;
    public int enemyCount1;
    private int lastPoint = -1;
    public float timeBetweenSpawns1 = 10;
    public float timeBetweenSpawns2 = 7;
    public float timeBetweenSpawns3 = 4;
    //int enemyCount2;
    public float timeReduce = 0.97f;
    public float timeReduceMax = 1.5f;
    //timer
    float timer = 0;
    float timer2 = 0;
    float timer3 = 0;


    void Update()
    {
        if (timer > timeBetweenSpawns1)
        {
            timer = 0;
            EnemySpawns(timeBetweenSpawns1);            
        } 
        else
            timer += Time.deltaTime;
        
        if (timer2 > timeBetweenSpawns2)
        {
            timer2 = 0;
            EnemySpawns(timeBetweenSpawns2);            
        } 
        else
            timer2 += Time.deltaTime;

        if (timer3 > timeBetweenSpawns3)
        {
            timer3 = 0;
            EnemySpawns(timeBetweenSpawns3);            
        } 
        else
            timer3 += Time.deltaTime;
    }
    void EnemySpawns(float time)
    {
        if (enemyCount1 < enemyMax1)
        {
            int spawnPoint = Random.Range(0, totalSpawnPoints - 1);
            while (spawnPoint == lastPoint)
            {
                spawnPoint = Random.Range(1, totalSpawnPoints - 1);
            }
            lastPoint = spawnPoint;
            if (time == timeBetweenSpawns1)
            {
                Instantiate(enemy1, spawnPointList[spawnPoint].transform.position, Quaternion.identity);
                Debug.Log("Enemy 1 Spawned ");
            }
            if (time == timeBetweenSpawns2)
            {
                Instantiate(enemy2, spawnPointList[spawnPoint].transform.position, Quaternion.identity);
                Debug.Log("Enemy 2 Spawned ");
            }
            if (time == timeBetweenSpawns3)
            {
                Instantiate(enemy3, spawnPointList[spawnPoint].transform.position, Quaternion.identity);
                Debug.Log("Enemy 3 Spawned ");
            }
            
            enemyCount1++;
            if (timeBetweenSpawns1 > timeBetweenSpawns1 - timeReduceMax)
            {
                timeBetweenSpawns1 *= timeReduce;
            }
            if (timeBetweenSpawns2 > timeBetweenSpawns2 - timeReduceMax)
            {
                timeBetweenSpawns2 *= timeReduce;
            }
            if (timeBetweenSpawns3 > timeBetweenSpawns3 - timeReduceMax)
            {
                timeBetweenSpawns3 *= timeReduce;
            }
        }
    }
    //IEnumerator SpawnEnemy()
    //{
    //    while (enemyCount1 < enemyMax1) 
    //    {
    //        int spawnPoint = Random.Range(0, totalSpawnPoints - 1);
    //        while (spawnPoint == lastPoint)
    //        {
    //            spawnPoint = Random.Range(1, totalSpawnPoints - 1);
    //        }
    //        lastPoint = spawnPoint;
    //        Instantiate(enemy1, spawnPointList[spawnPoint].transform.position, Quaternion.identity);
    //        enemyCount1++;
    //        yield return new WaitForSeconds(timeBetweenSpawns);
    //        if (timeBetweenSpawns > timeReduceMax)
    //        {
    //            timeBetweenSpawns *= timeReduce;
    //        }
            
    //        Debug.Log("Enemy Spawned ");
    //    }
        
    //}
}
