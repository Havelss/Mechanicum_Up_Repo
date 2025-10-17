using UnityEngine;

public class SO_Terminal : MonoBehaviour, IInteractable

{
    [SerializeField] private string prompt = "Usar terminal";
    private GameObject terminalCanvas;
    private bool isActive = false;

    private PlayerController playerController;

    public string InteractionPrompt => prompt;

    private void Awake()
    {
        
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()) // Buscar el Canvas incluso si está inactivo
        {
            if (obj.CompareTag("PantallaTerminal"))
            {
                terminalCanvas = obj;
                break;
            }
        }

        if (terminalCanvas == null)
            Debug.LogError("No se encontró Canvas con el tag 'PantallaTerminal'");
        else
            terminalCanvas.SetActive(false);

        
        playerController = FindFirstObjectByType<PlayerController>();  //Buscar automáticamente al jugador 
        if (playerController == null)
            Debug.LogWarning("No se encontró Player.");
    }

    #region Abrir y cerrar

    public bool Interact(Interactor interactor)
    {
        if (terminalCanvas == null) return false;

        // Solo abrir (no alternar)
        OpenTerminal();
        return true;
    }

    public void OpenTerminal()
    {
        isActive = true;
        terminalCanvas.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        
        Cursor.lockState = CursorLockMode.None; // Con este comando muestro y desbloquear el cursor
        Cursor.visible = true;

        Debug.Log("Terminal abierta");
    }

    public void CloseTerminal()
    {
        if (terminalCanvas == null || !isActive) return;

        isActive = false;
        terminalCanvas.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        
        Cursor.lockState = CursorLockMode.Locked;   // Con este comando dejo de mostrar y bloqueo el cursor
        Cursor.visible = false;

        Debug.Log("Terminal cerrada");
    }

    #endregion
}



//Mantener por si acaso

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

*/ //1º codigo

/*

{
[SerializeField] private string prompt = "Usar terminal";
private GameObject terminalCanvas;
private bool isActive = false;

private PlayerController playerController;

public string InteractionPrompt => prompt;

private void Awake()
{
    // Buscar el canvas con el tag "PantallaTerminal"
    terminalCanvas = GameObject.FindGameObjectWithTag("PantallaTerminal");

    if (terminalCanvas == null)
        Debug.LogWarning("No se encontró ningún objeto con el tag 'PantallaTerminal'");
    else
        terminalCanvas.SetActive(false);

    // Buscar automáticamente al jugador
    playerController = FindFirstObjectByType<PlayerController>();
}

public bool Interact(Interactor interactor)
{
    if (terminalCanvas == null) return false;

    // Alternar la pantalla
    isActive = !isActive;
    terminalCanvas.SetActive(isActive);

    // Activar/desactivar el control del jugador
    if (playerController != null)
        playerController.enabled = !isActive;

    Debug.Log(isActive ? "Terminal abierta (2.5D)" : "Terminal cerrada (2.5D)");

    return true;
}

public void CloseTerminal()
{
    if (terminalCanvas != null && isActive)
    {
        terminalCanvas.SetActive(false);
        isActive = false;

        if (playerController != null)
            playerController.enabled = true;

        Debug.Log("Terminal cerrada al salir (2.5D)");
    }
}
}

*/ //2º codigo
