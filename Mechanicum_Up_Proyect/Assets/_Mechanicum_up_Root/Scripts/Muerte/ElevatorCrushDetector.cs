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
            TryKillPlayer(other.gameObject);
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

    private void TryKillPlayer(GameObject player)
    {
        // Sistema de muerte personalizado
        var health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.DieByCrush();
            return;
        }

        // Fallback a respawn
        var respawn = player.GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            respawn.Respawn();
            return;
        }

        Debug.LogWarning("El jugador fue aplastado, pero no tiene sistema de muerte asignado.");
    }
}
