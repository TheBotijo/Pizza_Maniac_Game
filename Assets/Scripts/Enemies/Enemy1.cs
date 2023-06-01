using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy1 : MonoBehaviour
{
    //public NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Shooting takeDamage;

    public int damage = 5;
    public int Health = 50;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("MainCharacter").transform;
        takeDamage = Object.FindObjectOfType<Shooting>();
        //agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
        if (takeDamage.rayHit.collider.CompareTag("Enemy")) TakeDamage(); 
    }

    private void ChasePlayer()
    {
        //agent.SetDestination(player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health_Damage>().loseHealth(damage);
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
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        Health -= 10;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        //agent.SetDestination(transform.position);

        transform.LookAt(player);

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
        alreadyAttacked = false;
    }
}
