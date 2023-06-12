using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    private const float _force = 5f;

    [SerializeField] private AudioSource tomato;

    [SerializeField] private ParticleSystem DeathPt;

    [SerializeField] int damage = 5;


    private void Awake()
    {
        PushForward();
        Invoke(nameof(DestroyTomatoo), 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tomato.Play();
            collision.gameObject.GetComponent<Health_Damage>().LoseHealth(damage);
            Debug.Log("tomato");
            DeathPt.Play();
            Invoke(nameof(DestroyTomato), 0.5f);
        }
    }

    private void DestroyTomatoo()
    {
        DeathPt.Play();
        Invoke(nameof(DestroyTomato), 0.5f);
    }
    private void DestroyTomato()
    {
        Destroy(gameObject);
    }

    private void PushForward()
    {
        float force = Random.Range(_force - 1f, _force + 1f);
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, 1f) * force, ForceMode.VelocityChange);
    }
}
