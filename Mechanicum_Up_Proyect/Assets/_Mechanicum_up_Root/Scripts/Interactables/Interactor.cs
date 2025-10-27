using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayers; // Terminal y objetos interactuables

    [Header("UI")]
    [SerializeField] private InteractionPromptUI promptUI;

    private readonly Collider[] results = new Collider[6];

    private void Update()
    {
        DetectInteractables();
        HandleInput();
    }

    private void DetectInteractables()
    {
        int numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius, results, interactableLayers);

        for (int i = 0; i < numFound; i++)
        {
            Collider col = results[i];

            // Detectar terminales
            if (col.TryGetComponent(out TerminalBox box))
            {
                if (!promptUI.IsDisplayed)
                    promptUI.SetUp(box.GetPrompt());
                return;
            }

            // Detectar IInteractables (símbolos, etc.)
            if (col.TryGetComponent(out IInteractable interactable))
            {
                if (!promptUI.IsDisplayed)
                    promptUI.SetUp(interactable.InteractionPrompt);
                return;
            }
        }

        if (promptUI.IsDisplayed)
            promptUI.Close();
    }

    private void HandleInput()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;

        int numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius, results, interactableLayers);

        for (int i = 0; i < numFound; i++)
        {
            Collider col = results[i];

            // Si es una terminal
            if (col.TryGetComponent(out TerminalBox box))
            {
                box.Interact();
                return;
            }

            // Si es otro interactuable
            if (col.TryGetComponent(out IInteractable interactable))
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
