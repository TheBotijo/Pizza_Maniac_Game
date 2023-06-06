using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    private Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;

    public Shooting takeDamage;

    public float damage = 5;
    public float Health = 20;
    Color original;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public GameObject cos;
    public GameObject ColliderMano;

    //Sounds
    public AudioSource damag;
    public AudioSource death;
    public AudioSource punch;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool huevo = false;

    private void Awake()
    {
        player = GameObject.Find("MainCharacter").transform;
        takeDamage = FindObjectOfType<Shooting>();
        original = cos.GetComponent<Renderer>().material.color;
        agent = GetComponent<NavMeshAgent>();
        animator= GetComponent<Animator>();
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
        if (other.tag == "Player")
        {
            punch.Play();
            GameObject.Find("Player").GetComponent<Health_Damage>().loseHealth(damage);
            //Debug.Log("DAÑANDO A PLAYER");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<Health_Damage>().loseHealth(damage);
        }
    }

    public void TakeDamage()
    {
        //Debug.Log("DañoEnemigo");
        animator.SetTrigger("tookDamage");
        cos.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        Invoke(nameof(colorBack), 0.2f);
        Health -= 10;
        damag.Play();
        if (Health <= 0)        
        {
            death.Play();
            //GetComponent<DropBag>().InstantiateDrop(transform.position);            
            Destroy(gameObject);
        }
    }
    void colorBack()
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
