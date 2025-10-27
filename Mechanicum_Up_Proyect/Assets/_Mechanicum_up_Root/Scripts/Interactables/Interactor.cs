using UnityEngine;
using UnityEngine.InputSystem;


#region bastante bien
/*
public class Interactor : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 2f; // Ajusta según el jugador
    [Tooltip("Selecciona aquí los layers: Terminal y Interactable")]
    [SerializeField] private LayerMask interactableLayers;

    [Header("UI")]
    [SerializeField] private InteractionPromptUI promptUI;

    private readonly Collider[] results = new Collider[6];
    private IInteractable currentInteractable;

    private SO_Terminal globalTerminal;

    private void Start()
    {
        // Terminal global única
        globalTerminal = FindFirstObjectByType<SO_Terminal>();
        if (globalTerminal == null)
            Debug.LogWarning("No se encontró la terminal global en la escena.");
    }

    private void Update()
    {
        DetectInteractables();
        HandleInput();
    }

    private void DetectInteractables()
    {
        int numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position,
            interactionRadius,
            results,
            interactableLayers
        );

        currentInteractable = null;

        for (int i = 0; i < numFound; i++)
        {
            Collider col = results[i];

            // Terminal
            TerminalBox box = col.GetComponent<TerminalBox>();
            if (box != null)
            {
                currentInteractable = null; // Terminal global no es IInteractable
                if (promptUI != null && !promptUI.IsDisplayed)
                    promptUI.SetUp(box.GetPrompt());
                return;
            }

            // Mesas de símbolos u otros IInteractable
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                if (promptUI != null && !promptUI.IsDisplayed)
                    promptUI.SetUp(interactable.InteractionPrompt);
                return;
            }
        }

        // Si no hay nada cercano
        if (promptUI != null && promptUI.IsDisplayed)
            promptUI.Close();
    }

    private void HandleInput()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;

        int numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position,
            interactionRadius,
            results,
            interactableLayers
        );

        for (int i = 0; i < numFound; i++)
        {
            Collider col = results[i];

            TerminalBox box = col.GetComponent<TerminalBox>();
            if (box != null && globalTerminal != null)
            {
                globalTerminal.SetControlledObject(box.GetControlledObject());
                globalTerminal.OpenTerminal();  // ahora siempre abrirá con la caja que estés tocando
                return;
            }

            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    }
}
*/
#endregion


public class Interactor : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayers; // Terminal y mesas de símbolos

    [Header("UI")]
    [SerializeField] private InteractionPromptUI promptUI;

    private readonly Collider[] results = new Collider[6];
    private IInteractable currentInteractable;

    private SO_Terminal globalTerminal;

    private void Start()
    {
        globalTerminal = FindFirstObjectByType<SO_Terminal>();
        if (globalTerminal == null)
            Debug.LogWarning("No se encontró la terminal global en la escena.");
    }

    private void Update()
    {
        DetectInteractables();
        HandleInput();
    }

    private void DetectInteractables()
    {
        int numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position,
            interactionRadius,
            results,
            interactableLayers
        );

        currentInteractable = null;

        for (int i = 0; i < numFound; i++)
        {
            Collider col = results[i];

            TerminalBox box = col.GetComponent<TerminalBox>();
            if (box != null)
            {
                currentInteractable = null; // no IInteractable, es una terminal
                if (promptUI != null && !promptUI.IsDisplayed)
                    promptUI.SetUp(box.GetPrompt());
                return;
            }

            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                if (promptUI != null && !promptUI.IsDisplayed)
                    promptUI.SetUp(interactable.InteractionPrompt);
                return;
            }
        }

        if (promptUI != null && promptUI.IsDisplayed)
            promptUI.Close();
    }

    private void HandleInput()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;

        int numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position,
            interactionRadius,
            results,
            interactableLayers
        );

        for (int i = 0; i < numFound; i++)
        {
            Collider col = results[i];

            TerminalBox box = col.GetComponent<TerminalBox>();
            if (box != null && globalTerminal != null)
            {
                globalTerminal.SetControlledObject(box.GetControlledObject());
                globalTerminal.OpenTerminal();
                return;
            }

            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    }
}