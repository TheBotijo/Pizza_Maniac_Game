using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melee : MonoBehaviour
{
    private Shooting shootScr;
    private void OnTriggerEnter(Collider other)
    {
        shootScr = gameObject.GetComponentInParent<Shooting>();
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.name == "BichoQueso(Clone)")
            {
                shootScr.enemyDamage1 = other.gameObject.GetComponent<AIEnemy1>();
                shootScr.enemyDamage1.TakeDamage();
            }
            else if (other.gameObject.name == "BichoSeta(Clone)")
            {
                shootScr.enemyDamage2 = other.gameObject.GetComponent<AIEnemy2>();
                shootScr.enemyDamage2.TakeDamage();
            }
            else if (other.gameObject.name == "BichoTomate(Clone)")
            {
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
