using UnityEngine;

public class InstantKillZone : MonoBehaviour
{
    [Header("Opciones de movimiento")]
    [Tooltip("Si se quiere que el jugador siga el objeto en movimiento (tipo elevador)")]
    public Transform movingParent;

    [Header("Configuración")]
    public string playerTag = "Player";

    [Header("Zona de Aplastamiento")]
    [Tooltip("Tamaño del área de detección (ancho, alto, profundidad)")]
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);
    public LayerMask crushLayers = ~0; // Todos los layers por defecto

    private void Start()
    {
        // Si quieres que el jugador se mueva con este objeto
        if (movingParent != null)
        {
            transform.SetParent(movingParent);
        }
    }

    private void FixedUpdate()
    {
        // Detecta jugadores dentro del box
        Collider[] hits = Physics.OverlapBox(transform.position, boxSize * 0.5f, Quaternion.identity, crushLayers);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag(playerTag)) continue;

            PlayerController playerController = hit.GetComponent<PlayerController>();
            if (playerController != null && !playerController.IsDead())
            {
                playerController.DieInstant("crush");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
