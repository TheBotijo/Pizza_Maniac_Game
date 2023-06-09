using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy2 : MonoBehaviour
{
    //Scripts
    private Shooting takeDamage;
    private Drops drops;

    //Player Follow
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Enemy Stats
    Rigidbody rb;
    public Animator animator2;
    public float speed;
    public float damage = 10;
    public float Health = 14;
    Color original;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public GameObject cos;
    public GameObject drop;

    //Sounds
    public AudioSource damag;
    public AudioSource death;
    public AudioSource punch;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool huevo = false;
    public int bajas;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        drops = GetComponentInChildren<Drops>();
        takeDamage = player.GetComponent<Shooting>();
        original = cos.GetComponent<Renderer>().material.color;
        animator2 = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!huevo)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        }

        if (playerInSightRange && !playerInAttackRange && !huevo)
        {
            ChasePlayer();

        }
        else if (playerInAttackRange && !alreadyAttacked && !huevo)
        {
            AttackPlayer();
        }
        else
        {
            animator2.SetBool("movingg", false);
        }
    }

    private void ChasePlayer()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, 0, player.position.z), speed * Time.deltaTime);
        rb.MovePosition(pos);
        transform.LookAt(pos);
        animator2.SetBool("movingg", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            punch.Play();
            player.GetComponent<Health_Damage>().LoseHealth(damage);
            //Debug.Log("DA�ANDO A PLAYER");
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        GameObject.Find("Player").GetComponent<Health_Damage>().LoseHealth(damage);
    //    }
    //}

    public void TakeDamage()
    {
        //Debug.Log("Da�oEnemigo");
        animator2.SetTrigger("tookDamage");
        cos.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        Invoke(nameof(ColorBack), 0.2f);
        Health -= takeDamage.damage;
        damag.Play();
        if (Health <= 0)
        {
            Vector3 pose = gameObject.transform.position;
            drops.DropSystem(pose);
            death.Play();
            takeDamage.Bajas();
            Destroy(gameObject);
        }
    }
    void ColorBack()
    {
        cos.GetComponent<Renderer>().material.color = original;
    }
    private void AttackPlayer()
    {
        animator2.SetTrigger("attacking");
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
