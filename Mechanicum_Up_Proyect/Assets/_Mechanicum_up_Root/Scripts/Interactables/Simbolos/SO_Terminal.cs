using UnityEngine;

public class SO_Terminal : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject terminalCanvas;              // Canvas de la terminal
    [SerializeField] private SymbolTerminalController terminalController; // Controlador de símbolos
    [SerializeField] private MonoBehaviour controlledObject;          // Objeto que controla (ej: ascensor)

    private bool isOpen = false;

    private void Start()
    {
        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);
    }

    public void OpenTerminal()
    {
        if (isOpen) return;
        isOpen = true;

        if (terminalCanvas == null || terminalController == null)
        {
            Debug.LogError($"Faltan referencias en {name}: asigna Canvas y Controller.");
            return;
        }

        // Mostrar el canvas
        terminalCanvas.SetActive(true);

        // Configurar botones y limpiar secuencia
        if (SymbolManager.Instance != null)
            SymbolManager.Instance.SetupTerminalButtons(terminalController);

        terminalController.controlledObject = controlledObject;
        terminalController.ClearSequence();

        // Pausar el juego y desbloquear cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseTerminal()
    {
        if (!isOpen) return;
        isOpen = false;

        if (terminalCanvas != null)
            terminalCanvas.SetActive(false);

        // Reanudar juego y bloquear cursor
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
