using UnityEngine;

/*
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

*/


public class SO_Terminal : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject terminalCanvas;
    public SymbolTerminalController terminalController;

    private bool isOpen = false;

    // ⚡ Estado de electricidad
    private bool isElectrified = true; // por defecto encendida (puedes cambiar a false si lo deseas)

    private void Start()
    {
        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }

    // 🔹 Llamado por la electricidad para encender la terminal
    public void SetElectrified(bool state)
    {
        isElectrified = state;
        Debug.Log($"{name} electricidad: {(isElectrified ? "encendida" : "apagada")}");
    }

    // 🔹 Consultado por la TerminalBox antes de abrirla
    public bool IsElectrified()
    {
        return isElectrified;
    }

    // 🔹 Ahora recibe el objeto controlado dinámicamente
    public void OpenTerminal(MonoBehaviour controlledObject)
    {
        if (isOpen) return;
        isOpen = true;

        if (terminalCanvas != null)
            terminalCanvas.SetActive(true);

        if (UIManager.Instance != null)
            UIManager.Instance.SetMenuState(true); // 👈 activa cursor y bloquea el resto

        if (SymbolManager.Instance != null && terminalController != null)
            SymbolManager.Instance.SetupTerminalButtons(terminalController);

        if (terminalController != null)
        {
            terminalController.ControlledObject = controlledObject;
            terminalController.ClearSequence();
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }


    public void CloseTerminal()
    {
        if (!isOpen) return;
        isOpen = false;

        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);

        if (UIManager.Instance != null)
            UIManager.Instance.SetMenuState(false); // 👈 desactiva cursor si no hay más menús
    }
}
