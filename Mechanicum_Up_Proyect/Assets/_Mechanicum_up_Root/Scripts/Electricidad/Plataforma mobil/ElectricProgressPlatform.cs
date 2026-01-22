using UnityEngine;

// Plataforma progresiva que mueve al player mientras recibe electricidad
public class ElectricProgressPlatformWithPlayer : MonoBehaviour, I_Electrifiable
{
    [Header("Puntos de movimiento")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("Progresión")]
    [SerializeField] private float progressSpeed = 0.5f; // velocidad de avance/reversa

    private Rigidbody rb;
    private float progress = 0f; // 0 = inicio | 1 = final
    private bool receivingElectricity = false;

    // Para mover al player con la plataforma
    private Rigidbody playerRB;
    private Vector3 lastPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        lastPosition = rb.position;
    }

    private void FixedUpdate()
    {
        // Progresión de la plataforma
        if (receivingElectricity)
            progress += progressSpeed * Time.fixedDeltaTime;
        else
            progress -= progressSpeed * Time.fixedDeltaTime;

        progress = Mathf.Clamp01(progress);

        // Mover plataforma usando Rigidbody
        if (startPoint != null && endPoint != null)
        {
            Vector3 targetPos = Vector3.Lerp(startPoint.position, endPoint.position, progress);
            Vector3 delta = targetPos - rb.position;
            rb.MovePosition(targetPos);

            // Mover al player junto con la plataforma
            if (playerRB != null)
            {
                playerRB.MovePosition(playerRB.position + delta);
            }

            lastPosition = rb.position;
        }

        // Resetear electricidad cada frame
        receivingElectricity = false;
    }

    // =========================
    // I_Electrifiable
    // =========================
    public void PowerOn()
    {
        receivingElectricity = true;
    }

    public void PowerOff()
    {
        // No se usa, pero mantiene la interfaz
    }

    public bool IsPowered()
    {
        return receivingElectricity;
    }

    // =========================
    // Detectar player sobre la plataforma
    // =========================
    private void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody != null && col.gameObject.CompareTag("Player"))
        {
            playerRB = col.rigidbody;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.rigidbody == playerRB)
        {
            playerRB = null;
        }
    }
}
