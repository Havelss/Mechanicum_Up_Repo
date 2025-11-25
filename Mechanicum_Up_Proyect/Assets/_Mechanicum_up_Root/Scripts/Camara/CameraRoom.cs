using UnityEngine;

public class CameraRoom : MonoBehaviour
{
    [Header("Room Camera Limits")]
    public Vector2 xLimits;
    public Vector2 yLimits;

    [Header("Optional Settings")]
    public bool useCustomOffset = false;
    public Vector3 customOffset;

    public bool useCustomSmooth = false;
    public float customSmoothSpeed = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();

            cam.SetRoomLimits(xLimits, yLimits);

            if (useCustomOffset)
                cam.SetCameraOffset(customOffset);

            if (useCustomSmooth)
                cam.SetSmoothSpeed(customSmoothSpeed);
        }
    }

    // Gizmo para ver el área de la sala fácilmente
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 p1 = new Vector3(xLimits.x, yLimits.x, 0);
        Vector3 p2 = new Vector3(xLimits.x, yLimits.y, 0);
        Vector3 p3 = new Vector3(xLimits.y, yLimits.y, 0);
        Vector3 p4 = new Vector3(xLimits.y, yLimits.x, 0);

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
