using UnityEngine;

public class ElevatorCall : MonoBehaviour, IInteractable
{
    [Header("Configuración de la llamada")]
    [SerializeField] private MonoBehaviour controlledObject; // El ascensor controlado
    [SerializeField] public bool callUp = true;              // true = subir, false = bajar
    [SerializeField] private string promptMessage = "Usar válvula";

    [Header("Animación")]
    [SerializeField] private AnimationController valveAnim;  // Controlador de animación de la válvula

    private bool isTurning = false;

    //private void Start()
    //{
    //    valveAnim.GetComponentInChildren<Animator>();
    //}
    public string InteractionPrompt => promptMessage;

    public void Interact(Interactor interactor)
    {
        if (!(controlledObject is Elevator elevator))
        {
            Debug.LogWarning($"{name} no tiene asignado un Elevator válido.");
            return;
        }

        // Evita activar si ya está en uso o el ascensor se está moviendo
        if (isTurning || elevator.IsMoving()) return;

        isTurning = true;

        // Reproduce animación de la válvula mientras el ascensor se mueve
        if (valveAnim != null)
            valveAnim.PlayAnimation("ValveTurn", "Idle");

        // Llama al ascensor
        if (callUp)
            elevator.MoveUp();
        else
            elevator.MoveDown();

        // Corrutina para esperar a que el ascensor termine
        StartCoroutine(WaitForElevatorToStop(elevator));
    }

    private System.Collections.IEnumerator WaitForElevatorToStop(Elevator elevator)
    {
        // Espera mientras el ascensor se mueve
        while (elevator.IsMoving())
            yield return null;

        // Cambia a Idle cuando termina
        if (valveAnim != null)
            valveAnim.PlayAnimation("Idle");

        isTurning = false;
    }

    public MonoBehaviour GetControlledObject()
    {
        return controlledObject;
    }
}
