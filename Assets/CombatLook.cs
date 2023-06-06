using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class CombatLook : MonoBehaviour
{
    public Transform normalLook;
    public float damping = 5;

    // Update is called once per frame
    void Update()
    {
        var lookDir = normalLook.position - transform.position;
        lookDir.y = 0;
        var rotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        //transform.LookAt(combatLook);
    }
}
