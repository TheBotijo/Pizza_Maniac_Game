using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy1 : MonoBehaviour
{
    //Scripts
    public Shooting takeDamage;
    private Drops drops;

    //Player Follow
    public Transform playerTrans;
    public GameObject playerr;
    public LayerMask whatIsGround, whatIsPlayer;

    //Enemy Stats
    Rigidbody rb;
    private Animator animator;
    public float speed;
    public float damage;
    public float healthh;
    Color original;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public Transform cos1;
    public GameObject ColliderMano;
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
        playerr = GameObject.Find("Player");
        takeDamage = playerr.GetComponent<Shooting>();
        rb = GetComponent<Rigidbody>();
        playerTrans = playerr.transform;
        drops = GetComponentInChildren<Drops>();
        takeDamage = playerr.GetComponent<Shooting>();
        original = cos1.GetComponentInChildren<Renderer>().material.color;
        animator = GetComponent<Animator>();
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
            animator.SetBool("moving", false);
        }
        transform.LookAt(playerTrans.position);
    }

    private void ChasePlayer()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, new Vector3(playerTrans.position.x, 0, playerTrans.position.z), speed * Time.deltaTime);
        rb.MovePosition(pos);
        transform.LookAt(pos);
        animator.SetBool("moving", true);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //punch.Play();
            other.GetComponent<Health_Damage>().LoseHealth(damage);
            Debug.Log("DAÑANDO A PLAYER");
        }
    }

    public void TakeDamage()
    {
        Debug.Log("DañoEnemigo");
        animator.SetTrigger("tookDamage");

        foreach (Transform child in cos1)
        {
            Color c = child.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
            colors.Add(c);
        }

        foreach (Transform child in cos1)
        {
            SkinnedMeshRenderer renderer = child.GetComponentInChildren<SkinnedMeshRenderer>();

            if (renderer != null)
            {
                renderer.material.color = new Color(255, 0, 0);
            }
        }
        
        Invoke(nameof(ColorBack), 0.2f);
        damag.Play();
        DeathPt.Play();
        if (healthh <= 0)
        {
            Vector3 pose = gameObject.transform.position;
            drops.DropSystem(pose);
            death.Play();
            takeDamage.Bajas();     
            Destroy(gameObject);
        }
        else
        {
            healthh -= takeDamage.damage;
        }
    }
    void ColorBack()
    {
        int i= 0;
        foreach (Transform child in cos1)
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
        ColliderMano.GetComponent<BoxCollider>().enabled = true;
        animator.SetTrigger("attacking");
        if (!alreadyAttacked)
        {
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
