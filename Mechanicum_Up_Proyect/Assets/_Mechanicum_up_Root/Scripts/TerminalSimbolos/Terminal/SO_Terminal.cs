////using UnityEngine;

////public class SO_Terminal : MonoBehaviour
////{
////    [Header("Referencias")]
////    public GameObject terminalCanvas;
////    public SymbolTerminalController terminalController;

////    [Header("Respawn")]
////    [SerializeField] private Transform respawnPoint;

////    [Header("Electricidad")]
////    [Tooltip("Si está marcada, la terminal empieza con electricidad")]
////    [SerializeField] private bool startsElectrified = false;

////    private bool isOpen = false;
////    private bool isElectrified;

////    private void Awake()
////    {
////        // 🔌 Estado inicial configurable desde el Inspector
////        isElectrified = startsElectrified;
////    }

////    private void Start()
////    {
////        if (terminalCanvas != null)
////            terminalCanvas.SetActive(false);
////    }

////    // =========================
////    // ELECTRICIDAD
////    // =========================
////    public void SetElectrified(bool state)
////    {
////        isElectrified = state;
////        Debug.Log($"{name} electricidad: {(isElectrified ? "encendida" : "apagada")}");
////    }

////    public bool IsElectrified()
////    {
////        return isElectrified;
////    }

////    // =========================
////    // INTERACCIÓN
////    // =========================
////    public void OpenTerminal(MonoBehaviour controlledObject)
////    {
////        if (isOpen) return;

////        // 🔒 BLOQUEO SI NO TIENE ENERGÍA
////        if (!isElectrified)
////        {
////            Debug.Log($"{name} está apagada. No se puede acceder.");
////            return;
////        }

////        isOpen = true;
////        Time.timeScale = 0f;

////        if (terminalCanvas != null)
////            terminalCanvas.SetActive(true);

////        if (UIManager.Instance != null)
////            UIManager.Instance.SetMenuState(true);

////        if (SymbolManager.Instance != null && terminalController != null)
////            SymbolManager.Instance.SetupTerminalButtons(terminalController);

////        if (terminalController != null)
////        {
////            terminalController.ControlledObject = controlledObject;
////            terminalController.ClearSequence();
////        }

////        var player = FindFirstObjectByType<PlayerRespawn>();
////        if (player != null && respawnPoint != null)
////            player.UpdateRespawn(respawnPoint);
////    }

////    public bool IsOpen()
////    {
////        return isOpen;
////    }

////    public void CloseTerminal()
////    {
////        if (!isOpen) return;

////        isOpen = false;
////        Time.timeScale = 1f;

////        if (terminalCanvas != null)
////            terminalCanvas.SetActive(false);

////        if (UIManager.Instance != null)
////            UIManager.Instance.SetMenuState(false);
////    }
////}

using UnityEngine;

public class SO_Terminal : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject terminalCanvas;
    public SymbolTerminalController terminalController;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;

    [Header("Electricidad")]
    [Tooltip("Si está marcada, la terminal empieza con electricidad")]
    [SerializeField] private bool startsElectrified = false;

    private bool isOpen = false;
    private bool isElectrified;

    private void Awake()
    {
        // 🔌 Estado inicial configurable desde el Inspector
        isElectrified = startsElectrified;
    }

    private void Start()
    {
        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }

    // =========================
    // ELECTRICIDAD
    // =========================
    public void SetElectrified(bool state)
    {
        isElectrified = state;
        Debug.Log($"{name} electricidad: {(isElectrified ? "encendida" : "apagada")}");
    }

    public bool IsElectrified()
    {
        return isElectrified;
    }

    // =========================
    // INTERACCIÓN
    // =========================
    public void OpenTerminal(MonoBehaviour controlledObject)
    {
        if (isOpen) return;

        // 🔒 BLOQUEO SI NO TIENE ENERGÍA
        if (!isElectrified)
        {
            Debug.Log($"{name} está apagada. No se puede acceder.");
            return;
        }

        isOpen = true;
        Time.timeScale = 0f;

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

        var player = FindFirstObjectByType<PlayerRespawn>();
        if (player != null && respawnPoint != null)
            player.UpdateRespawn(respawnPoint);
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public void CloseTerminal()
    {
        if (!isOpen) return;

        isOpen = false;
        Time.timeScale = 1f;

        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);

        if (UIManager.Instance != null)
            UIManager.Instance.SetMenuState(false);
    }
}
