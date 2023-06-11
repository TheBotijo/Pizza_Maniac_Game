using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    
    
    public AudioSource tomato;

    [SerializeField]
    int damage = 5;


    private void Start()
    {
        tomato = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tomato.Play();
            other.GetComponent<Health_Damage>().LoseHealth(damage);
            Debug.Log("tomato");
            Invoke(nameof(destroy), 0.5f);
            
        }

    }
    private void destroy()
    {
        Destroy(gameObject);
    }
}
