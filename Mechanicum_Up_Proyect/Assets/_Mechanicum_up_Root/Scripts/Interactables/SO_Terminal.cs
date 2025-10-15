using UnityEngine;

public class SO_Terminal : MonoBehaviour, IInteractable

/*
{
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Abre Terminal");
        return true;
    }
}

*/

{
    [SerializeField] private string prompt = "Usar terminal";
    private GameObject terminalCanvas;
    private bool isActive = false;

    public string InteractionPrompt => prompt;

    private void Awake()
    {
        // Busca automáticamente el canvas con el tag "PantallaTerminal"
        terminalCanvas = GameObject.FindGameObjectWithTag("PantallaTerminal");

        if (terminalCanvas == null)
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag 'PantallaTerminal'");
        }
        else
        {
            // Asegurarse de que arranque desactivado
            terminalCanvas.SetActive(false);
        }
    }

    public bool Interact(Interactor interactor)
    {
        if (terminalCanvas == null) return false;

        // Alternar visibilidad
        isActive = !isActive;
        terminalCanvas.SetActive(isActive);

        // (Opcional) Control del cursor si es una UI interactiva
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;

        Debug.Log(isActive ? "Terminal abierta" : "Terminal cerrada");

        return true;
    }

    // Llamado desde Interactor cuando el jugador se aleja
    public void CloseTerminal()
    {
        if (terminalCanvas != null && isActive)
        {
            terminalCanvas.SetActive(false);
            isActive = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("Terminal cerrada al salir");
        }
    }
}
