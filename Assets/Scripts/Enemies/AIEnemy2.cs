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
    public float damage;
    public float Health;
    Color original;

    //Attacking
    [SerializeField]
    private ParticleSystem Pinchos;
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public Transform cos;
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
        original = cos.GetComponentInChildren<Renderer>().material.color;
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
        transform.LookAt(player.position);
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
            //Debug.Log("DAÑANDO A PLAYER");
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
        //Debug.Log("DañoEnemigo");
        animator2.SetTrigger("tookDamage");
        //cos.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        foreach (Transform child in cos)
        {
            Color a = child.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
            colors.Add(a);
        }

        foreach (Transform child in cos)
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
        //cos.GetComponent<Renderer>().material.color = original;
        foreach (Transform child in cos)
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
        animator2.SetTrigger("attacking");
        Pinchos.Play();
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
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health_Damage>().LoseHealth(10);
            Debug.Log("Esporas");
        }
    }
}
