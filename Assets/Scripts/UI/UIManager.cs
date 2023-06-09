using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    private GameReferences referencess;

    [Header("INGAME INFO")]
    //Texts
    [SerializeField]
    private TMP_Text TextoPizzas;
    [SerializeField]
    private TMP_Text TextoAmmo;
    [SerializeField]
    private TMP_Text TextoRounds;

    //Scripts
    private PizzaDeliver pizzas;
    private Shooting balas;

    private void Start()
    {
        referencess = GetComponent<GameReferences>();
        pizzas = referencess.deliverHere.GetComponent<PizzaDeliver>();
        balas = referencess.playerr.GetComponent<Shooting>();
    }
    // Update is called once per frame
    void Update()
    {
        TextoAmmo.SetText("Ammo: " + balas.bulletsLeft + " / " + balas.magazineSize);
        TextoPizzas.SetText("Pizzas: " + pizzas.currentPizzas + " / " + pizzas.totalPizzas);
        TextoRounds.SetText("Round: " + pizzas.rounds + " / 4");

      
    }
}
