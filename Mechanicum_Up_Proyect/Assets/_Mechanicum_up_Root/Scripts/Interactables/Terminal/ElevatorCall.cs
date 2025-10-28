using UnityEngine;

public class ElevatorCall : MonoBehaviour, IInteractable
{
    [SerializeField] private MonoBehaviour controlledObject; // el ascensor
    [SerializeField] public bool callUp = true;             // true = Up, false = Down
    [SerializeField] private string promptMessage = "Usar tubería";

    public string InteractionPrompt => promptMessage;

    public void Interact(Interactor interactor)
    {
        if (controlledObject is Elevator elevator)
        {
            if (callUp)
                elevator.MoveUp();
            else
                elevator.MoveDown();
        }
        else
        {
            Debug.LogWarning($"{name} no tiene asignado un Elevator válido.");
        }
    }
}
