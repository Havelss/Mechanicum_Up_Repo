using UnityEngine;

public class SO_Terminal : MonoBehaviour, IInteractable
{
    [Header("Terminal Setup")]
    [SerializeField] private string prompt = "Usar terminal";
    [SerializeField] private GameObject terminalCanvas;            // Canvas de esta terminal
    [SerializeField] private SymbolTerminalController terminalController; // Controller de esta terminal
    [SerializeField] private MonoBehaviour controlledObject;      // Objeto que controla la terminal (Ej: Elevator)

    private bool isActive = false;
    private PlayerController playerController;

    public string InteractionPrompt => prompt;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("No se encontró PlayerController en la escena.");

        if (terminalCanvas != null)
            terminalCanvas.SetActive(false); // ocultar al inicio
    }

    public void Interact(Interactor interactor)
    {
        OpenTerminal();
    }

    public void SetControlledObject(MonoBehaviour obj)
    {
        controlledObject = obj;
    }

    public MonoBehaviour GetControlledObject()
    {
        return controlledObject;
    }

    public void OpenTerminal()
    {
        if (terminalCanvas == null || terminalController == null)
        {
            Debug.LogError("Asigna terminalCanvas y terminalController en SO_Terminal");
            return;
        }

        // Limpiar la secuencia anterior
        terminalController.ClearSequence();

        isActive = true;
        terminalCanvas.SetActive(true);

        // Pausar el tiempo
        Time.timeScale = 0f;
        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Configurar botones de la terminal actual
        if (SymbolManager.Instance != null)
            SymbolManager.Instance.SetupTerminalButtons(terminalController);
    }

    public void CloseTerminal()
    {
        if (!isActive) return;

        isActive = false;
        terminalCanvas.SetActive(false);

        // Reanudar tiempo
        Time.timeScale = 1f;
        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
