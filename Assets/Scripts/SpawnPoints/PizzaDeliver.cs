using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PizzaDeliver : MonoBehaviour
{
    [SerializeField] private GameReferences referencess;
    [HideInInspector] public int currentPizzas = 0;
    [HideInInspector] public int totalPizzas;
    private SpawnPoints spawnPoint;
    private EnemySpawn spawnEnemy;
    private Shooting deadEnemy;
    private Health_Damage healthScr;
    private UIManager uiManager;
    private GameObject deliverHere;
    public AIEnemy1 enemy1;
    public AIEnemy2 enemy2;
    public AIEnemy3 enemy3;
    public int rounds = 0, totalDelivers;
    public AudioSource deliver;
    [Header("UIs")]
    public GameObject finalUI;
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject Other;
    public GameObject Fini;
    public AudioSource win;
    [HideInInspector] public bool Finish; 
    private TextMeshProUGUI textoBajass, textoTiempos, textoEntregass;
    private float timerr;
    private float timeReduce;
    private TextMeshProUGUI timerTextr;
    public PlayerInputMap playerInput;

    [Header("Particles")]
    public ParticleSystem Spawn;
    //public TextMeshPro repartirText;
    Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "ZonaFinal")
        {
            deliverHere.SetActive(false);
        }
        //Assignamos las referencias
        referencess = GetComponentInParent<GameReferences>();
        deliverHere = referencess.deliverHere;
        deadEnemy = referencess.playerr.GetComponent<Shooting>();
        healthScr = referencess.playerr.GetComponent<Health_Damage>();
        spawnPoint = referencess.SpawnSystem.GetComponent<SpawnPoints>();
        spawnEnemy = referencess.GetComponent<EnemySpawn>();
        uiManager = referencess.GetComponent<UIManager>();
        enemy1 = spawnEnemy.enemy1.GetComponent<AIEnemy1>();
        enemy2 = spawnEnemy.enemy2.GetComponent<AIEnemy2>();
        enemy3 = spawnEnemy.enemy3.GetComponent<AIEnemy3>();
        finalUI = uiManager.FinalUI;
        winUI = uiManager.Win;
        loseUI = uiManager.Loose;
        textoBajass = uiManager.textoBajas;
        textoTiempos = uiManager.textoTiempo;
        textoEntregass = uiManager.textoEntregas;
        timerTextr = uiManager.timerText;
        timerr = uiManager.timer;
        totalPizzas = 0;
        timeReduce = spawnEnemy.timeReduceMax;
        Spawn = gameObject.GetComponentInChildren<ParticleSystem>();
        Spawn.Play();
        playerInput = new PlayerInputMap();
        playerInput.Juego.Enable();
    }

    void FormatTimer()
    {
        int hours = (int)(timerr / 3600) % 24;
        int minutes = (int)(timerr / 60) % 60;
        int seconds = (int)(timerr % 60);

        if (hours > 0) { timerTextr.text += hours + "h"; }
        if (minutes > 0) { timerTextr.text += minutes + "min"; }
        if (seconds > 0) { timerTextr.text += seconds + "s"; }
    }

    private void OnTriggerEnter(Collider other)
    {

        string sceneName = currentScene.name;

        if (other.CompareTag("Player"))
        {
            if (sceneName == "ZonaFinal")
            {
                Debug.Log("C'rest fini");
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Other.SetActive(false);
                Fini.SetActive(true);
            }
            else
            {
                if (currentPizzas == totalPizzas - 1) //Perque reapareixi el punt al camió
                {
                    spawnPoint.entregadas = true;
                    currentPizzas++;
                }
                else if (currentPizzas == totalPizzas) //Per recollir les pizzes del camió i tornar a repartir
                {
                    spawnPoint.entregadas = false;
                    if (rounds == 0)
                    {
                        Debug.Log("Temps màxim a reduir: " + spawnEnemy.timeReduceMax);
                        Debug.Log("Multiplicador: " + spawnEnemy.timeReduce);
                        Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns1);
                        Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns2);
                        Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns3);
                        Debug.Log("Enemigos totales: " + spawnEnemy.enemyMax1);
                        Debug.Log("Vida enemigo: " + enemy1.Health);
                        totalPizzas = 5;
                    }
                    else
                    {
                        if (rounds == 1)
                            totalPizzas = 3;
                        if (rounds == 2)
                            totalPizzas = 2;
                        if (rounds == 3)
                            totalPizzas = 1;
                        if (rounds == 4)
                        {
                            FormatTimer();
                            Finish = true;
                            textoBajass.SetText("Bajas: " + deadEnemy.bajass);
                            textoTiempos.SetText("Tiempo: " + timerTextr.text);
                            textoEntregass.SetText("Entrtegas: " + totalDelivers);
                            Debug.Log(deadEnemy.bajass);
                            Debug.Log(timerTextr.text);
                            Debug.Log(totalDelivers);
                            healthScr.invencible = true;
                            finalUI.SetActive(true);
                            winUI.SetActive(true);
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                            win.Play();
                        }
                        currentPizzas = 0;

                        enemy1.Health += 7f;
                        enemy2.Health += 5f;
                        enemy3.Health += 6f;
                        spawnEnemy.timeBetweenSpawns1 -= spawnEnemy.timeReduceMax;
                        spawnEnemy.timeBetweenSpawns2 -= spawnEnemy.timeReduceMax;
                        spawnEnemy.timeBetweenSpawns3 -= spawnEnemy.timeReduceMax;
                        spawnEnemy.timeReduceMax -= timeReduce;
                        spawnEnemy.enemyMax1 += 5;
                        spawnEnemy.timeReduce -= 0.05f;
                        spawnEnemy.enemyCount1 = 0;
                        Debug.Log("Temps màxim a reduir: " + spawnEnemy.timeReduceMax);
                        Debug.Log("Multiplicador: " + spawnEnemy.timeReduce);
                        Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns1);
                        Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns2);
                        Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns3);
                        Debug.Log("Enemigos totales: " + spawnEnemy.enemyMax1);
                        Debug.Log("Vida enemigo: " + enemy1.Health);
                    }
                    rounds++;

                    //Debug.Log("Enemy max: " + spawnEnemy.enemyMax1);

                    currentPizzas = 0;
                }
                else  //Repartint pizzes normals
                {
                    deliver.Play();
                    currentPizzas++;
                    totalDelivers++;
                }

                spawnPoint.Respawn(deliverHere);
            }
            
        }
    }
}
