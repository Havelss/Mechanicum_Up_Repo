using UnityEngine;

public class SO_Terminal : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject terminalCanvas;
    public SymbolTerminalController terminalController;

    private bool isOpen = false;

    private void Start()
    {
        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }

    // 🔹 Ahora recibe el objeto controlado dinámicamente
    public void OpenTerminal(MonoBehaviour controlledObject)
    {
        if (isOpen) return;
        isOpen = true;

        if (terminalCanvas != null)
            terminalCanvas.SetActive(true);

        if (SymbolManager.Instance != null && terminalController != null)
            SymbolManager.Instance.SetupTerminalButtons(terminalController);

        if (terminalController != null)
        {
            terminalController.ControlledObject = controlledObject; // Usar la propiedad pública
            terminalController.ClearSequence();
        }
        else
        {
            Debug.LogWarning($"La terminal {name} no tiene asignado un SymbolTerminalController.");
        }
    }

    public void CloseTerminal()
    {
        if (!isOpen) return;
        isOpen = false;

        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }
}


