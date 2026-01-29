using UnityEngine;
using UnityEngine.InputSystem;

public class P_ElectricityArea : MonoBehaviour
{
    [Header("Power Up")]
    [SerializeField] private bool hasElectricPower = false;

    [Header("Configuración de energía")]
    public float maxEnergy = 10f;
    public float currentEnergy = 10f;
    public float baseConsumption = 0.5f;    // gasto pasivo
    public float activeConsumption = 1f;    // gasto mientras usa electricidad

    

    [Header("Absorción de energía (editable)")]
    [Tooltip("Velocidad a la que se recarga energía de terminales (por segundo)")]
    public float absorbRate = 3f;

    [Header("Aura visual")]
    public GameObject electricityAura;
    public AnimationCurve auraGrowth;
    private float auraTime;

    [Header("Radio del aura (por ejes)")]
    public Vector3 auraRadius = new Vector3(3f, 3f, 3f); // editable desde Inspector

    [Header("Controles")]
    public Key usePowerKey = Key.Q;   // activar electricidad ofensiva
    public Key absorbKey = Key.F;     // recargar energía de terminales

    public float radius = 3f; // radio para detección de electrificables

    private bool isActive = false;

    private void Start()
    {
        if (electricityAura != null)
        {
            electricityAura.SetActive(false);
            electricityAura.transform.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        // 🔒 No puede usar electricidad si no tiene el poder
        if (!hasElectricPower)
            return;

        // --- ACTIVAR EL PODER (Q) ---
        if (Keyboard.current[usePowerKey].wasPressedThisFrame)
            StartElectricity();

        if (Keyboard.current[usePowerKey].wasReleasedThisFrame)
            StopElectricity();

        // --- USO ACTIVO DE ELECTRICIDAD ---
        if (isActive)
        {
            float consumption = baseConsumption + activeConsumption;
            currentEnergy -= consumption * Time.deltaTime;

            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                StopElectricity();
                Debug.Log("⚡ Energía agotada!");
                return;
            }

            ApplyElectricity();
            UpdateAuraVisual();
        }
        else
        {
            // Consumo pasivo
            currentEnergy -= baseConsumption * Time.deltaTime;
            if (currentEnergy < 0f)
                currentEnergy = 0f;
        }

        // --- ABSORBER ENERGÍA DE TERMINALES (F) ---
        if (Keyboard.current[absorbKey].isPressed && !isActive)
        {
            AbsorbEnergyFromTerminal();
        }
    }

    // =========================
    // POWER UP
    // =========================

    public void ActivatePower()
    {
        hasElectricPower = true;
        Debug.Log("⚡ Poder eléctrico desbloqueado");
    }

    public bool HasPower()
    {
        return hasElectricPower;
    }

    // =========================
    // ELECTRICIDAD ACTIVA
    // =========================

    private void StartElectricity()
    {
        if (currentEnergy <= 0f || isActive)
            return;

        isActive = true;
        auraTime = 0f;

        if (electricityAura != null)
        {
            electricityAura.SetActive(true);
            electricityAura.transform.localScale = Vector3.zero;
        }

        Debug.Log("⚡ Electricidad activada");
    }

    private void StopElectricity()
    {
        if (!isActive) return;

        isActive = false;
        auraTime = 0f;

        if (electricityAura != null)
        {
            electricityAura.SetActive(false);
            electricityAura.transform.localScale = Vector3.zero;
        }

        Debug.Log("💤 Electricidad desactivada");
    }

    // =========================
    // ACTUALIZAR AURA VISUAL
    // =========================
    private void UpdateAuraVisual()
    {
        if (electricityAura == null || auraGrowth == null || auraGrowth.length == 0)
        {
            if (electricityAura != null) electricityAura.transform.localScale = auraRadius;
            return;
        }

        // 1. CLAMP DEL TIEMPO: Evitamos que el tiempo siga creciendo y se salga de la curva
        auraTime += Time.deltaTime;
        float duration = auraGrowth[auraGrowth.length - 1].time; // Duración total de tu curva
        float evaluationTime = Mathf.Clamp(auraTime, 0, duration);

        float scaleFactor = auraGrowth.Evaluate(evaluationTime);

        // 2. APLICAR ESCALA
        electricityAura.transform.localScale = new Vector3(
            auraRadius.x * scaleFactor,
            auraRadius.y * scaleFactor,
            auraRadius.z * scaleFactor
        );

        // 3. LA SOLUCIÓN AL "PARPADEO": Forzar a Unity a ver el objeto
        // Si es un Mesh (Esfera/Cubo), actualizamos sus límites
        var renderer = electricityAura.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Esto le dice a Unity: "No me ocultes por optimización, mi tamaño ha cambiado"
            renderer.enabled = false;
            renderer.enabled = true;
        }
    }

    // =========================
    // APLICAR ELECTRICIDAD A OBJETOS
    // =========================
    private void ApplyElectricity()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Electrifiable"))
            {
                I_Electrifiable electrifiable = hit.GetComponent<I_Electrifiable>();
                if (electrifiable != null)
                    electrifiable.PowerOn();
            }
        }
    }

    // =========================
    // ABSORCIÓN DE ENERGÍA
    // =========================
    private void AbsorbEnergyFromTerminal()
    {
        if (currentEnergy >= maxEnergy)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Electrifiable"))
                continue;

            T_Electrifiable terminal = hit.GetComponent<T_Electrifiable>();
            if (terminal != null && terminal.IsPowered())
            {
                currentEnergy += absorbRate * Time.deltaTime; // valor editable por Inspector
                if (currentEnergy > maxEnergy)
                    currentEnergy = maxEnergy;

                Debug.Log("🔌 Absorbiendo energía de terminal");
                return; // solo una terminal a la vez
            }
        }
    }

    // =========================
    // RECARGAR ENERGÍA MANUAL
    // =========================
    public void RechargeEnergy(float amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        Debug.Log($"🔋 Energía recargada a {currentEnergy}/{maxEnergy}");
    }

    // =========================
    // DEBUG
    // =========================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
