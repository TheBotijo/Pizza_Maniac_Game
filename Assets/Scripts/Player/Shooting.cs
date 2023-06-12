using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

public class Shooting : MonoBehaviour
{
    private GameReferences referencess;

    [Header("Gun Stats")]
    [HideInInspector] public float reloadTime;
    [HideInInspector] public int pistolBulletsLeft, akBulletsLeft, pistolBulletsLeft2, akBulletsLeft2, bulletsShot, pistolMagazineSize = 20, akMagazineSize = 40, damage;
    private bool allowButtonHold;
    [SerializeField] private int bulletsPerTap;
    public float timeBetweenShooting, timeBetweenShots;
    private float range;
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
    public GameObject deliverHere;
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
    public AudioSource pistolshoot, akshoot, macarena;

    [Header("Graphics")]
    private GameObject bulletHoleGraphic;
    //public GameObject muzzleFlash;
    public float camShakeMagnitude, camShakeDuration;
    Scene currentScene;
    public GameObject questpointer;

    [Header("Animations")]
    public Animator animator;
    private Animator Macaanimator;
    private Rigidbody enemyRb;
    int counter = 0;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();

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
        string sceneName = currentScene.name;

        if (sceneName == "ZonaFinal") //si estem a la ronda final, el shot afecta diferent
        {
            questpointer.SetActive(false);
        }

        pistolMagazineSize = 20;
        akMagazineSize = 40;
        pistolBulletsLeft2 = pistolMagazineSize;
        akBulletsLeft2 = akMagazineSize;
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
        //Zona de debug per comprovar on estem apuntant
        centerScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ray = Camera.main.ScreenPointToRay(centerScreen);
        if (Physics.Raycast(ray, out rayHit, range, whatIsEnemy))
        {
            string sceneName = currentScene.name;
            if (sceneName != "ZonaFinal")
            {
                //debugTransform.position = rayHit.point;
            }
        }
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = _playerInput.Juego.Shoot.IsPressed();
        else shooting = _playerInput.Juego.Shoot.WasPressedThisFrame();

        // if (_playerInput.Juego.Reload.WasPressedThisFrame() && bulletsLeft < magazineSize) Reload();

        //Comprovem si el player pot atacar, depenent de les seves 
        if (readyToShoot && shooting && pistolBulletsLeft > 0 && pistol == true)
        {
            bulletsShot = bulletsPerTap;
            shot = true;
            Shoot();
            Invoke(nameof(Stop), 1);
        }

        else if (readyToShoot && shooting && akBulletsLeft > 0 && ak == true)
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
                Debug.Log(pistolMagazineSize);
                pistolBulletsLeft = pistolBulletsLeft2;
                referencess.crosshair.SetActive(true);
                rodill.SetActive(false);
                pistola.SetActive(true);
                pistol = true;
                rodillo = false;
                damage = 7;
                timeBetweenShooting = 1f;
                //spread = 0f;
                range = 300f;
                //reloadTime = 2f;
                timeBetweenShots = 1f;
            }
            else if (pistol == true) // si la pistola esta activa, s'activa la AK
            {
                Debug.Log(akMagazineSize);
                pistolBulletsLeft2 = pistolBulletsLeft;
                akBulletsLeft = akBulletsLeft2;
                pistola.SetActive(false);
                Ak.SetActive(true);
                pistol = false;
                ak = true;
                allowButtonHold = true;
                damage = 3;
                timeBetweenShooting = 0.3f;
                //spread = 0f;
                range = 600f;
                //reloadTime = 4f;
                timeBetweenShots = 2f;
            }
            else if (ak == true) //Si esta la AK activa, s'activa el Rodillo
            {
                akBulletsLeft2 = akBulletsLeft;
                referencess.crosshair.SetActive(false);
                Ak.SetActive(false);
                rodill.SetActive(true);
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
            Invoke(nameof(Meleee), 0.5f);
            Invoke(nameof(ResetShot), 1f);
        }
        else if (pistol == true)
        {
            akshoot.Play();
            pistolBulletsLeft--;
            animator.SetTrigger("Pistol");
        }
        else if (ak == true)
        {
            pistolshoot.Play();
            akBulletsLeft--;
            animator.SetTrigger("Ak");
        }

        readyToShoot = false;

        // Pillem el nom de l'escena actual
        string sceneName = currentScene.name;

        // RayCast
        if (Physics.Raycast(ray, out rayHit, range, whatIsEnemy))
        {
            GameObject gameRay;
            if (rayHit.transform.CompareTag("Enemy"))
            {
                gameRay = rayHit.transform.gameObject;

                if (sceneName != "ZonaFinal") //si estem a la ronda final, el shot afecta diferent
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
                }
                else
                {
                    int r, g, b;
                    Macaanimator = gameRay.GetComponent<Animator>();
                    gameRay.transform.localScale = new Vector3(1,1,1);
                    enemyRb = gameRay.GetComponent<Rigidbody>();
                    Macaanimator.SetTrigger("macarena");
                    if (counter == 1)
                    {
                        macarena.Play();
                    }
                    enemyRb.isKinematic = true;
                    enemyRb.constraints = RigidbodyConstraints.FreezePosition;

                    foreach (Transform child in rayHit.collider.gameObject.transform)
                    {
                        SkinnedMeshRenderer renderer = child.GetComponentInChildren<SkinnedMeshRenderer>();
                        r = Random.Range(20, 200);
                        g = Random.Range(20, 200);
                        b = Random.Range(20, 200);
                        if (renderer != null)
                        {
                            renderer.material.color = new Color(r, g, b);
                        }
                    }
                    counter++;
                    Debug.Log(counter);
                    if (counter == 5)
                    {
                        deliverHere.SetActive(true);
                        questpointer.SetActive(true);
                    }
                }
            }
        }
        // Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        // Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        Invoke(nameof(ResetShot), timeBetweenShooting);

    }

    void Meleee()
    {
        string sceneName = currentScene.name;
        if (sceneName != "ZonaFinal")
        {
            if (rodillo == true)
            {
                player.GetComponentInChildren<CapsuleCollider>().enabled = true;
            }
        }
    }

    public void ResetShot()
    {
        readyToShoot = true;

        if (rodillo == true)
            player.GetComponentInChildren<CapsuleCollider>().enabled = false;
    }
}