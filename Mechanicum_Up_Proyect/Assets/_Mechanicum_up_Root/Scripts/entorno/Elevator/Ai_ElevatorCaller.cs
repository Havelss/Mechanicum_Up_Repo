using UnityEngine;
using System.Collections;

public class Ai_ElevatorCaller : MonoBehaviour
{
    [Header("Referencia a la llamada del ascensor")]
    [SerializeField] private ElevatorCall elevatorCall;

    [Header("Opciones de llamada")]
    [SerializeField] private bool callUp = true;
    [SerializeField] private float callDelay = 1f;
    [SerializeField] private float detectionRange = 3f;

    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private string playerTag = "Player"; // 🔹 NUEVO

    private bool isCalling = true;

    private void Start()
    {
        if (elevatorCall == null)
        {
            Debug.LogWarning("[Ai_ElevatorCaller] No hay ElevatorCall asignado.");
            enabled = false;
            return;
        }

        StartCoroutine(CallLoop());
    }

    private IEnumerator CallLoop()
    {
        while (isCalling)
        {
            // 🔎 Buscar player si no existe (respawn-safe)
            TryAssignPlayer();

            if (playerTransform != null)
            {
                // Llamar al ascensor
                elevatorCall.callUp = callUp;
                elevatorCall.Interact(null);

                float distance = Vector3.Distance(playerTransform.position, transform.position);

                if (distance <= detectionRange)
                {
                    Debug.Log("[Ai_ElevatorCaller] Player detectado. Deteniendo llamadas.");
                    isCalling = false;
                    Destroy(gameObject);
                    yield break;
                }
            }

            yield return new WaitForSeconds(callDelay);
        }
    }

    private void TryAssignPlayer()
    {
        if (playerTransform != null) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);

        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            Debug.Log("[Ai_ElevatorCaller] Player asignado automáticamente.");
        }
    }
}
