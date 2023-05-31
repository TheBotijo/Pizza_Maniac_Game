using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public int spawnPoints;
    [SerializeField] List<GameObject> spawnPointList;
    [SerializeField] List<GameObject> deliverPointList;
    public GameObject player;
    public GameObject deliverPoint;
    public int pizzas = 0;
    private int lastPoint = 0;
    private int spawnPoint;

    private void Start()
    {
        respawn(deliverPoint);
        player.transform.position = spawnPointList[spawnPoint].transform.position;
        player.transform.localRotation = spawnPointList[spawnPoint].transform.localRotation;

    }
    public void respawn(GameObject deliverPoint)
    {
        if (pizzas== 0)
        {
            Debug.Log("Num de pizzas: " + pizzas + "(Deberia ser 0)");
            deliverPoint.transform.position = deliverPointList[0].transform.position;
        }
        else
        {
            spawnPoint = Random.Range(1, spawnPoints);
            while (spawnPoint == lastPoint)
            {
                spawnPoint = Random.Range(1, spawnPoints);
                Debug.Log("Ha salido el mismo");
            }
            Debug.Log("Last point avant: " + lastPoint);
            lastPoint = spawnPoint;
            Debug.Log("Num de pizzas: " + pizzas);
            Debug.Log("Random num: " + spawnPoint);
            Debug.Log("Last point apres: " + lastPoint);
            deliverPoint.transform.position = deliverPointList[spawnPoint].transform.position;
        }
        
    }
    
}
