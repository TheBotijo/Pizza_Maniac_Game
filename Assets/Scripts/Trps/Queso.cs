using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queso : MonoBehaviour
{
    public PlayerMoveJump velocity;
    int damage = 2;
    public PowerUp guindilla;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health_Damage>().loseHealth(damage);
            Debug.Log("DAÑANDO A PLAYER");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !velocity.guindilla)
        {
            other.GetComponent<Health_Damage>().loseHealth(damage);
            Debug.Log("velocidad lenta");
            velocity.moveSpeed /= 5;
            velocity.cheese = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            
            Debug.Log("velocidad normal");
            velocity.cheese = false;
        }
    }
}
