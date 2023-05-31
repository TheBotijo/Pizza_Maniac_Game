using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class QuestPoint : MonoBehaviour
{
    public Image deliverImage;
    public Image pizzaImage;
    public TextMeshProUGUI meter;
    public TextMeshProUGUI meter2;
    public GameObject deliverHere;
    public Vector3 offset;
    public PizzaDeliver pizzasCount;

    // Update is called once per frame
    public void Update()
    {
        float minX = deliverImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = deliverImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.width - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(deliverHere.transform.position + offset);

        if (Vector3.Dot((deliverHere.transform.position - transform.position), transform.forward) < 0)
        {
            //Target is behind the player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxX);

        if (pizzasCount.currentPizzas == 0)
        {
            meter.enabled = false;
            meter2.enabled = true;
            deliverImage.enabled = false;
            pizzaImage.enabled = true;
            pizzaImage.transform.position = pos;
            meter2.text = (((int)Vector3.Distance(deliverHere.transform.position, transform.position)) - 6).ToString() + "m";
        }
        else
        {
            meter2.enabled = false;
            meter.enabled = true;
            deliverImage.enabled = true;
            pizzaImage.enabled = false;
            deliverImage.transform.position = pos;
            meter.text = (((int)Vector3.Distance(deliverHere.transform.position, transform.position)) - 6).ToString() + "m";

        }

        
    }
}
