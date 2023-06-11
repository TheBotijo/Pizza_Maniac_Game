using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class melee : MonoBehaviour
{
    private Shooting shootScr;
    private void OnTriggerEnter(Collider other)
    {
        shootScr = gameObject.GetComponentInParent<Shooting>();
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("ATACANDO CON EL RODILLO");
            if (other.gameObject.name == "BichoQueso(Clone)")
            {
                Debug.Log("ATACANDO CON EL RODILLO " + other.gameObject.name);
                shootScr.enemyDamage1 = other.gameObject.GetComponent<AIEnemy1>();
                shootScr.enemyDamage1.TakeDamage();
            }
            else if (other.gameObject.name == "BichoSeta(Clone)")
            {
                Debug.Log("ATACANDO CON EL RODILLO " + other.gameObject.name);
                shootScr.enemyDamage2 = other.gameObject.GetComponent<AIEnemy2>();
                shootScr.enemyDamage2.TakeDamage();
            }
            else if (other.gameObject.name == "BichoTomate(Clone)")
            {
                Debug.Log("ATACANDO CON EL RODILLO " + other.gameObject.name);
                shootScr.enemyDamage3 = other.gameObject.GetComponent<AIEnemy3>();
                shootScr.enemyDamage3.TakeDamage();
            }

            Invoke(nameof(ReseteShot), shootScr.timeBetweenShooting);
        }

        void ReseteShot()
        {
            shootScr.ResetShot();
        }
    }
}
