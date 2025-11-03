using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject panelMain;     // Panel principal del menú de pausa
    [SerializeField] private GameObject panelOptions;  // Panel de opciones

    private bool isPaused = false;

    private void Start()
    {
        if (panelMain != null)
            panelMain.SetActive(false);

        if (panelOptions != null)
            panelOptions.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (panelMain != null)
        {
            panelMain.SetActive(true);
            panelOptions.SetActive(false);
        }

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (panelMain != null)
            panelMain.SetActive(false);

        if (panelOptions != null)
            panelOptions.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 🔹 Abre el submenú de opciones
    public void OpenOptions()
    {
        if (panelMain != null)
            panelMain.SetActive(false);

        if (panelOptions != null)
            panelOptions.SetActive(true);
    }

    // 🔹 Vuelve del submenú al menú principal
    public void BackFromOptions()
    {
        if (panelOptions != null)
            panelOptions.SetActive(false);

        if (panelMain != null)
            panelMain.SetActive(true);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("MainMenu");
    }

}
