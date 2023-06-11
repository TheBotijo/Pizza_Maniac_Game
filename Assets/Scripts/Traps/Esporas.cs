using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esporas : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Health_Damage>().LoseHealth(10);
            Debug.Log("Esporas");
        }
    }
}
