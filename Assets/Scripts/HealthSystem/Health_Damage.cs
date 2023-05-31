using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Damage : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public bool invencible = false;
    public float time_invencible = 1f;
    public float time_Stop = 0.5f;
    public float time_death = 5f;
    public bool moving = true;

    [SerializeField] 
    public HealthBar healthBar;
    public Animator animator;
    public PlayerMoveJump player;
    public PlayerThirdCam thirddCam;

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
        thirddCam.thirdCam.m_Lens.FieldOfView = 20;
        moving = false;
        animator.SetTrigger("Death");
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
