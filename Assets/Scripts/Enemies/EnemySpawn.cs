using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int totalSpawnPoints;
    [SerializeField] List<GameObject> spawnPointList;
    public GameObject enemy1;
    public GameObject enemy2;
    public int enemyMax1;
    public int enemyCount1;
    private int lastPoint = -1;
    public float timeBetweenSpawns = 10;
    int enemyCount2;
    public float timeReduce = 0.97f;
    public float timeReduceMax;
    //timer
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnEnemy());
        timeReduceMax = timeBetweenSpawns - 2;
    }

    void Update()
    {
        if (timer > timeBetweenSpawns)
        {
            timer = 0;
            enemySpawn();            
        } 
        else
            timer += Time.deltaTime;
    }
    void enemySpawn()
    {
        if (enemyCount1 < enemyMax1)
        {
            int spawnPoint = Random.Range(0, totalSpawnPoints - 1);
            while (spawnPoint == lastPoint)
            {
                spawnPoint = Random.Range(1, totalSpawnPoints - 1);
            }
            lastPoint = spawnPoint;
            Instantiate(enemy1, spawnPointList[spawnPoint].transform.position, Quaternion.identity);
            enemyCount1++;
            if (timeBetweenSpawns > timeReduceMax)
            {
                timeBetweenSpawns *= timeReduce;
            }
        }
        Debug.Log("Enemy Spawned ");
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
