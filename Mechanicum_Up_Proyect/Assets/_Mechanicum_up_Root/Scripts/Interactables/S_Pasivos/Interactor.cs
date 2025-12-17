using UnityEngine;
using UnityEngine.InputSystem;

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
                currentInteractable = null; // Terminal no es IInteractable
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
            if (box != null)
            {
                box.Interact(); // abre la terminal correspondiente con el objeto controlado
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
