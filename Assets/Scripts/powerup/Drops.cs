using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Drops : MonoBehaviour
{
    [SerializeField] [Range(0,100)] private int ProbabilidadDrop, XcentGuind, XcentHuevo, XcentMuni, XcentCora, Xlevadura;
    private int DropsTotalNum = 5;
    int whichdrop;
    int dropxcent;
    public int[] xCents;
    public int[] xDrops;

    Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    public void DropSystem(Vector3 pos)
    {
        Vector3 pos2 = new Vector3(pos.x, 1f, pos.z);
        int total = XcentCora + XcentMuni + XcentGuind + XcentHuevo + Xlevadura;

        if (total != 100)
        {
            Debug.LogWarning("Los porcentajes de los Drops no suman 100");
        }
        else
        {
            xDrops = new int[100];
            xCents = new int[DropsTotalNum];
            Debug.LogWarning("DROP TIME");

            dropxcent = Random.Range(0, 100);



            if (dropxcent < ProbabilidadDrop)
            {
                int i, a, x=0;

                // Creem una estructura per omplir una array amb numeros del 0 al total de 
                for (i = 0; i < DropsTotalNum - 1; i++)
                {
                    if (i == 0)
                    {
                        for (a = 0; a < XcentGuind; a++)
                        {
                            xDrops[a] = i;
                        }
                    }
                    else if (i == 1)
                    {
                        for (a = 0; a < XcentMuni; a++)
                        {
                            x = XcentGuind - 1 + a;
                            xDrops[x] = i;
                        }
                    }
                    else if (i == 2)
                    {
                        for (a = 0; a < XcentCora; a++)
                        {
                            x = XcentGuind - 1 + XcentMuni + a;
                            xDrops[x] = i;
                        }
                    }
                    else if (i == 3)
                    {
                        for (a = 0; a < XcentHuevo; a++)
                        {
                            x = XcentGuind - 1 + XcentMuni + XcentCora + a;
                            xDrops[x] = i;
                        }
                    }
                    else if (i == 4)
                    {
                        for (a = 0; a < Xlevadura; a++)
                        {
                            x = XcentGuind - 1 + XcentMuni + XcentCora + XcentHuevo + a;
                            xDrops[x] = i;
                        }
                    }
                }
                Reshuffle();

                whichdrop = Random.Range(0, 99);
                Debug.Log(whichdrop);
                Debug.Log(dropxcent);
                if (xDrops[whichdrop] == 0) Instantiate(Resources.Load("guindilla"), pos2, Quaternion.identity);
                else if (xDrops[whichdrop] == 1) Instantiate(Resources.Load("Municion"), pos2, Quaternion.identity);
                else if (xDrops[whichdrop] == 2) Instantiate(Resources.Load("pizzacor"), pos2, Quaternion.identity);
                else if (xDrops[whichdrop] == 3) Instantiate(Resources.Load("huevotimer"), pos2, Quaternion.identity);
                else if (xDrops[whichdrop] == 4) Instantiate(Resources.Load("levadura"), pos2, Quaternion.identity);
            }
            else
            {
                Debug.Log("NO DROP");
            }
        }
    }
    void Reshuffle()
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < xCents.Length; t++)
        {
            int tmp = xCents[t];
            int r = Random.Range(t, xCents.Length);
            xCents[t] = xCents[r];
            xCents[r] = tmp;
        }
    }
    
}
