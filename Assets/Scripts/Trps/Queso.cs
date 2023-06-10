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
    [SerializeField] 
    private ParticleSystem pisarQueso;

    private void Start()
    {
        cheese = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pisarQueso.Play();
            References(other);
            cheese.Play();
            other.GetComponent<Health_Damage>().LoseHealth(damage);
            Debug.Log("DAÑANDO A PLAYER");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !velocity.guindilla)
        {
            //if (velocity.flatVel.magnitude >= 0)
            //{
            //    pisarQueso.Play();
            //}
            //else pisarQueso.Stop();

            References(other);
            other.GetComponent<Health_Damage>().LoseHealth(damage);
            //Debug.Log("velocidad lenta");
            referencess.moveSpeedr /= VelocityFactor;
            velocity.cheese = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pisarQueso.Stop();
            References(other);
            cheese.Pause();
            //Debug.Log("velocidad normal");
            velocity.cheese = false;
        }
    }

    private void References(Collider other)
    {
        velocity = other.GetComponent<PlayerMoveJump>();
        referencess = other.GetComponent<PlayerMoveJump>().references;
    }
}
