using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform target; // El jugador a seguir

    [Header("Camera Settings")]
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10); // Distancia de la cámara
    [SerializeField] float smoothSpeed = 0.1f; // Suavizado

    [Header("Optional Limits")]
    [SerializeField] Vector2 xLimits = new Vector2(-Mathf.Infinity, Mathf.Infinity);
    [SerializeField] Vector2 yLimits = new Vector2(-Mathf.Infinity, Mathf.Infinity);

    private void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada
        Vector3 desiredPosition = target.position + offset;

        // Aplicar límites
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, xLimits.x, xLimits.y);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, yLimits.x, yLimits.y);

        // Suavizar movimiento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
