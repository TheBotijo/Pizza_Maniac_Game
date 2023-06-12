using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SoundClickMenu : MonoBehaviour
{
    public PlayerInputMap _playerInputmap;
    public AudioSource click;

    void Start()
    {
        _playerInputmap = new PlayerInputMap();
        _playerInputmap.UI.Enable();
    }

    void Update()
    {
        if (_playerInputmap.UI.Click.WasPressedThisFrame())
            click.Play();
    }
}
