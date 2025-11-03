using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject pauseMenuCanvas;   // Canvas del menú de pausa
    [SerializeField] private GameObject optionsMenuCanvas; // Canvas del submenú de opciones

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (optionsMenuCanvas != null) optionsMenuCanvas.SetActive(false);
    }

    void Update()
    {
        // Evitar abrir el menú si una terminal está activa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsAnyTerminalOpen()) return;

            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    private bool IsAnyTerminalOpen()
    {
        // Usa el nuevo método recomendado por Unity 2023+
        SO_Terminal[] terminals = Object.FindObjectsByType<SO_Terminal>(FindObjectsSortMode.None);
        foreach (SO_Terminal terminal in terminals)
        {
            if (terminal != null && terminal.IsOpen())
                return true;
        }
        return false;
    }


    public void Pause()
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(true);

        if (optionsMenuCanvas != null)
            optionsMenuCanvas.SetActive(false);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void Resume()
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);

        if (optionsMenuCanvas != null)
            optionsMenuCanvas.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void OpenOptions()
    {
        if (optionsMenuCanvas != null)
            optionsMenuCanvas.SetActive(true);

        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);
    }

    public void CloseOptions()
    {
        if (optionsMenuCanvas != null)
            optionsMenuCanvas.SetActive(false);

        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(true);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    
}
