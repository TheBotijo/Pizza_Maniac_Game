using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Health_Damage : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public bool invencible = false;
    public float time_invencible = 3f;
    public float time_Stop = 1f;
    public float time_death = 5f;
    public bool moving = true;

    [SerializeField] 
    public HealthBar healthBar;
    public Animator animator;
    public PlayerMoveJump player;
    public Camera thirddCam;
    public GameObject playerObj;

    public void Start()
    {
        health = maxHealth;
        healthBar.InitiateHealthBar(health);
    }

    public void loseHealth(int damage)
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
        yield return new WaitForSeconds(time_invencible);
        invencible = false;
    }
    IEnumerator Death()
    {
        thirddCam.fieldOfView = 50;
        moving = false;
        animator.SetTrigger("Death");
        playerObj.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<PlayerInput>().enabled = false;
        gameObject.GetComponent<PlayerMoveJump>().enabled = false;
        gameObject.GetComponent<Shooting>().enabled = false;
        Debug.Log("muerte");
        yield return new WaitForSeconds(time_death);
        Destroy(gameObject);
    }
    IEnumerator StopVelocity()
    {
        Debug.Log("stop");
        moving = false;
        yield return new WaitForSeconds(time_Stop);
        moving = true;
    }
}
