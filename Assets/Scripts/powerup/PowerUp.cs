using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameReferences referencess;
    private PlayerMoveJump velocityScr;
    private Shooting munitionScr;
    public GameObject enemies;
    private Rigidbody stop;

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

    [Header("Particles")]
    public ParticleSystem guindillafart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            velocityScr = other.GetComponent<PlayerMoveJump>();
            referencess = other.GetComponent<PlayerMoveJump>().references;
            munitionScr = other.GetComponent<Shooting>();
            guindillafart = other.GetComponentInChildren<ParticleSystem>();

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
        guindillafart.Play();
        guindillaSound.Play();
        guindilla.SetTrigger("Touch");
        velocityScr.guindilla = true;
        referencess.moveSpeedr *= 2f;
        yield return new WaitForSeconds(time_guindilla);
        velocityScr.guindilla = false;
        Invoke(nameof(Destroy), 1);
        guindillafart.Stop();
    }
    IEnumerator Huevo()
    {
        stop = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody>();
        
        huevoSound.Play();
        huevo.SetTrigger("Touch");
        stop.isKinematic = true;
        Debug.Log("CONGELASSION2");
        stop.constraints = RigidbodyConstraints.FreezePosition;

        Debug.Log("CONGELASSION");

        //foreach (GameObject _stop in enemies)
        //{
        //    Rigidbody rb;
        //    rb = _stop.GetComponent<Rigidbody>();
        //    rb.isKinematic = true;
        //    Debug.Log("CONGELASSION2");
        //    rb.constraints = RigidbodyConstraints.FreezePosition;
        //}
        
        yield return new WaitForSeconds(time_huevo);

        stop.isKinematic = false;
        Debug.Log("CONGELASSION3");
        stop.constraints = RigidbodyConstraints.None;
        //foreach (GameObject _stop in enemies)
        //{
        //    Rigidbody rb;
        //    rb = _stop.GetComponent<Rigidbody>();
        //    rb.isKinematic = true;
        //    Debug.Log("CONGELASSION2");
        //    rb.constraints = RigidbodyConstraints.FreezePosition;
        //}
        Invoke(nameof(Destroy), 1);

    }
    IEnumerator Municion()
    {
        //guindillaSound.Play();
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
