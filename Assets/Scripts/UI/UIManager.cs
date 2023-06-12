using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private GameReferences referencess;

    [Header("INGAME INFO")]
    [SerializeField] private TMP_Text TextoPizzas;
    [SerializeField] private TMP_Text TextoAmmo;
    [SerializeField] private TMP_Text TextoRounds;

    [Header("BOTONES UIs")]
    [SerializeField] private Button BackMenuBtn;
    [SerializeField] private Button ContinueBtn, RetryBtn,BackBtn2;
    [SerializeField] public GameObject FinalUI, Win, Loose;

    [Header("POSTGAME INFO")]
    [SerializeField] public TextMeshProUGUI textoBajas;
    [SerializeField] public TextMeshProUGUI textoTiempo, textoEntregas;
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
    Scene currentScene;
    string sceneName;
    private PlayerInputMap _playerInput;

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
        BackBtn2.onClick.AddListener(Back);
        RetryBtn.onClick.AddListener(Retry);
        ContinueBtn.onClick.AddListener(Continue);

        currentScene = SceneManager.GetActiveScene ();

        _playerInput = new PlayerInputMap();
        _playerInput.Juego.Enable();
    }

    

    // Update is called once per frame
    void Update()
    {
        //if (_playerInput.Juego.Jump.WasPressedThisFrame())
        //    Continue();

        if (finished == false)
        {
            timer += Time.deltaTime;
            FormatTimer();
        }

        TextoAmmo.SetText("Balas " + balas.bulletsLeft + " / " + balas.magazineSize);
        TextoPizzas.SetText("Pizzas Entregadas: " + pizzas.currentPizzas + " / " + pizzas.totalPizzas);
        TextoRounds.SetText("Ronda " + pizzas.rounds + " / 4");
    }
    void FormatTimer()
    {
        int hours = (int)(timer / 3600) % 24;
        int minutes = (int)(timer / 60) % 60;
        int seconds = (int)(timer % 60);
        timerText.text = "";
        if (hours > 0) { timerText.text = hours + "h"; }
        if (minutes > 0) { timerText.text += minutes + "min"; }
        if (seconds > 0) { timerText.text += seconds + "s"; }
    }

    private void Back()
    {
        SceneManager.LoadScene("Menu");       
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
        //Invoke(nameof(VallasOut), 1f);
        finalUIr.SetActive(false);
        winUIr.SetActive(false);

        string sceneName = currentScene.name;
 
        if (sceneName == "Mapa1"){
            SceneManager.LoadScene("Mapa2");
        } else if (sceneName == "Mapa2"){
            SceneManager.LoadScene("ZonaFinal");
        } else if (sceneName == "ZonaFinal"){
            SceneManager.LoadScene("Menu");  
        }          
    }
    //void VallasOut()
    //{
    //    vallas = GameObject.FindGameObjectWithTag("VallasFinal").GetComponentInChildren<Animator>();
    //    vallas.SetBool("disappear", true);
    //    Debug.Log("VAllas ALPHA OUT");
    //}
}