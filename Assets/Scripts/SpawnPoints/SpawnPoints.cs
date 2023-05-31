using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{    
    [SerializeField] List<GameObject> spawnPointList;
    public int deliverPoints;
    [SerializeField] List<GameObject> deliverPointList;
    public GameObject player;
    public GameObject deliverHere;
    public int pizzas = 0;
    private int lastPoint = 0;
    private int spawnPoint;

    private void Start()
    {
        spawnPoint = Random.Range(1, 3);
        respawn(deliverHere);
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
            spawnPoint = Random.Range(1, deliverPoints);
            while (spawnPoint == lastPoint)
            {
                spawnPoint = Random.Range(1, deliverPoints);
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
