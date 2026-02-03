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

        // --- BLOQUEO POR UBICACIÓN ---
        // Si la válvula llama hacia ARRIBA y el ascensor YA está ARRIBA, salimos.
        if (callUp && controlledElevator.IsAtUpperPoint())
        {
            Debug.Log("El ascensor ya está arriba.");
            return;
        }

        // Si la válvula llama hacia ABAJO y el ascensor YA está ABAJO, salimos.
        if (!callUp && controlledElevator.IsAtLowerPoint())
        {
            Debug.Log("El ascensor ya está abajo.");
            return;
        }
        // ----------------------------

        if (isTurning || controlledElevator.IsMoving()) return;

        isTurning = true;

        // 🔹 Activa animación de palanca (Solo llegará aquí si el ascensor debe moverse)
        if (valveAnim != null)
            valveAnim.PlayValveAnimationForElevator(controlledElevator);

        // 🔹 Mueve el ascensor
        if (callUp)
            controlledElevator.MoveUp();
        else
            controlledElevator.MoveDown();

        StartCoroutine(WaitForElevatorToStop(controlledElevator));
    }

    private System.Collections.IEnumerator WaitForElevatorToStop(Elevator elevator)
    {
        while (elevator.IsMoving())
            yield return null;

        isTurning = false;
    }
}
