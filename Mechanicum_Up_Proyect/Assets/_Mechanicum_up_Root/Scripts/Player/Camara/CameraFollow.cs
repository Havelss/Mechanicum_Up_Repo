using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform target;

    [Header("Default Camera Settings")]
    public Vector3 defaultOffset = new Vector3(0, 5, -10);
    private Vector3 currentOffset;
    private Vector3 offsetVelocity = Vector3.zero; // Para el SmoothDamp
    public float smoothSpeed = 0.1f;

    [Header("Ceiling Logic (Pro Zoom)")]
    [SerializeField] private string ceilingTag = "techo";
    [SerializeField] private float ceilingCheckDistance = 3.0f;

    [Tooltip("¿Cuánto se acerca la cámara? 1 es normal, 0.2 es pegado al personaje")]
    [Range(0.1f, 1f)]
    [SerializeField] private float maxZoomIntensity = 0.3f;

    [Tooltip("Suavizado del zoom. Valores más altos = más lento y orgánico")]
    [SerializeField] private float zoomSmoothTime = 0.3f;

    [Header("Current Limits")]
    public Vector2 xLimits = new Vector2(-9999, 9999);
    public Vector2 yLimits = new Vector2(-9999, 9999);

    private void Start()
    {
        currentOffset = defaultOffset;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        HandleDynamicZoom();

        // Posición deseada con el offset calculado
        Vector3 desiredPosition = target.position + currentOffset;

        // Límites
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, xLimits.x, xLimits.y);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, yLimits.x, yLimits.y);

        // Movimiento de la cámara (Sigue usando Lerp para el seguimiento del player)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    private void HandleDynamicZoom()
    {
        RaycastHit hit;
        // Lanzamos el rayo un poco más largo para anticipar el techo
        float rayLength = defaultOffset.y + ceilingCheckDistance;
        Vector3 targetOffset = defaultOffset;

        if (Physics.Raycast(target.position, Vector3.up, out hit, rayLength))
        {
            if (hit.collider.CompareTag(ceilingTag))
            {
                // Calculamos la cercanía del techo (0 es lejos, 1 es encima del player)
                float closeness = 1.0f - Mathf.Clamp01(hit.distance / rayLength);

                // Aplicamos una intensidad más fuerte al zoom
                // Cuanto más cerca el techo, más se acerca al multiplicador maxZoomIntensity
                float zoomFactor = Mathf.Lerp(1.0f, maxZoomIntensity, closeness);

                targetOffset = defaultOffset * zoomFactor;
            }
        }

        // --- CAMBIO CLAVE: SmoothDamp en lugar de Lerp ---
        // SmoothDamp es mucho más fluido para cambios de zoom dinámicos
        currentOffset = Vector3.SmoothDamp(currentOffset, targetOffset, ref offsetVelocity, zoomSmoothTime);
    }

    // --- MÉTODOS PÚBLICOS ---
    public void SetTarget(Transform newTarget) => target = newTarget;

    public void SetRoomLimits(Vector2 newXLimits, Vector2 newYLimits)
    {
        xLimits = newXLimits; yLimits = newYLimits;
    }

    public void SetCameraOffset(Vector3 newOffset)
    {
        defaultOffset = newOffset;
        currentOffset = newOffset;
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(target.position, Vector3.up * (defaultOffset.y + ceilingCheckDistance));
        }
    }
}