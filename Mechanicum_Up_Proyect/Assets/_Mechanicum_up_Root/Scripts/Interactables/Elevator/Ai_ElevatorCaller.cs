using UnityEngine;

public class Ai_ElevatorCaller : MonoBehaviour
{
    [Header("Referencia a la llamada del ascensor")]
    [SerializeField] private ElevatorCall elevatorCall; // Arrastra aquí el GameObject que tiene ElevatorCall

    [Header("Opciones de llamada automática")]
    [SerializeField] private bool callUp = true; // true = subir, false = bajar
    [SerializeField] private bool callOnStart = false; // Llamar automáticamente al inicio

    private void Start()
    {
        if (callOnStart)
            CallElevator();
    }

    /// <summary>
    /// Ejecuta la llamada al ascensor usando el ElevatorCall asignado
    /// </summary>
    public void CallElevator()
    {
        if (elevatorCall == null)
        {
            Debug.LogWarning("[Ai_ElevatorCaller] No hay ElevatorCall asignado.");
            return;
        }

        // Configura la dirección de la llamada
        elevatorCall.callUp = callUp;

        // Llama a la función de interacción directamente
        elevatorCall.Interact(null); // Si no hay interactor, puedes pasar null
    }

    /// <summary>
    /// Si quieres, puedes llamar desde otro script o trigger
    /// </summary>
    public void CallElevator(bool directionUp)
    {
        if (elevatorCall == null) return;
        elevatorCall.callUp = directionUp;
        elevatorCall.Interact(null);
    }
}
