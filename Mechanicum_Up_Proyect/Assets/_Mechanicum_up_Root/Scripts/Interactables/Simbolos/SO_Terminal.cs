using UnityEngine;



/*
public class SO_Terminal : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Usar terminal";
    [SerializeField] private GameObject terminalCanvas;
    [SerializeField] private SymbolTerminalController terminalController;
    [SerializeField] private MonoBehaviour controlledObject;

    private bool isActive = false;
    private PlayerController playerController;

    public string InteractionPrompt => prompt;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("No se encontró Player.");
    }

    #region Abrir y cerrar

    public bool Interact(Interactor interactor)
    {
        OpenTerminal();
        return true;
    }



    public void OpenTerminal()
    {
        if (terminalCanvas == null || terminalController == null)
        {
            Debug.LogError("Asigna terminalCanvas y terminalController en SO_Terminal");
            return;
        }

        isActive = true;
        terminalCanvas.SetActive(true);

        // Pausar el tiempo del juego
        Time.timeScale = 0f;

        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Configura los botones de la terminal
        SymbolManager.Instance.SetupTerminalButtons(terminalController);

        Debug.Log("Terminal abierta (tiempo pausado)");
    }

    public void CloseTerminal()
    {
        if (!isActive) return;

        isActive = false;
        terminalCanvas.SetActive(false);

        // Reanudar el tiempo del juego
        Time.timeScale = 1f;

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Terminal cerrada (tiempo reanudado)");
    }

    #endregion

}

*/



public class SO_Terminal : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Usar terminal";
    [SerializeField] private GameObject terminalCanvas; // Canvas de esta terminal
    [SerializeField] private SymbolTerminalController terminalController; // Referencia a su controlador
    [SerializeField] private MonoBehaviour controlledObject;
    public MonoBehaviour ControlledObject => controlledObject; // Ascensor u otro objeto que controla

    private bool isActive = false;
    private PlayerController playerController;

    public string InteractionPrompt => prompt;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("No se encontró PlayerController en la escena.");

        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }

    public bool Interact(Interactor interactor)
    {
        OpenTerminal();
        return true;
    }

    public void OpenTerminal()
    {
        if (terminalCanvas == null || terminalController == null)
        {
            Debug.LogError("Asigna terminalCanvas y terminalController en SO_Terminal");
            return;
        }

        isActive = true;
        terminalCanvas.SetActive(true);

        // Pausar el tiempo del juego
        Time.timeScale = 0f;

        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Configura los botones de esta terminal usando el SymbolManager
        SymbolManager.Instance.SetupTerminalButtons(terminalController);
    }

    public void CloseTerminal()
    {
        if (!isActive) return;

        isActive = false;
        terminalCanvas.SetActive(false);

        Time.timeScale = 1f;

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}


