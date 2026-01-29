//using UnityEngine;

//public class SO_Terminal : MonoBehaviour
//{
//    [Header("Referencias")]
//    public GameObject terminalCanvas;
//    public SymbolTerminalController terminalController;

//    [Header("Respawn")]
//    [SerializeField] private Transform respawnPoint;

//    [Header("Electricidad")]
//    [Tooltip("Si está marcada, la terminal empieza con electricidad")]
//    [SerializeField] private bool startsElectrified = false;

//    private bool isOpen = false;
//    private bool isElectrified;

//    private void Awake()
//    {
//        // 🔌 Estado inicial configurable desde el Inspector
//        isElectrified = startsElectrified;
//    }

//    private void Start()
//    {
//        if (terminalCanvas != null)
//            terminalCanvas.SetActive(false);
//    }

//    // =========================
//    // ELECTRICIDAD
//    // =========================
//    public void SetElectrified(bool state)
//    {
//        isElectrified = state;
//        Debug.Log($"{name} electricidad: {(isElectrified ? "encendida" : "apagada")}");
//    }

//    public bool IsElectrified()
//    {
//        return isElectrified;
//    }

//    // =========================
//    // INTERACCIÓN
//    // =========================
//    public void OpenTerminal(MonoBehaviour controlledObject)
//    {
//        if (isOpen) return;

//        // 🔒 BLOQUEO SI NO TIENE ENERGÍA
//        if (!isElectrified)
//        {
//            Debug.Log($"{name} está apagada. No se puede acceder.");
//            return;
//        }

//        isOpen = true;
//        Time.timeScale = 0f;

//        if (terminalCanvas != null)
//            terminalCanvas.SetActive(true);

//        if (UIManager.Instance != null)
//            UIManager.Instance.SetMenuState(true);

//        if (SymbolManager.Instance != null && terminalController != null)
//            SymbolManager.Instance.SetupTerminalButtons(terminalController);

//        if (terminalController != null)
//        {
//            terminalController.ControlledObject = controlledObject;
//            terminalController.ClearSequence();
//        }

//        var player = FindFirstObjectByType<PlayerRespawn>();
//        if (player != null && respawnPoint != null)
//            player.UpdateRespawn(respawnPoint);
//    }

//    public bool IsOpen()
//    {
//        return isOpen;
//    }

//    public void CloseTerminal()
//    {
//        if (!isOpen) return;

//        isOpen = false;
//        Time.timeScale = 1f;

//        if (terminalCanvas != null)
//            terminalCanvas.SetActive(false);

//        if (UIManager.Instance != null)
//            UIManager.Instance.SetMenuState(false);
//    }


//}

using UnityEngine;

public class SO_Terminal : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject terminalCanvas;
    public SymbolTerminalController terminalController;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;

    [Header("Electricidad")]
    [Tooltip("Si está marcada, la terminal empieza con electricidad")]
    [SerializeField] private bool startsElectrified = false;

    [Header("Configuración del Cursor")]
    [Tooltip("Sprite del cursor en estado normal")]
    public Texture2D cursorNormal;

    [Tooltip("Sprite del cursor cuando mantienes presionado el clic")]
    public Texture2D cursorClick;

    [Tooltip("Punto de activación del clic (0,0 es arriba-izquierda)")]
    public Vector2 hotspot = Vector2.zero;

    private bool isOpen = false;
    private bool isElectrified;

    private void Awake()
    {
        // Estado inicial de energía
        isElectrified = startsElectrified;
    }

    private void Start()
    {
        // Aseguramos que el canvas esté apagado al inicio
        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }

    private void Update()
    {
        // Solo detectamos clics para cambiar el sprite si la terminal está abierta
        if (isOpen)
        {
            HandleCursorVisuals();
        }
    }

    private void HandleCursorVisuals()
    {
        // Cambia el sprite mientras mantienes el botón izquierdo presionado
        if (Input.GetMouseButtonDown(0))
        {
            SetCursorTexture(cursorClick);
        }

        // Vuelve al sprite normal al soltar el botón
        if (Input.GetMouseButtonUp(0))
        {
            SetCursorTexture(cursorNormal);
        }
    }

    private void SetCursorTexture(Texture2D tex)
    {
        if (tex != null)
        {
            Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
        }
    }

    // =========================
    // INTERACCIÓN PRINCIPAL
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
        Time.timeScale = 0f; // Pausar el juego

        // --- Configuración del Ratón ---
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetCursorTexture(cursorNormal);

        // --- Lógica de UI ---
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

        // --- Actualizar Respawn ---
        var player = FindFirstObjectByType<PlayerRespawn>();
        if (player != null && respawnPoint != null)
            player.UpdateRespawn(respawnPoint);
    }

    public void CloseTerminal()
    {
        if (!isOpen) return;

        isOpen = false;
        Time.timeScale = 1f; // Reanudar el juego

        // --- Resetear el Ratón al de Windows/Sistema ---
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        /* Si tu juego es en primera persona, descomenta estas líneas 
           para que el ratón vuelva a desaparecer al cerrar la terminal:
           
           Cursor.visible = false;
           Cursor.lockState = CursorLockMode.Locked;
        */

        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);

        if (UIManager.Instance != null)
            UIManager.Instance.SetMenuState(false);
    }

    // =========================
    // MÉTODOS PÚBLICOS (GETTERS/SETTERS)
    // =========================
    public void SetElectrified(bool state)
    {
        isElectrified = state;
        Debug.Log($"{name} electricidad: {(isElectrified ? "encendida" : "apagada")}");
    }

    public bool IsElectrified() => isElectrified;
    public bool IsOpen() => isOpen;
}