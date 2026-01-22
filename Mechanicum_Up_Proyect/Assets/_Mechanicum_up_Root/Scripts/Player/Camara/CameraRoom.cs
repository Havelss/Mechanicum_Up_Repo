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

    private CameraFollow camara;


    private void Start()
    {
        camara = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (camara == null)
        {
            camara = Camera.main.GetComponent<CameraFollow>();
            if (camara == null)
            {
                Debug.LogError("CameraFollow no encontrado en la MainCamera!");
                return;
            }
        }

        camara.SetRoomLimits(xLimits, yLimits);
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
