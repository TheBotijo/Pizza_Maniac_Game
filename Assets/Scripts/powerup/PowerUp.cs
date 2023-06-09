using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameReferences referencess;
    private PlayerMoveJump velocityScr;
    private Shooting munitionScr;
    public Enemy1 stop;

    [Header("Sounds")]
    [SerializeField] private AudioSource guindillaSound;
    [SerializeField] private AudioSource huevoSound;

    [Header("Animations")]
    [SerializeField] private Animator guindilla;
    [SerializeField] private Animator huevo, municion;

    [Header("Timings")]
    [SerializeField] private float time_guindilla = 8f;
    [SerializeField] private float time_huevo = 8f;
    [SerializeField] private float time_municion = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            velocityScr = other.GetComponent<PlayerMoveJump>();
            referencess = other.GetComponent<PlayerMoveJump>().references;
            munitionScr = other.GetComponent<Shooting>();

            if (gameObject.CompareTag("guindilla"))
            {
                Debug.Log("guindilla");                
                StartCoroutine(Guindilla());                 
            }
            if (gameObject.CompareTag("huevo"))
            {
                Debug.Log("huevo");
                StartCoroutine(Huevo());              
            }
            if (gameObject.CompareTag("municion"))
            {
                Debug.Log("municion");
                StartCoroutine(Municion());                
            }
        }
    }
    
    IEnumerator Guindilla()
    {
        guindillaSound.Play();
        guindilla.SetTrigger("Touch");         
        velocityScr.guindilla = true;        
        referencess.moveSpeedr *= 1.5f;
        yield return new WaitForSeconds(time_guindilla);        
        velocityScr.guindilla = false;
        Invoke(nameof(Destroy), 1);        
    }
    IEnumerator Huevo()
    {
        stop = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy1>();
        huevoSound.Play();
        huevo.SetTrigger("Touch");        
        stop.huevo = true;
        yield return new WaitForSeconds(time_huevo);
        stop.huevo = false;
        Invoke(nameof(Destroy), 1);

    }
    IEnumerator Municion()
    {
        guindillaSound.Play();
        municion.SetTrigger("Touch");        
        munitionScr.bulletsLeft = munitionScr.magazineSize;
        yield return new WaitForSeconds(time_municion);
        Invoke(nameof(Destroy), 1);

    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
