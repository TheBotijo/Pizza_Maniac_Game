using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTimeOffset : MonoBehaviour
{

    public Button PlayBtn;
    public GameObject nivelesatras;
    public Button AtrasBtn;
    public GameObject jugar;

    void Start()
    {
        PlayBtn.onClick.AddListener(play);
        AtrasBtn.onClick.AddListener(atras);
    }

    void play()
    {
        nivelesatras.SetActive(true);
    }

    void atras()
    {
        jugar.SetActive(true);
    }

    public void offsetniveles()
    {
        Invoke(nameof(play), 0.7f);
    }

    public void offsetjugar()
    {
        Invoke(nameof(atras), 0.7f);
    }

}
