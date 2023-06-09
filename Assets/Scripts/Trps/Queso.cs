using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queso : MonoBehaviour
{
    private GameReferences referencess;
    private PlayerMoveJump velocity;
    public AudioSource cheese;

    [SerializeField]
    int damage = 2;
    [SerializeField]
    float VelocityFactor = 10;

    private void Start()
    {
        cheese = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            references(other);
            cheese.Play();
            other.GetComponent<Health_Damage>().loseHealth(damage);
            Debug.Log("DAÑANDO A PLAYER");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !velocity.guindilla)
        {
            references(other);
            other.GetComponent<Health_Damage>().loseHealth(damage);
            //Debug.Log("velocidad lenta");
            referencess.moveSpeedr /= VelocityFactor;
            velocity.cheese = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            references(other);
            cheese.Pause();
            //Debug.Log("velocidad normal");
            velocity.cheese = false;
        }
    }

    private void references(Collider other)
    {
        velocity = other.GetComponent<PlayerMoveJump>();
        referencess = other.GetComponent<PlayerMoveJump>().references;
    }
}
