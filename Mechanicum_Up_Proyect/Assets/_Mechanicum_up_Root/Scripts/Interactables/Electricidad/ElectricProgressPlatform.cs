using UnityEngine;

// Plataforma que se mueve progresivamente mientras recibe electricidad
public class ElectricProgressPlatform : MonoBehaviour, I_Electrifiable
{
    [Header("Puntos de movimiento")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("Progresión")]
    [SerializeField] private float progressSpeed = 0.5f;

    private float progress = 0f; // 0 = inicio | 1 = final
    private bool receivingElectricity = false;

    private void Update()
    {
        // Si recibe electricidad avanza, si no, retrocede
        if (receivingElectricity)
            progress += progressSpeed * Time.deltaTime;
        else
            progress -= progressSpeed * Time.deltaTime;

        // Limitar progreso
        progress = Mathf.Clamp01(progress);

        // Mover plataforma según progreso
        if (startPoint != null && endPoint != null)
        {
            transform.position = Vector3.Lerp(
                startPoint.position,
                endPoint.position,
                progress
            );
        }

        // ⚠️ Muy importante: resetear cada frame
        receivingElectricity = false;
    }

    // =========================
    // ELECTRICIDAD (FRAME-BASED)
    // =========================
    public void PowerOn()
    {
        receivingElectricity = true;
    }

    public void PowerOff()
    {
        // No se usa (no queremos apagado permanente)
    }

    public bool IsPowered()
    {
        return receivingElectricity;
    }
}
