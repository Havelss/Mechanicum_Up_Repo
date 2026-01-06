using UnityEngine;

public class GatoCrushDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Transform movingParent; // el objeto que mueve al gato, por ejemplo el elevador

    [Header("Crush Settings")]
    [SerializeField] private Vector3 boxSize = new Vector3(0.5f, 1f, 0.5f);
    [SerializeField] private LayerMask crushLayers;

    private void Reset()
    {
        crushLayers = LayerMask.GetMask("Default");
    }

    private void Start()
    {
        // Si quieres que el gato se mueva con el elevador u objeto, lo hacemos hijo
        if (movingParent != null)
        {
            transform.SetParent(movingParent);
        }
    }

    private void FixedUpdate()
    {
        // Detectamos jugadores dentro del volumen de aplastamiento
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
        //   var playerController = player.GetComponent<PlayerController>();
        //   if (playerController != null && !playerController.IsDead())
        {
            //      playerController.DieInstant("crush"); // muerte instantánea con respawn
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
