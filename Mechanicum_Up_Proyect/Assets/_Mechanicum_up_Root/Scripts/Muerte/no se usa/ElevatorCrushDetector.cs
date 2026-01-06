using UnityEngine;

public class ElevatorCrushDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Elevator elevator;   // referencia a tu script Elevator
    [SerializeField] private string playerTag = "Player";

    [Header("Crush Settings")]
    [SerializeField] private Vector3 boxSize = new Vector3(0.5f, 1f, 0.5f); // tamaño del box para detectar al jugador
    [SerializeField] private LayerMask crushLayers; // suelo, techo, elevador, etc.

    private void Reset()
    {
        crushLayers = LayerMask.GetMask("Default");
    }

    private void FixedUpdate()
    {
        if (!elevator.IsMoving())
            return;

        // Detectamos todo lo que esté dentro del box (jugador)
        Collider[] hits = Physics.OverlapBox(transform.position, boxSize * 0.5f, Quaternion.identity, crushLayers);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag(playerTag))
                continue;

            KillPlayer(hit.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        // var playerController = player.GetComponent<PlayerController>();
        //   if (playerController != null && !playerController.IsDead())
        {
            //     playerController.DieInstant("crush");
            return;
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Visualizar el box en la escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }


}
