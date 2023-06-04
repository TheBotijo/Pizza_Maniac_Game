using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Drop> dropList = new List<Drop> ();

    Drop GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); //1-100
        List<Drop> possibleDrops = new List<Drop> ();
        foreach (Drop item in dropList)
        {
            if(randomNumber <= item.dropChance)
            {
                possibleDrops.Add(item);
            }
        }
        if(possibleDrops.Count > 0)
        {

        }
        Debug.Log("No drop");
        return null;
    }
    public void InstantiateDrop(Vector3 spawnPosition)
    {
        Drop droppedItem = GetDroppedItem();
        if(droppedItem != null)
        {
           GameObject dropGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);            
        }
    }
}
