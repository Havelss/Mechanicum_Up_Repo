//using UnityEngine;
//using UnityEngine.InputSystem;

//public class P_ElectricityArea : MonoBehaviour
//{
//    [Header("Configuración de energía")]
//    public float maxEnergy = 10f;
//    public float currentEnergy = 10f;
//    public float baseConsumption = 0.5f;
//    public float activeConsumption = 1f;
//    public float radius = 3f;

//    [Header("Aura visual")]
//    public GameObject electricityAura;   // Asigna una esfera o sistema de partículas
//    public AnimationCurve auraGrowth;    // Opcional: curva para el crecimiento del aura
//    private float auraTime;

//    [Header("Controles")]
//    public Key activateKey = Key.F;

//    private bool isActive = false;
//    private P_ElectricityPowerUp powerUp;



//    private void Start()
//    {
//        powerUp = GetComponent<P_ElectricityPowerUp>();

//        if (electricityAura != null)
//        {
//            electricityAura.SetActive(false);
//            electricityAura.transform.localScale = Vector3.zero;
//        }
//    }

//    private void Update()
//    {
//        // Si no tiene el poder eléctrico aún, no puede usar la electricidad
//        if (powerUp == null || !powerUp.HasPower())
//            return;

//        // Detectar pulsación para activar
//        if (Keyboard.current[activateKey].wasPressedThisFrame)
//        {
//            StartElectricity();
//        }

//        // Detectar cuándo se suelta
//        if (Keyboard.current[activateKey].wasReleasedThisFrame)
//        {
//            StopElectricity();
//        }

//        // Si está activa, consumir energía y mantener el aura
//        if (isActive)
//        {
//            float consumption = baseConsumption + activeConsumption;
//            currentEnergy -= consumption * Time.deltaTime;

//            if (currentEnergy <= 0f)
//            {
//                currentEnergy = 0f;
//                StopElectricity();
//                Debug.Log("⚡ Energía agotada!");
//                return;
//            }

//            ApplyElectricity();
//            UpdateAuraVisual();
//        }
//        else
//        {
//            // Consumo pasivo mínimo
//            currentEnergy -= baseConsumption * Time.deltaTime;
//            if (currentEnergy < 0f)
//                currentEnergy = 0f;
//        }
//    }

//    private void StartElectricity()
//    {
//        if (currentEnergy <= 0f)
//        {
//            Debug.Log("❌ No hay energía suficiente.");
//            return;
//        }

//        if (isActive) return;

//        isActive = true;
//        auraTime = 0f;

//        if (electricityAura != null)
//        {
//            electricityAura.SetActive(true);
//            electricityAura.transform.localScale = Vector3.zero;
//        }

//        Debug.Log("⚡ Electricidad activada");
//    }

//    private void StopElectricity()
//    {
//        if (!isActive) return;

//        isActive = false;
//        auraTime = 0f;

//        if (electricityAura != null)
//        {
//            electricityAura.SetActive(false);
//            electricityAura.transform.localScale = Vector3.zero;
//        }

//        Debug.Log("💤 Electricidad desactivada");
//    }

//    private void UpdateAuraVisual()
//    {
//        if (electricityAura == null)
//            return;

//        if (auraGrowth != null && auraGrowth.length > 0)
//        {
//            auraTime += Time.deltaTime;
//            float scaleFactor = auraGrowth.Evaluate(auraTime);
//            electricityAura.transform.localScale = Vector3.one * radius * scaleFactor;
//        }
//        else
//        {
//            // Si no hay curva, usa una escala constante
//            electricityAura.transform.localScale = Vector3.one * radius;
//        }
//    }

//    private void ApplyElectricity()
//    {
//        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
//        foreach (var hit in hits)
//        {
//            if (hit.CompareTag("Electrifiable"))
//            {
//                I_Electrifiable electrifiable = hit.GetComponent<I_Electrifiable>();
//                if (electrifiable != null)
//                    electrifiable.PowerOn();
//            }
//        }
//    }

//    public void RechargeEnergy(float amount)
//    {
//        currentEnergy += amount;
//        if (currentEnergy > maxEnergy)
//            currentEnergy = maxEnergy;

//        Debug.Log($"🔋 Energía recargada a {currentEnergy}/{maxEnergy}");
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, radius);
//    }
//}

using UnityEngine;
using UnityEngine.InputSystem;

public class P_ElectricityArea : MonoBehaviour
{
    [Header("Power Up")]
    [SerializeField] private bool hasElectricPower = false;

    [Header("Configuración de energía")]
    public float maxEnergy = 10f;
    public float currentEnergy = 10f;
    public float baseConsumption = 0.5f;
    public float activeConsumption = 1f;
    public float radius = 3f;

    [Header("Aura visual")]
    public GameObject electricityAura;
    public AnimationCurve auraGrowth;
    private float auraTime;

    [Header("Controles")]
    public Key activateKey = Key.F;

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
        // 🔒 No puede usar electricidad si no tiene el power-up
        if (!hasElectricPower)
            return;

        // Activar al presionar
        if (Keyboard.current[activateKey].wasPressedThisFrame)
            StartElectricity();

        // Desactivar al soltar
        if (Keyboard.current[activateKey].wasReleasedThisFrame)
            StopElectricity();

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
    // ELECTRICIDAD
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

    private void UpdateAuraVisual()
    {
        if (electricityAura == null)
            return;

        if (auraGrowth != null && auraGrowth.length > 0)
        {
            auraTime += Time.deltaTime;
            float scaleFactor = auraGrowth.Evaluate(auraTime);
            electricityAura.transform.localScale = Vector3.one * radius * scaleFactor;
        }
        else
        {
            electricityAura.transform.localScale = Vector3.one * radius;
        }
    }

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
    // ENERGÍA
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
