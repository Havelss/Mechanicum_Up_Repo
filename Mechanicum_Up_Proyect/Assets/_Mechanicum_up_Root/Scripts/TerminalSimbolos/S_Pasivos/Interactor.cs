using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayers;

    [Header("UI")]
    [SerializeField] private InteractionPromptUI promptUI;

    [Header("Animación")]
    [Tooltip("Arrastra aquí el Animator del Player (o se buscará solo si está en el mismo objeto)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string diskAnimationName = "Anim_Ibm_Link_001";

    private readonly Collider[] results = new Collider[6];
    private IInteractable currentInteractable;

    private void Awake()
    {
        // Si no asignaste el animator en el inspector, lo buscamos en el objeto o sus hijos
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
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
                currentInteractable = null;
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

            // 1. Caso: Terminal
            TerminalBox box = col.GetComponent<TerminalBox>();
            if (box != null)
            {
                box.Interact();
                return;
            }

            // 2. Caso: Objetos Interactuables (Discos, etc.)
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // --- CAMBIO AQUÍ: Detección de Disk para Animación ---
                if (col.CompareTag("Disk") && animator != null)
                {
                    // Reproducimos la animación directamente por nombre
                    animator.Play(diskAnimationName);
                    Debug.Log("Animación de recolección de disco activada");
                }

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