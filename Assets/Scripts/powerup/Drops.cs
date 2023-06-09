using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    [SerializeField] [Range(0,100)] private int ProbabilidadDrop, XcentGuind, XcentHuevo, XcentMuni, XcentCora;
    [SerializeField] private int DropsTotalNum;
    int whichdrop;
    int dropxcent;
    public int[] xCents;
    public int[] xDrops;

    public void DropSystem(Vector3 pos)
    {
        Vector3 pos2 = pos;
        int total = XcentCora + XcentMuni + XcentGuind + XcentHuevo;

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
                }
                Reshuffle();

                whichdrop = Random.Range(0, 99);

                if (xDrops[whichdrop] == 0) Instantiate(Resources.Load("guindilla"), pos2 + new Vector3(0, 2, 0), Quaternion.identity);
                else if (xDrops[whichdrop] == 1) Instantiate(Resources.Load("Municion"), pos2 + new Vector3(0, 2, 0), Quaternion.identity);
                else if (xDrops[whichdrop] == 2) Debug.Log("DROP CORAZON");
                else if (xDrops[whichdrop] == 3) Instantiate(Resources.Load("huevotimer"), pos2 + new Vector3(0, 2, 0), Quaternion.identity);

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
