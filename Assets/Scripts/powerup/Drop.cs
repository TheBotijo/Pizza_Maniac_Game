using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Drop : ScriptableObject
{
    public GameObject lootGameobject;
    public string DropName;
    public int dropChance;

    public Drop(string DropName, int dropChance)
    {        
        this.DropName = DropName;
        this.dropChance = dropChance;
    }
}
 