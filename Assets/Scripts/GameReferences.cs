using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameReferences : MonoBehaviour
{
    [Header("PLAYER")]
    public float moveSpeedr;
    public GameObject playerr;
    public GameObject playerrOrient;
    public GameObject playerrObj;
    public GameObject mainCam;
    public HealthBar healthBarScr;

    [Header("SPAWN SYSTEM")]
    public GameObject SpawnSystem;
    public GameObject deliverHere;
    public Image deliverImage;
    public Image pizzaImage;

    //[Header("UIs")]
    //public GameObject winUI;
    //public GameObject loseUI;

    [Header("WEAPONS")]
    public GameObject rodillo;
    public GameObject pistol;
    public GameObject Ak47;
    public LayerMask whatIsShootable;
    public GameObject bulletHoleGraphicr;
    public GameObject crosshair;

    [Header("CINEMATIC CAM")]
    public GameObject cineCam;

}
