using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PizzaDeliver : MonoBehaviour
{
    [SerializeField] private GameReferences referencess;
    [HideInInspector] public int currentPizzas = 0;
    [HideInInspector] public int totalPizzas;
    private SpawnPoints spawnPoint;
    private EnemySpawn spawnEnemy;
    private Shooting deadEnemy;
    private Health_Damage healthScr;
    private GameObject deliverHere;
    public Enemy1 enemy1;
    public int rounds = 0;
    public AudioSource deliver;
    [Header("UIs")]
    public GameObject finalUI;
    public GameObject winUI;
    public GameObject loseUI;
    public AudioSource win;
    [HideInInspector] public bool Finish; 
    private TextMeshProUGUI textoBajass;
    private TextMeshProUGUI textoTiempos;
    private float timerr;
    private TextMeshProUGUI timerTextr;
    //public TextMeshPro repartirText;

    private void Start()
    {
        //Assignamos las referencias
        referencess = GetComponentInParent<GameReferences>();
        deliverHere = referencess.deliverHere;
        finalUI = referencess.GetComponent<UIManager>().FinalUI;
        deadEnemy = finalUI.GetComponent<Shooting>();
        textoBajass = referencess.GetComponent<UIManager>().textoBajas;
        textoTiempos = referencess.GetComponent<UIManager>().textoTiempo;
        timerTextr = referencess.GetComponent<UIManager>().timerText;
        timerr = referencess.GetComponent<UIManager>().timer;
        healthScr = referencess.playerr.GetComponent<Health_Damage>();
        winUI = referencess.GetComponent<UIManager>().Win;
        spawnPoint = referencess.SpawnSystem.GetComponent<SpawnPoints>();
        spawnEnemy = referencess.GetComponent<EnemySpawn>();
        totalPizzas = 0;
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
        if (other.CompareTag("Player"))
        {
            if (currentPizzas == totalPizzas - 1)
            {
                spawnPoint.entregadas = true;
                currentPizzas++;
            }
            else if (currentPizzas == totalPizzas)
            {
                spawnPoint.entregadas = false;
                if (rounds == 0)
                {
                    Debug.Log("Temps màxim a reduir: " + spawnEnemy.timeReduceMax);
                    Debug.Log("Multiplicador: " + spawnEnemy.timeReduce);
                    Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns);
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
                        healthScr.invencible = true;
                        finalUI.SetActive(true);
                        winUI.SetActive(true);
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        win.Play();
                        textoBajass.SetText("Bajas: " + deadEnemy.bajass);
                        textoTiempos.SetText("Tiempo: " +  timerTextr.text);
                    }
                    currentPizzas = 0;

                    enemy1.Health += 7.5f;
                    spawnEnemy.timeBetweenSpawns -= 1.5f;
                    spawnEnemy.timeReduceMax -=1.5f;
                    spawnEnemy.enemyMax1 += 5;
                    spawnEnemy.timeReduce -= 0.05f;
                    spawnEnemy.enemyCount1 = 0;
                    Debug.Log("Temps màxim a reduir: " + spawnEnemy.timeReduceMax);
                    Debug.Log("Multiplicador: " + spawnEnemy.timeReduce);
                    Debug.Log("Temps entre spawns: " + spawnEnemy.timeBetweenSpawns);
                    Debug.Log("Enemigos totales: " + spawnEnemy.enemyMax1); 
                    Debug.Log("Vida enemigo: " + enemy1.Health);
                }
                rounds++;
                
                //Debug.Log("Enemy max: " + spawnEnemy.enemyMax1);

                currentPizzas = 0;
            }
            else { deliver.Play(); currentPizzas++; }
                
            spawnPoint.Respawn(deliverHere);
        }
    }
}
