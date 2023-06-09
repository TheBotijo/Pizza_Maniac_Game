using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queso : MonoBehaviour
{
    public PlayerMoveJump velocity;
    public AudioSource cheese;    
    public PowerUp guindilla;

    [SerializeField]
    int damage = 2;
    [SerializeField]
    float VelocityFactor = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cheese.Play();
            other.GetComponent<Health_Damage>().loseHealth(damage);
            Debug.Log("DAÑANDO A PLAYER");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !velocity.guindilla)
        {
            
            other.GetComponent<Health_Damage>().loseHealth(damage);
            //Debug.Log("velocidad lenta");
            velocity.moveSpeed /= VelocityFactor;
            velocity.cheese = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            cheese.Pause();
            //Debug.Log("velocidad normal");
            velocity.cheese = false;
        }
    }
}
