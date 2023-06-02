using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    private Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;

    public Shooting takeDamage;

    public int damage = 5;
    public int Health = 50;
    Color original;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject cos;
    public GameObject ColliderMano;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

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
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            animator.SetBool("moving", true);

        }
        if (playerInSightRange && playerInAttackRange)
        {
            animator.SetBool("attacking", true);
            AttackPlayer();
            
        }
        else
        {
            animator.SetBool("moving", false);
            animator.SetBool("attacking", false);
            

        }
        //if (takeDamage.rayHit.collider.CompareTag("Enemy")) TakeDamage();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health_Damage>().loseHealth(damage);
            Debug.Log("DAÑANDO A PLAYER");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health_Damage>().loseHealth(damage);
        }
    }

    public void TakeDamage()
    {
        Debug.Log("DañoEnemigo");
        animator.SetTrigger("tookDamage");
        cos.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        Invoke(nameof(colorBack), 0.2f);
        Health -= 10;
        if (Health <= 0)
        {
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
        if (!alreadyAttacked)
        {
            ////Attack code
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ColliderMano.GetComponent<BoxCollider>().enabled = false;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
