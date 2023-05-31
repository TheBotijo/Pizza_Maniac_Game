using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();            
    }

    public void ChangeMaxHealth(float MaxHealth)
    {
        slider.maxValue = MaxHealth;
    }

    public void ChangeActualHealth(float Health)
    {
        slider.value = Health;
    }

    public void InitiateHealthBar(float Health)
    {
        ChangeMaxHealth(Health);
        ChangeActualHealth(Health);
    }
}

