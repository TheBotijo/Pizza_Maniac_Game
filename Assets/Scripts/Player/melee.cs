using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class melee : MonoBehaviour
{
    private Shooting shootScr;
    [SerializeField]
    private ParticlesAgua aguaScr;
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

            Invoke(nameof(ReseteShot), shootScr.timeBetweenShots);
        }
        else if (other.CompareTag("Farola"))
        {
            other.GetComponent<ParticlesMetal>().metal.Play();
            other.GetComponent<ParticlesMetal>().chispaSound.Play();
            other.GetComponent<ParticlesMetal>().Chispas.Play();
            Debug.Log("chispas");
        }
        else if (other.CompareTag("BocaIncendio"))
        {
            aguaScr = other.GetComponent<ParticlesAgua>();
            aguaScr.metal.Play();
            aguaScr.Agua.Play();
            aguaScr.aguaSound.Play();
            Debug.Log("agua");
            Invoke(nameof(Stop), 5f);
        }
    }
    void Stop()
        {
            Debug.Log("noagua");
            aguaScr.Agua.Stop();
            aguaScr.aguaSound.Stop();
        }

        void ReseteShot()
        {
            shootScr.ResetShot();
        }
    
}
