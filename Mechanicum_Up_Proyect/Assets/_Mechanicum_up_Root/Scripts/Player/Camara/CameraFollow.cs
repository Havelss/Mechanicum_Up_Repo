using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform target;

    [Header("Default Camera Settings")]
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.1f;

    [Header("Current Limits (assigned by rooms)")]
    public Vector2 xLimits = new Vector2(-9999, 9999);
    public Vector2 yLimits = new Vector2(-9999, 9999);

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, xLimits.x, xLimits.y);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, yLimits.x, yLimits.y);

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPos;
    }

    // --- Métodos para modificar dinámicamente ---
    public void SetRoomLimits(Vector2 newXLimits, Vector2 newYLimits)
    {
        xLimits = newXLimits;
        yLimits = newYLimits;
    }

    public void SetCameraOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    public void SetSmoothSpeed(float newSmooth)
    {
        smoothSpeed = newSmooth;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


    // --- Gizmos para debug ---
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector3 p1 = new Vector3(xLimits.x, yLimits.x, transform.position.z);
        Vector3 p2 = new Vector3(xLimits.x, yLimits.y, transform.position.z);
        Vector3 p3 = new Vector3(xLimits.y, yLimits.y, transform.position.z);
        Vector3 p4 = new Vector3(xLimits.y, yLimits.x, transform.position.z);

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
