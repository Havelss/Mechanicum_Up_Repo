using UnityEngine;

public class ElectricProgressPlatformWithPlayer : MonoBehaviour, I_Electrifiable
{
    [Header("Puntos de movimiento")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("Progresión")]
    [SerializeField] private float progressSpeed = 0.5f;
    [SerializeField] private float powerOffDelay = 1.5f; // Tiempo de espera antes de retroceder

    private Rigidbody rb;
    private float progress = 0f;
    private bool receivingElectricity = false;
    private float lastPowerTime; // Registra el último momento con energía

    // Para mover al player con la plataforma
    private Rigidbody playerRB;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        lastPowerTime = -powerOffDelay; // Empezar sin energía
    }

    private void FixedUpdate()
    {
        // Lógica de Retardo: 
        // Si estamos recibiendo energía O si el tiempo actual es menor al último contacto + el delay...
        bool stayPowered = receivingElectricity || (Time.time <= lastPowerTime + powerOffDelay);

        // Progresión de la plataforma
        if (stayPowered)
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
        }

        // Resetear la señal cada frame (el aura debe llamar a PowerOn constantemente)
        receivingElectricity = false;
    }

    // =========================
    // I_Electrifiable
    // =========================
    public void PowerOn()
    {
        receivingElectricity = true;
        lastPowerTime = Time.time; // Actualizamos el cronómetro mientras haya contacto
    }

    public void PowerOff()
    {
        // Opcional: Podrías forzar el apagado inmediato aquí si quisieras
    }

    public bool IsPowered() => receivingElectricity || (Time.time <= lastPowerTime + powerOffDelay);

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