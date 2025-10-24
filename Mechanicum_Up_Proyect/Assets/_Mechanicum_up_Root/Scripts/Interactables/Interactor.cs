using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 1f;
    [Tooltip("Selecciona aquí los layers: Terminal y Interactable")]
    [SerializeField] private LayerMask interactableLayers;

    [Header("UI")]
    [SerializeField] private InteractionPromptUI promptUI;

    private readonly Collider[] results = new Collider[5];
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

        if (numFound > 0)
        {
            IInteractable interactable = results[0].GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                // Mostrar prompt si no está visible
                if (!promptUI.IsDisplayed)
                    promptUI.SetUp(interactable.InteractionPrompt);
                return;
            }
        }

        // Si no hay nada cerca, limpia referencias
        if (currentInteractable != null)
        {
            if (currentInteractable is SO_Terminal terminal)
                terminal.CloseTerminal();

            currentInteractable = null;
        }

        if (promptUI.IsDisplayed)
            promptUI.Close();
    }

    private void HandleInput()
    {
        if (currentInteractable == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    }
}
