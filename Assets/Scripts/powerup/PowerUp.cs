using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameReferences referencess;
    private PlayerMoveJump velocityScr;
    private Shooting munitionScr;
    private Health_Damage health;
    public GameObject enemies;
    private Rigidbody[] stop;

    [Header("Sounds")]
    [SerializeField] private AudioSource guindillaSound;
    [SerializeField] private AudioSource huevoSound;

    [Header("Animations")]
    private Animator guindilla;
    private Animator huevo, municion, corazon;

    [Header("Timings")]
    [SerializeField] private float time_guindilla = 8f;
    [SerializeField] private float time_huevo = 8f;
    [SerializeField] private float time_municion = 1f;
    [SerializeField] private float time_cora = 1f;
    [SerializeField] private float time_leva = 10f;

    [Header("Particles")]
    public ParticleSystem guindillafart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            velocityScr = other.GetComponent<PlayerMoveJump>();
            referencess = other.GetComponentInParent<GameReferences>();
            health = other.GetComponent<Health_Damage>();
            munitionScr = other.GetComponent<Shooting>();
            guindillafart = other.GetComponentInChildren<ParticleSystem>();

            if (gameObject.CompareTag("guindilla"))
            {
                guindilla = gameObject.GetComponent<Animator>();
                Debug.Log("guindilla");
                StartCoroutine(Guindilla());
            }
            if (gameObject.CompareTag("huevo"))
            {
                huevo = gameObject.GetComponent<Animator>();
                Debug.Log("huevo");
                StartCoroutine(Huevo());
            }
            if (gameObject.CompareTag("municion"))
            {
                municion = gameObject.GetComponent<Animator>();
                Debug.Log("municion");
                StartCoroutine(Municion());
            }
            if (gameObject.CompareTag("PizzaCora"))
            {
                corazon = gameObject.GetComponent<Animator>();
                Debug.Log("pizzacor");
                StartCoroutine(Cora());
            }
            if (gameObject.CompareTag("levadura"))
            {
                corazon = gameObject.GetComponent<Animator>();
                Debug.Log("levadura");
                StartCoroutine(Leva());
            }
        }
    }

    IEnumerator Guindilla()
    {
        guindillafart.Play();
        guindillaSound.Play();
        guindilla.SetTrigger("Touch");
        velocityScr.guindilla = true;
        velocityScr.guindSpeed = 3;
        yield return new WaitForSeconds(time_guindilla);
        velocityScr.guindilla = false;
        Invoke(nameof(Destroy), 1);
        guindillafart.Stop();
    }
    IEnumerator Huevo()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Rigidbody[] enemiesRigidbody = new Rigidbody[enemies.Length];

        for (int i = 0; i < enemiesRigidbody.Length; i++)
        {
            enemiesRigidbody[i] = enemies[i].GetComponent<Rigidbody>();

            if (enemiesRigidbody[i] != null)
            {
                enemiesRigidbody[i].isKinematic = true;
                enemiesRigidbody[i].constraints = RigidbodyConstraints.FreezePosition;
            }
        }

        huevoSound.Play();
        huevo.SetTrigger("Touch");
        Debug.Log("CONGELASSION2");
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

        for (int i = 0; i < enemiesRigidbody.Length; i++)
        {
            if (enemiesRigidbody[i] != null)
            {
                enemiesRigidbody[i].isKinematic = false;
                enemiesRigidbody[i].constraints = RigidbodyConstraints.None;
            }
        }


        Debug.Log("CONGELASSION3");

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

        municion.SetTrigger("Touch");
        munitionScr.bulletsLeft = munitionScr.magazineSize;
        yield return new WaitForSeconds(time_municion);
        Invoke(nameof(Destroy), 1);
    }
    IEnumerator Cora()
    {

        corazon.SetTrigger("Touch");
        health.HealthHeart();
        yield return new WaitForSeconds(time_cora);
        Invoke(nameof(Destroy), 1);
    }
    IEnumerator Leva()
    {
        corazon.SetTrigger("Touch");
        velocityScr.GetComponent<Transform>().localScale *= 3;
        velocityScr.playerHeight *= 3;
        velocityScr.xSpeed *= 3;
        yield return new WaitForSeconds(time_leva);
        velocityScr.GetComponent<Transform>().localScale /= 3;
        velocityScr.playerHeight /= 3;
        velocityScr.xSpeed /= 3;
        Invoke(nameof(Destroy), 1);
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}

