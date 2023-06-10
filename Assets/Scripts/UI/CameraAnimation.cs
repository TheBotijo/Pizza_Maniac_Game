using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{

    public Animator animator;

    public void jugar()
    {
        animator.SetBool("play", true);
    }

    public void atras()
    {
        animator.SetBool("play", false);
    }

}
