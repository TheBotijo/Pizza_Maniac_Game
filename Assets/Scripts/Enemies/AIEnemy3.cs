using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy3 : MonoBehaviour
{
    //Scripts
    private Shooting takeDamage;
    private Drops drops;

    //Player Follow
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Enemy Stats
    Rigidbody rb;
    public Animator animator3;
    public float speed;
    public float damage = 10;
    public float Health = 14;
    Color original;

    //Attacking
    [SerializeField] 
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform[] projectileSpawnPoint;
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public Transform cos3;
    public GameObject drop;
    List<MeshRenderer> renderers = new List<MeshRenderer>();
    List<Color> colors = new List<Color>();

    //Sounds
    public AudioSource damag;
    public AudioSource death;
    public AudioSource punch;

    //States
    [SerializeField]
    private ParticleSystem DeathPt;
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
        original = cos3.GetComponentInChildren<Renderer>().material.color;
        animator3 = GetComponent<Animator>();
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
            animator3.SetBool("moving", false);
        }
    }

    private void ChasePlayer()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, 0, player.position.z), speed * Time.deltaTime);
        rb.MovePosition(pos);
        transform.LookAt(pos);
        animator3.SetBool("moving", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            punch.Play();
            player.GetComponent<Health_Damage>().LoseHealth(damage);
            //Debug.Log("DAÑANDO A PLAYER");
        }
    }

    public void TakeDamage()
    {
        //Debug.Log("DañoEnemigo");
        animator3.SetTrigger("tookDamage");
        //cos.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        Health -= takeDamage.damage;
        damag.Play();
        foreach (Transform child in cos3)
        {
            Color b = child.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
            colors.Add(b);
        }

        foreach (Transform child in cos3)
        {
            SkinnedMeshRenderer renderer = child.GetComponentInChildren<SkinnedMeshRenderer>();

            if (renderer != null)
            {
                renderer.material.color = new Color(255, 0, 0);
            }
        }
        Invoke(nameof(ColorBack), 0.2f);
        Health -= takeDamage.damage;
        damag.Play();
        DeathPt.Play();
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
        int i = 0;
        foreach (Transform child in cos3)
        {
            SkinnedMeshRenderer renderer = child.GetComponentInChildren<SkinnedMeshRenderer>();

            if (renderer != null)
            {
                renderer.material.color = colors[i];
                i++;
            }
        }
    }
    private void AttackPlayer()
    {
        animator3.SetTrigger("attacking");
        Invoke(nameof(Throw), 1.5f);        
            
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
    private void Throw()
    {
        foreach (Transform SpawnPoints in projectileSpawnPoint)
        {
            Instantiate(projectilePrefab, SpawnPoints.position, transform.rotation);
        }
    }
}
