using UnityEngine;

public class ElevatorCall : MonoBehaviour, IInteractable
{
    [Header("Configuración de la llamada")]
    [SerializeField] private Elevator controlledElevator;
    [SerializeField] public bool callUp = true;
    [SerializeField] private string promptMessage = "Usar válvula";

    [Header("Animación")]
    [SerializeField] private ValveAnimationController valveAnim;

    private bool isTurning = false;

    public string InteractionPrompt => promptMessage;

    public void Interact(Interactor interactor)
    {
        if (controlledElevator == null)
        {
            Debug.LogWarning($"{name} no tiene asignado un Elevator válido.");
            return;
        }

        if (isTurning || controlledElevator.IsMoving()) return;

        isTurning = true;

        // 🔹 Activa animación de giro
        if (valveAnim != null)
            valveAnim.PlayValveRotation(callUp);

        // 🔹 Mueve el ascensor
        if (callUp)
            controlledElevator.MoveUp();
        else
            controlledElevator.MoveDown();

        // 🔹 Espera a que termine
        StartCoroutine(WaitForElevatorToStop(controlledElevator));
    }

    private System.Collections.IEnumerator WaitForElevatorToStop(Elevator elevator)
    {
        while (elevator.IsMoving())
            yield return null;

        

        isTurning = false;
    }
}
