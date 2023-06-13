using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Health_Damage : MonoBehaviour
{
    [Header("Health values")]
    [Range(0,100)]public float health;
    //private float maxHealth = 100;
    public bool invencible = false, moving = true;
    private float time_invencible = 0.2f, time_Stop = 1f, time_death = 3f;

    [Header("Scripts")]
    private HealthBar healthBar;

    [Header("References")]
    public GameReferences referencess;
    public PizzaDeliver finScr;
    [SerializeField] private Animator animator;
    private Camera thirddCam;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject finalUI;

    [Header("Sounds")]
    [SerializeField] private AudioSource damage;
    [SerializeField] private AudioSource lose, death;

    public void Start()
    {
        //health = maxHealth;
        //Assignamos las referencias
        referencess = GetComponentInParent<GameReferences>();
        healthBar = referencess.healthBarScr;
        thirddCam = referencess.mainCam.GetComponent<Camera>();
        healthBar.InitiateHealthBar(health);
    }

    public void LoseHealth(float damage)
    {
        if(!invencible && health > 0)
        {
            health -= damage;
            healthBar.ChangeActualHealth(health);
            StartCoroutine(Invulnerability());
            animator.SetTrigger("TakeDamage");
            StartCoroutine(StopVelocity());
            
        }
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    public void HealthHeart()
    {
        if (health < 50)
        {
            health += 7;
        }
        healthBar.ChangeActualHealth(health);
    }

    IEnumerator Invulnerability()
    {
        invencible = true;
        damage.Play();
        yield return new WaitForSeconds(time_invencible);
        invencible = false;
    }
    IEnumerator Death()
    {
        Rigidbody playerrb;
        thirddCam.fieldOfView = 50;
        finScr.variablesOnScreen();
        moving = false;
        animator.SetTrigger("Death");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        finalUI.SetActive(true);
        deathUI.SetActive(true);
        lose.Play();
        playerrb= gameObject.GetComponent<Rigidbody>();
        playerrb.isKinematic = true;
        playerrb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(time_death);

    }
    IEnumerator StopVelocity()
    {
        //Debug.Log("stop");
        moving = false;
        yield return new WaitForSeconds(time_Stop);
        moving = true;
    }
}
