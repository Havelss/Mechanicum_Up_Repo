using UnityEngine;
using System.Collections;

public class Ai_ElevatorCaller : MonoBehaviour
{
    [Header("Referencia a la llamada del ascensor")]
    [SerializeField] private ElevatorCall elevatorCall; // GameObject con ElevatorCall

    [Header("Opciones de llamada")]
    [SerializeField] private bool callUp = true;         // true = subir, false = bajar
    [SerializeField] private float callDelay = 1f;       // Tiempo entre llamadas
    [SerializeField] private float detectionRange = 3f;  // Distancia para “detectar al jugador”
    [SerializeField] private Transform playerTransform;  // Transform del jugador

    private bool isCalling = true;

    private void Start()
    {
        if (elevatorCall == null)
        {
            Debug.LogWarning("[Ai_ElevatorCaller] No hay ElevatorCall asignado.");
            enabled = false;
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning("[Ai_ElevatorCaller] No hay jugador asignado para la detección.");
            enabled = false;
            return;
        }

        StartCoroutine(CallLoop());
    }

    private IEnumerator CallLoop()
    {
        while (isCalling)
        {
            // Llama al ascensor
            elevatorCall.callUp = callUp;
            elevatorCall.Interact(null);

            // Espera el tiempo configurado
            yield return new WaitForSeconds(callDelay);

            // Comprueba si el jugador está dentro del rango
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            if (distance <= detectionRange)
            {
                isCalling = false;
                Destroy(gameObject); // Se elimina el objeto que hace la llamada
            }
        }
    }
}
