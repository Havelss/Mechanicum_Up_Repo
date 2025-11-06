using UnityEngine;

public class SO_Terminal : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject terminalCanvas;
    public SymbolTerminalController terminalController;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint; // Empty hijo de la terminal

    private bool isOpen = false;
    private bool isElectrified = true; // por defecto encendida

    private void Start()
    {
        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }

    // 🔹 Encender / apagar la terminal
    public void SetElectrified(bool state)
    {
        isElectrified = state;
        Debug.Log($"{name} electricidad: {(isElectrified ? "encendida" : "apagada")}");
    }

    public bool IsElectrified()
    {
        return isElectrified;
    }

    // 🔹 Abre la terminal y actualiza el respawn
    public void OpenTerminal(MonoBehaviour controlledObject)
    {
        if (isOpen) return;
        isOpen = true;

        if (terminalCanvas != null)
            terminalCanvas.SetActive(true);

        if (UIManager.Instance != null)
            UIManager.Instance.SetMenuState(true);

        if (SymbolManager.Instance != null && terminalController != null)
            SymbolManager.Instance.SetupTerminalButtons(terminalController);

        if (terminalController != null)
        {
            terminalController.ControlledObject = controlledObject;
            terminalController.ClearSequence();
        }

        // 🧠 Actualizar respawn del jugador si existe un punto válido
        var player = FindFirstObjectByType<PlayerRespawn>();
        if (player != null && respawnPoint != null)
        {
            player.UpdateRespawn(respawnPoint);
            Debug.Log($"Nuevo punto de respawn establecido en {respawnPoint.name}");
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
            UIManager.Instance.SetMenuState(false);
    }
}


