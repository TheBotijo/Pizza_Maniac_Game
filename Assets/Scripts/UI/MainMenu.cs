using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    //new input system
    public PlayerInputMap _playerInputmap;
    //public AudioSource click;

    void Start()
    {
        _playerInputmap = new PlayerInputMap();
        _playerInputmap.UI.Enable();
    }

    void Update()
    {
        //if (_playerInputmap.UI.Click.WasPressedThisFrame())
            //click.Play();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void PlayGame3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
