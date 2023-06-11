using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class Shooting : MonoBehaviour
{
    private GameReferences referencess;

    [Header("Gun Stats")]
    [HideInInspector] public float reloadTime;
    [HideInInspector] public int bulletsLeft, bulletsShot, magazineSize, damage;
    private bool allowButtonHold;
    [SerializeField] private int bulletsPerTap;
    public float timeBetweenShooting;
    private float range, timeBetweenShots;
    //private float spread;

    [Header("Bools")]
    [HideInInspector] public bool shot, readyToShoot;
    bool shooting, reloading;

    [Header("Weapons")]
    private GameObject rodill;
    private GameObject pistola;
    private GameObject Ak;
    [HideInInspector] public bool rodillo = true;
    [HideInInspector] public bool pistol;
    [HideInInspector] public bool ak;

    [Header("Camera")]
    [HideInInspector] public bool aiming;
    private Camera fpsCam;
    private float fieldOfView;
    private float maxfield;
    Vector2 centerScreen;
    Ray ray;

    [Header("References")]
    private GameObject player;
    // public Transform attackPoint;
    public RaycastHit rayHit;
    private LayerMask whatIsEnemy;
    [SerializeField]
    private Transform debugTransform;
    private PlayerInputMap _playerInput;
    public AIEnemy1 enemyDamage1;
    public AIEnemy2 enemyDamage2;
    public AIEnemy3 enemyDamage3;
    public int bajass;

    [Header("Sounds")]
    public AudioSource melee;
    public AudioSource pistolshoot, akshoot;

    [Header("Graphics")]
    private GameObject bulletHoleGraphic;
    //public GameObject muzzleFlash;
    public float camShakeMagnitude, camShakeDuration;

    [Header("Animations")]
    public Animator animator;

    private void Start()
    {
        referencess = GetComponentInParent<GameReferences>();
        player = referencess.playerrObj;
        rodill = referencess.rodillo;
        pistola = referencess.pistol;
        Ak = referencess.Ak47;
        whatIsEnemy = referencess.whatIsShootable;
        fpsCam = referencess.mainCam.GetComponent<Camera>();
        bulletHoleGraphic = referencess.bulletHoleGraphicr;


        fieldOfView = Camera.main.fieldOfView;
        maxfield = fpsCam.fieldOfView / 2;
        _playerInput = new PlayerInputMap();
        _playerInput.Juego.Enable();
    }
    public void Bajas()
    {
        bajass++;
        Debug.Log("Bajas = " + bajass);
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
        LookAtShoot();
    }

    private void LookAtShoot()
    {
        centerScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ray = Camera.main.ScreenPointToRay(centerScreen);
        if (Physics.Raycast(ray, out rayHit, range, whatIsEnemy))
        {
            debugTransform.position = rayHit.point;
        }
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = _playerInput.Juego.Shoot.IsPressed();
        else shooting = _playerInput.Juego.Shoot.WasPressedThisFrame();

        ///if (_playerInput.Juego.Reload.WasPressedThisFrame() && bulletsLeft < magazineSize) Reload();

        //Shoot
        if (readyToShoot && shooting && bulletsLeft > 0 && rodillo == false)
        {
            bulletsShot = bulletsPerTap;
            shot = true;
            Shoot();
            Invoke(nameof(Stop), 1);
        }

        else if (readyToShoot && shooting && rodillo == true)
        {
            shot = true;
            Shoot();
            Invoke(nameof(Stop), 1.5f);
        }

        if (_playerInput.Juego.Aim.IsPressed())
        {
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, maxfield, 10f * Time.deltaTime);
            aiming = true;

        }
        else
        {
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, fieldOfView, 10f * Time.deltaTime);
            aiming = false;
        }
    }
    private void Stop()
    {
        shot = false;
    }
    private void ChangeGun()
    {
        if (_playerInput.Juego.ChangeGun.WasPressedThisFrame())
        {
            if (rodillo == true) // Quan apretes la Q i el rodillo està activat, s'activen les stats de la següent arma
            {
                rodill.gameObject.SetActive(false);
                pistola.gameObject.SetActive(true);
                pistol = true;
                rodillo = false;
                damage = 7;
                timeBetweenShooting = 0.5f;
                //spread = 0f;
                range = 300f;
                //reloadTime = 2f;
                timeBetweenShots = 1f;
            }
            else if (pistol == true)
            {
                pistola.gameObject.SetActive(false);
                Ak.gameObject.SetActive(true);
                pistol = false;
                ak = true;
                allowButtonHold = true;
                damage = 3;
                timeBetweenShooting = 1f;
                //spread = 0f;
                range = 600f;
                //reloadTime = 4f;
                timeBetweenShots = 2f;
            }
            else if (ak == true)
            {
                Ak.gameObject.SetActive(false);
                rodill.gameObject.SetActive(true);
                rodillo = true;
                ak = false;
                allowButtonHold = false;
                damage = 5;
                timeBetweenShooting = 2f;
                //spread = 0f;
                range = 0f;
                //reloadTime = 0f;
                timeBetweenShots = 0.5f;
            }
        }
    }
    private void Shoot()
    {   //Animations
        if (rodillo == true)
        {
            melee.Play();
            animator.SetTrigger("melee");
            Invoke(nameof(meleee), 0.5f);
            Invoke(nameof(ResetShot), 1f);
        }
        else if (pistol == true)
        {
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

        // Spread
        //float x = Random.Range(-spread, spread);
        //float y = Random.Range(-spread, spread);

        // Calculate Direction with Spread
        // Vector3 direction = attackPoint.transform.forward + new Vector3(x, y, 0);

        // RayCast
        if (Physics.Raycast(ray, out rayHit, range, whatIsEnemy))
        {
            if (rayHit.transform.CompareTag("Enemy"))
            {
                Debug.Log(rayHit.collider.name);
                if (rayHit.collider.name == "BichoQueso(Clone)")
                {
                    enemyDamage1 = rayHit.transform.gameObject.GetComponent<AIEnemy1>();
                    enemyDamage1.TakeDamage();
                }
                else if (rayHit.collider.name == "BichoSeta(Clone)")
                {
                    enemyDamage2 = rayHit.transform.gameObject.GetComponent<AIEnemy2>();
                    enemyDamage2.TakeDamage();
                }
                else if (rayHit.collider.name == "BichoTomate(Clone)")
                {
                    enemyDamage3 = rayHit.transform.gameObject.GetComponent<AIEnemy3>();
                    enemyDamage3.TakeDamage();
                }
                // Destroy(rayHit.transform.gameObject);
            }
            // Debug.Log(rayHit.transform.tag);
            // Debug.Log(rayHit.collider.name);
        }
        // Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        // Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        Invoke(nameof(ResetShot), timeBetweenShooting);

        // if(bulletsShot > 0 && bulletsLeft > 0)
        //    Invoke("Shoot", timeBetweenShots);
    }

    void meleee()
    {
        player.GetComponentInChildren<CapsuleCollider>().enabled = true;
    }
   
    public void ResetShot()
    {
        readyToShoot = true;
        player.GetComponentInChildren<CapsuleCollider>().enabled = false;
    }
    //private void Reload()
    //{
    //    reloading = true;
    //    Invoke(nameof(ReloadFinished), reloadTime);
    //}
    //private void ReloadFinished()
    //{
    //    bulletsLeft = magazineSize;
    //    reloading = false;
    //}
}