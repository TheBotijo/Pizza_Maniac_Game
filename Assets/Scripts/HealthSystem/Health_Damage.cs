using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Health_Damage : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public bool invencible = false;
    public float time_invencible = 3f;
    public float time_Stop = 1f;
    public float time_death = 3f;
    public bool moving = true;

    [SerializeField] 
    public HealthBar healthBar;
    public Animator animator;
    public PlayerMoveJump player;
    public Camera thirddCam;
    public GameObject playerObj;
    public AudioSource damage;
    public AudioSource lose;
    public AudioSource death;
    public CameraController camScript;
    public GameObject deathUI;

    public void Start()
    {
        health = maxHealth;
        healthBar.InitiateHealthBar(health);
        camScript = GameObject.Find("CameraHold").GetComponent<CameraController>();
    }

    public void loseHealth(float damage)
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
