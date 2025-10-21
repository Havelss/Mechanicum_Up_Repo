using UnityEngine;

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

        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Configura los botones de la terminal
        SymbolManager.Instance.SetupTerminalButtons(terminalController);
    }

    public void CloseTerminal()
    {
        if (!isActive) return;

        isActive = false;
        terminalCanvas.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion

}
