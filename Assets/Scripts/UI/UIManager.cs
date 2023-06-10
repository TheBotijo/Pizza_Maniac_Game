using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    private GameReferences referencess;

    [Header("INGAME INFO")]
    [SerializeField] private TMP_Text TextoPizzas;
    [SerializeField] private TMP_Text TextoAmmo;
    [SerializeField] private TMP_Text TextoRounds;

    [Header("BOTONES UIs")]
    [SerializeField] private Button BackMenuBtn;
    [SerializeField] private Button ContinueBtn, RetryBtn;
    [SerializeField] public GameObject FinalUI, Win, Loose;

    [Header("POSTGAME INFO")]
    [SerializeField] public TextMeshProUGUI textoBajas;
    [SerializeField] public TextMeshProUGUI textoTiempo;
    private GameObject cineCamObj;
    private Animator vallas;
    private Camera cineCam;
    private Animator camAnim;
    public float timer;
    public TextMeshProUGUI timerText;
    private bool finished;
    private GameObject finalUIr;
    private GameObject winUIr;
    private GameObject loseUIr;

    //Scripts
    private PizzaDeliver pizzas;
    private Shooting balas;
    private Enemy1 enemy1; 

    private void Start()
    {
        referencess = GetComponent<GameReferences>();
        pizzas = referencess.deliverHere.GetComponent<PizzaDeliver>();
        finalUIr = pizzas.finalUI;
        winUIr = pizzas.winUI;
        //winUIr.SetActive(true);
        balas = referencess.playerr.GetComponent<Shooting>();
        enemy1 = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy1>();
        cineCamObj = referencess.cineCam;
        cineCam = cineCamObj.GetComponent<Camera>();
        finished = referencess.deliverHere.GetComponent<PizzaDeliver>().Finish;
        BackMenuBtn.onClick.AddListener(Back);
        RetryBtn.onClick.AddListener(Retry);
        ContinueBtn.onClick.AddListener(Continue);

    }

    

    // Update is called once per frame
    void Update()
    {
        if (finished == false)
            timer += Time.deltaTime;
        TextoAmmo.SetText("Ammo: " + balas.bulletsLeft + " / " + balas.magazineSize);
        TextoPizzas.SetText("Pizzas: " + pizzas.currentPizzas + " / " + pizzas.totalPizzas);
        TextoRounds.SetText("Round: " + pizzas.rounds + " / 4");
    }

    private void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Continue()
    {
        cineCamObj.SetActive(true);
        camAnim = cineCam.GetComponent<Animator>();
        camAnim.SetBool("vallasDisappear", true);
        Invoke(nameof(VallasOut), 1f);
        finalUIr.SetActive(false);
        winUIr.SetActive(false);
    }
    void VallasOut()
    {
        vallas = GameObject.FindGameObjectWithTag("VallasFinal").GetComponentInChildren<Animator>();
        vallas.SetBool("disappear", true);
        Debug.Log("VAllas ALPHA OUT");
    }
}
