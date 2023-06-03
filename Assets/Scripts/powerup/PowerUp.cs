using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PlayerMoveJump velocity;
    public Enemy1 stop;
    public Shooting munition;
    public AudioSource powerup;
    public Animator guindilla;
    public Animator huevo;
    public Animator municion;
    public float time_guindilla = 8f;
    public float time_huevo = 8f;
    public float time_municion = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            powerup.Play();
            if (gameObject.tag== ("guindilla"))
            {
                Debug.Log("guindilla");                
                StartCoroutine(Guindilla());                 
            }
            if (gameObject.tag == ("huevo"))
            {
                Debug.Log("huevo");
                StartCoroutine(Huevo());              
            }
            if (gameObject.tag == ("municion"))
            {
                Debug.Log("municion");
                StartCoroutine(Municion());                
            }
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
    
    IEnumerator Guindilla()
    {
        guindilla.SetTrigger("Touch");         
        velocity.guindilla = true;        
        velocity.moveSpeed *= 1.5f;
        yield return new WaitForSeconds(time_guindilla);        
        velocity.guindilla = false;
        Invoke(nameof(Destroy), 1);        
    }
    IEnumerator Huevo()
    {
        huevo.SetTrigger("Touch");        
        stop.huevo = true;
        yield return new WaitForSeconds(time_huevo);
        stop.huevo = false;
        Invoke(nameof(Destroy), 1);

    }
    IEnumerator Municion()
    {
        municion.SetTrigger("Touch");        
        munition.bulletsLeft = munition.magazineSize;
        yield return new WaitForSeconds(time_municion);
        Invoke(nameof(Destroy), 1);

    }
}
