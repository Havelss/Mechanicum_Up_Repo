using UnityEngine;
using System.Collections;

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
        if (callUp && controlledElevator.IsAtUpperPoint())
        {
            Debug.Log("El ascensor ya está arriba.");
            return;
        }

        if (!callUp && controlledElevator.IsAtLowerPoint())
        {
            Debug.Log("El ascensor ya está abajo.");
            return;
        }

        // Si ya está funcionando, ignoramos la interacción
        if (isTurning || controlledElevator.IsMoving()) return;

        isTurning = true;

        // 1. Activa animación física de la palanca
        if (valveAnim != null)
            valveAnim.PlayValveAnimationForElevator();

        // 2. Mueve el ascensor
        if (callUp)
            controlledElevator.MoveUp();
        else
            controlledElevator.MoveDown();

        // Iniciamos la espera para permitir una nueva interacción después
        StartCoroutine(WaitForElevatorToStop(controlledElevator));
    }

    private IEnumerator WaitForElevatorToStop(Elevator elevator)
    {
        // Esperamos un frame por si el inicio del movimiento no es instantáneo
        yield return null;

        while (elevator.IsMoving())
            yield return null;

        isTurning = false;
    }
}