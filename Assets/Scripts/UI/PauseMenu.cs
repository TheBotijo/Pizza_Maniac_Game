using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    //new input system
    bool GameIsPaused = false;
    public bool OptionsMenuOpen = false;
    public GameObject pauseMenuUI;
    public GameObject UIGeneral;
    public GameObject pauseBotones;
    public GameObject pauseOpciones;
    //new input system
    public PlayerInputMap _playerInput;

    private void Start()
    {
        _playerInput = new PlayerInputMap();
        _playerInput.Juego.Enable();
    }

    void Update()
    {

        if (_playerInput.Juego.Pause.WasPressedThisFrame())
        {
            _playerInput.UI.Enable();
            Debug.Log("Hola");
            if (OptionsMenuOpen)
            {
                Cross();
            }
            else if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        UIGeneral.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerInput.Juego.Enable();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        UIGeneral.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Opciones()
    {
        OptionsMenuOpen = true;
        pauseBotones.SetActive(false);
        pauseOpciones.SetActive(true);

    }
    public void Cross()
    {
        OptionsMenuOpen = false;
        pauseBotones.SetActive(true);
        pauseOpciones.SetActive(false);

    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }
}