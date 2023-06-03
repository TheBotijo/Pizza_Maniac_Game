using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class Shooting : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public float time_Damage = 0.5f;

    //bools 
    public bool shot; 
    bool shooting, readyToShoot, reloading;
    
    //Weapons
    public GameObject rodill;
    public GameObject pistola;
    public GameObject Ak;
    public bool rodillo =true;
    public bool pistol;
    public bool ak;

    //Reference
    public GameObject player;
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Sounds
    public AudioSource melee;
    public AudioSource pistolshoot;
    public AudioSource akshoot;

    //Graphics
    public GameObject bulletHoleGraphic;
    //public GameObject muzzleFlash;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    private PlayerInputMap _playerInput;
    private Enemy1 enemyDamage;
    //Animations
    public Animator animator;

    private void Start()
    {
        _playerInput = new PlayerInputMap();
        _playerInput.Juego.Enable();
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
        ChangeGun();
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = _playerInput.Juego.Shoot.IsPressed();
        else shooting = _playerInput.Juego.Shoot.WasPressedThisFrame();

        if (_playerInput.Juego.Reload.WasPressedThisFrame() && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && rodillo==false){
            bulletsShot = bulletsPerTap;
            shot = true;
            Shoot();
            Invoke(nameof(stop), 1);
        }
        else if (readyToShoot && shooting && !reloading && rodillo == true) 
        {
            shot = true;
            Shoot();         
            Invoke(nameof(stop), 1.5f);
            

        }
    }
    private void stop()
    {
        shot = false;
    }
    private void ChangeGun() 
    {
        if (_playerInput.Juego.ChangeGun.WasPressedThisFrame())
        {            
            if (rodillo == true)
            {
                rodill.gameObject.SetActive(false);
                pistola.gameObject.SetActive(true);
                pistol = true;
                rodillo = false;
                damage = 5;
                timeBetweenShooting = 1f;
                spread = 0f;
                range = 400f;
                reloadTime = 2f;
                timeBetweenShots = 1f;
            }
            else if (pistol == true)
            {
                pistola.gameObject.SetActive(false);
                Ak.gameObject.SetActive(true);
                pistol = false;
                ak = true;
                damage = 10;
                timeBetweenShooting = 2f;
                spread = 0f;
                range = 400f;
                reloadTime = 4f;
                timeBetweenShots = 2f;
            }
            else if (ak == true) 
            {
                Ak.gameObject.SetActive(false);
                rodill.gameObject.SetActive(true);
                rodillo = true;
                ak = false;
                damage = 0;
                timeBetweenShooting = 2f;
                spread = 0f;
                range = 0f;
                reloadTime = 0.5f;
                timeBetweenShots = 0.5f;
            }
        }
    }
    
    private void Shoot()
    {   //Animations
        if (rodillo == true)
        {
            melee.Play();
            player.GetComponent<BoxCollider>().enabled = true;
            animator.SetTrigger("melee");
        }
        else  if (pistol == true)
        {;
            akshoot.Play();
            bulletsLeft--;
            animator.SetTrigger("Pistol");
        }
        else if (ak == true)
        {
            pistolshoot.Play();
            bulletsLeft--;
            animator.SetTrigger("Ak");
        }
        

        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = attackPoint.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(attackPoint.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            if (rayHit.transform.tag == "Enemy")
            {
                //Destroy(rayHit.transform.gameObject);
                enemyDamage = rayHit.transform.gameObject.GetComponent<Enemy1>();
                enemyDamage.TakeDamage();
            }

            Debug.Log(rayHit.transform.tag);
            Debug.Log(rayHit.collider.name);
                                     
        }        
        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        
        Invoke("ResetShot", timeBetweenShooting);

        //if(bulletsShot > 0 && bulletsLeft > 0)
        //    Invoke("Shoot", timeBetweenShots);
    }
    //RodilloDamage
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy1>().TakeDamage();
            Debug.Log("DAÑANDO A enemigo con rodillo");
        }

    }
    private void ResetShot()
    {
        readyToShoot = true;
        player.GetComponent<BoxCollider>().enabled = false;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}