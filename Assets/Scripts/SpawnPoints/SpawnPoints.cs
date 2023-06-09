using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private GameReferences referencess;
    [SerializeField] List<GameObject> spawnPointList;
    public int deliverPoints;
    [SerializeField] List<GameObject> deliverPointList;
    private GameObject player;
    private GameObject deliverHere;
    public bool entregadas = true;
    private int lastPoint = 0;
    private int spawnPoint;

    private void Start()
    {
        referencess = GetComponentInParent<GameReferences>();
        player = referencess.playerr;
        deliverHere= referencess.deliverHere;
        entregadas = true;
        Respawn(deliverHere);
        spawnPoint = Random.Range(0, 2);
        player.transform.position = spawnPointList[spawnPoint].transform.position;
        player.transform.localRotation = spawnPointList[spawnPoint].transform.localRotation;
    }
    public void Respawn(GameObject deliverPoint)
    {
        if (entregadas == true)
        {
            deliverPoint.transform.position = deliverPointList[0].transform.position;
            //entregadas = false;
        }
        else
        {
            spawnPoint = Random.Range(1, deliverPoints);
            while (spawnPoint == lastPoint)
            {
                spawnPoint = Random.Range(1, deliverPoints);
            }
            lastPoint = spawnPoint;
            deliverPoint.transform.position = deliverPointList[spawnPoint].transform.position;
        }
    }
}
