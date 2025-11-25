using UnityEngine;

public class ElevatorCrushDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Elevator elevator;   // referencia a tu script Elevator
    [SerializeField] private string playerTag = "Player";

    [Header("Crush Settings")]
    [SerializeField] private float crushCheckDistance = 0.2f;
    [SerializeField] private LayerMask crushLayers; // suelo, techo, elevador, etc.

    private void Reset()
    {
        crushLayers = LayerMask.GetMask("Default");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        // si el ascensor NO se está moviendo, no aplasta
        if (!elevator.IsMoving())
            return;

        // ¿Está el jugador atrapado entre el ascensor y otro collider?
        if (IsPlayerCrushed(other.transform))
        {
            KillPlayer(other.gameObject);
        }
    }

    private bool IsPlayerCrushed(Transform player)
    {
        // Raycast hacia arriba y abajo desde el jugador
        bool hitUp = Physics.Raycast(player.position, Vector3.up, crushCheckDistance, crushLayers);
        bool hitDown = Physics.Raycast(player.position, Vector3.down, crushCheckDistance, crushLayers);

        // Si está tocando techo y suelo/ascensor a la vez → aplastado
        return hitUp && hitDown;
    }

    private void KillPlayer(GameObject player)
    {
        // Intentamos usar PlayerRespawn para que siga el flujo de respawn
        PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            // Llamamos a Die() del PlayerController directamente para no usar vida
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null && !playerController.IsDead())
            {
                playerController.Die("crush");
            }
            return;
        }

        // Si no tiene PlayerRespawn, destruimos como fallback
        Destroy(player);

        Debug.Log("[ElevatorCrushDetector] Jugador aplastado y eliminado (sin respawn).");
    }
}
