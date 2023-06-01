using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Drop : ScriptableObject
{
    public GameObject lootGameobject;
    public string lootName;
        public int dropChance;

    public Drop(string lootName, int dropChance)
    {        
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
 