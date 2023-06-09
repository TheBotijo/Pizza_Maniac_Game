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
    public Button ResumeBtn;
    public Button OptionsBtn;
    public Button MenuBtn;
    public Button CrossBtn;
    //new input system
    public PlayerInputMap _playerInput;
    private CameraController camScript;
    public AudioSource click;

    private void Start()
    {
        camScript = Object.FindObjectOfType<CameraController>();
        _playerInput = new PlayerInputMap();
        _playerInput.Juego.Enable();

        ResumeBtn.onClick.AddListener(Resume);
        OptionsBtn.onClick.AddListener(Opciones);
        MenuBtn.onClick.AddListener(BackToMenu);
        CrossBtn.onClick.AddListener(Cross);
    }

    void Update()
    {

        if (_playerInput.Juego.Pause.WasPressedThisFrame())
        {
            click.Play();
            _playerInput.UI.Enable();
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
        click.Play();
        int num = 1;
        camScript.cameraSwitch(num);
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
        click.Play();
        int num = 0;
        camScript.cameraSwitch(num);
        pauseMenuUI.SetActive(true);
        UIGeneral.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Opciones()
    {
        click.Play();
        OptionsMenuOpen = true;
        pauseBotones.SetActive(false);
        pauseOpciones.SetActive(true);

    }
    public void Cross()
    {
        click.Play();
        OptionsMenuOpen = false;
        pauseBotones.SetActive(true);
        pauseOpciones.SetActive(false);

    }
    public void BackToMenu()
    {
        click.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }
}