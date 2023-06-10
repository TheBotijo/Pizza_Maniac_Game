using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    public NavMeshAgent agent;

    private GameObject playerr;
    private Transform player;
    private Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;

    private Shooting takeDamage;
    private Drops drops;

    public float damage = 10;
    public float Health = 14;
    Color original;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public GameObject cos;
    public GameObject ColliderMano;
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

    private void Awake()
    {
        playerr = GameObject.FindGameObjectWithTag("Player");
        player = playerr.transform;
        drops = GetComponent<Drops>();
        takeDamage = playerr.GetComponent<Shooting>();
        original = cos.GetComponent<Renderer>().material.color;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!huevo) 
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
            animator.SetBool("moving", false);           
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("moving", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            punch.Play();
            GameObject.Find("Player").GetComponent<Health_Damage>().LoseHealth(damage);
            //Debug.Log("DAÑANDO A PLAYER");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("Player").GetComponent<Health_Damage>().LoseHealth(damage);
        }
    }

    public void TakeDamage()
    {
        //Debug.Log("DañoEnemigo");
        animator.SetTrigger("tookDamage");
        cos.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        Invoke(nameof(ColorBack), 0.2f);
        Health -= takeDamage.damage;
        damag.Play();
        if (Health <= 0)     
        {
            Vector3 pose = gameObject.transform.position;
            //drop = GameObject.FindGameObjectWithTag("guindilla");
            drops.DropSystem(pose);
            //Instantiate(Resources.Load("guindilla"), gameObject.transform.position + new Vector3(0,3,0), Quaternion.identity);
            death.Play();
            //GetComponent<DropBag>().InstantiateDrop(transform.position); 
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
        //Make sure enemy doesn't move
        //agent.SetDestination(transform.position);
        //transform.LookAt(player);

        ColliderMano.GetComponent<BoxCollider>().enabled = true;
        animator.SetTrigger("attacking");
        if (!alreadyAttacked)
        {
            ////Attack code
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            
        }
    }

    private void ResetAttack()
    {
        ColliderMano.GetComponent<BoxCollider>().enabled = false;
        alreadyAttacked = false;
    }
}
