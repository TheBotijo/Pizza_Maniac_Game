using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    //Texts
    [SerializeField]
    private TMP_Text TextoPizzas;
    [SerializeField]
    private TMP_Text TextoAmmo;
    [SerializeField]
    private TMP_Text TextoRounds;

    //Scripts
    public PizzaDeliver pizzas;
    public Shooting balas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextoAmmo.SetText("Ammo: " + balas.bulletsLeft + " / " + balas.magazineSize);
        TextoPizzas.SetText("Pizzas: " + pizzas.currentPizzas + " / " + pizzas.totalPizzas);
        TextoRounds.SetText("Round: " + pizzas.rounds);

      
    }
}
