using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

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
    bool shooting, readyToShoot, reloading;
    //Guns
    bool pistol=true, ak;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject bulletHoleGraphic;
    //public GameObject muzzleFlash;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    private PlayerInputMap _playerInput;

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
        Zoom();
        if (allowButtonHold) shooting = _playerInput.Juego.Shoot.IsPressed();
        else shooting = _playerInput.Juego.Shoot.WasPressedThisFrame();

        if (_playerInput.Juego.Reload.WasPressedThisFrame() && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void ChangeGun() 
    {
        if (_playerInput.Juego.ChangeGun.WasPressedThisFrame())
        {
            if (pistol == true)
            {
                pistol= false;
                ak = true;
                damage = 10;
                timeBetweenShooting = 3f;
                spread = 0f;
                range = 400f;
                reloadTime = 4f;
                timeBetweenShots = 3f;
            }
            else
            {
                pistol = true;
                ak = false;
                ak = true;
                damage = 5;
                timeBetweenShooting = 2f;
                spread = 0f;
                range = 200f;
                reloadTime = 2f;
                timeBetweenShots = 1f;
            }
        }
    }
    private void Zoom()
    {
        //Animations
        if (pistol == true && _playerInput.Juego.Aim.IsPressed())
        {
            animator.SetBool("SupportPistol",true);
        }else{ animator.SetBool("SupportPistol", false); }
        if (ak == true && _playerInput.Juego.Aim.IsPressed())
        {
            animator.SetBool("SupportAK", true);
        }else{ animator.SetBool("SupportAK", false); }
    }
    private void Shoot()
    {   //Animations
        if (pistol == true)
        {
            animator.SetTrigger("Pistol");
        }
        if (ak == true)
        {
            animator.SetTrigger("Ak");
        }
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log("Holiwis");
            Debug.Log(rayHit.collider.name);
                                     
        }        
        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }
    
    private void ResetShot()
    {
        readyToShoot = true;
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