using UnityEngine;

public class GatoCrushDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GatoAnimationController gatoController; // referencia al gato
    [SerializeField] private string playerTag = "Player";

    [Header("Crush Settings")]
    [SerializeField] private Vector3 boxSize = new Vector3(0.5f, 1f, 0.5f);
    [SerializeField] private LayerMask crushLayers;

    private void Reset()
    {
        crushLayers = LayerMask.GetMask("Default");
    }

    private void FixedUpdate()
    {
        if (gatoController == null) return;

        // Detectamos jugadores dentro del volumen de aplastamiento
        Collider[] hits = Physics.OverlapBox(transform.position, boxSize * 0.5f, Quaternion.identity, crushLayers);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag(playerTag))
                continue;

            KillPlayer(hit.gameObject);
            PlayGatoAnimation();
        }
    }

    private void KillPlayer(GameObject player)
    {
        var playerController = player.GetComponent<PlayerController>();
        if (playerController != null && !playerController.IsDead())
        {
            playerController.DieInstant("crush"); // muerte instantánea
        }
    }

    private void PlayGatoAnimation()
    {
        if (gatoController != null)
        {
            // Por ejemplo, hacer que el gato baje instantáneamente
            gatoController.PlayInstant(gatoController.downAnimation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
