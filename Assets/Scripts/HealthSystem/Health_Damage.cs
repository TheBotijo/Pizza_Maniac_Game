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
    private float time_invencible = 1.5f, time_Stop = 1f, time_death = 3f;

    [Header("Scripts")]
    private HealthBar healthBar;
    private CameraController camScript;

    [Header("References")]
    public GameReferences referencess;
    [SerializeField] private Animator animator;
    private Camera thirddCam;
    [SerializeField] private GameObject deathUI;

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
        camScript = thirddCam.GetComponent<CameraController>();
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
            if (health <= 0)
            {
                StartCoroutine(Death());
            }
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
        death.Play();
        thirddCam.fieldOfView = 50;
        moving = false;
        animator.SetTrigger("Death");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathUI.SetActive(true);
        lose.Play();
        yield return new WaitForSeconds(time_death);
        camScript.enabled = false;
        Destroy(gameObject);

    }
    IEnumerator StopVelocity()
    {
        //Debug.Log("stop");
        moving = false;
        yield return new WaitForSeconds(time_Stop);
        moving = true;
    }
}
