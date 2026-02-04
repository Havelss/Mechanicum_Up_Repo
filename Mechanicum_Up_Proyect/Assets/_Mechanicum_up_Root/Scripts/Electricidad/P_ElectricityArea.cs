//using UnityEngine;
//using UnityEngine.InputSystem;

//public class P_ElectricityArea : MonoBehaviour
//{
//    [Header("Power Up")]
//    private bool hasElectricPower = false; // 🔴 NO serializado

//    [Header("Configuración de energía")]
//    public float maxEnergy = 10f;
//    public float currentEnergy = 10f;
//    public float baseConsumption = 0.5f;
//    public float activeConsumption = 1f;

//    [Header("Absorción de energía")]
//    public float absorbRate = 3f;

//    [Header("Aura visual")]
//    public GameObject electricityAura;
//    public AnimationCurve auraGrowth;
//    private float auraTime;

//    [Header("Radio del aura")]
//    public Vector3 auraRadius = new Vector3(3f, 3f, 3f);

//    [Header("Controles")]
//    public Key usePowerKey = Key.Q;
//    public Key absorbKey = Key.F;

//    public float radius = 3f;

//    private bool isActive = false;

//    private void Start()
//    {
//        // 🔄 RESET SEGURO AL SPAWN
//        hasElectricPower = false;
//        isActive = false;
//        currentEnergy = maxEnergy;

//        if (electricityAura != null)
//        {
//            electricityAura.SetActive(false);
//            electricityAura.transform.localScale = Vector3.zero;
//        }

//        // 🔁 REAPLICAR DESDE INVENTARIO
//        if (PlayerInventory.Instance != null && PlayerInventory.Instance.hasElectricPower)
//        {
//            ActivatePower();
//        }
//    }

//    private void Update()
//    {
//        if (!hasElectricPower)
//            return;

//        if (Keyboard.current[usePowerKey].wasPressedThisFrame)
//            StartElectricity();

//        if (Keyboard.current[usePowerKey].wasReleasedThisFrame)
//            StopElectricity();

//        if (isActive)
//        {
//            float consumption = baseConsumption + activeConsumption;
//            currentEnergy -= consumption * Time.deltaTime;

//            if (currentEnergy <= 0f)
//            {
//                currentEnergy = 0f;
//                StopElectricity();
//                return;
//            }

//            ApplyElectricity();
//            UpdateAuraVisual();
//        }
//        else
//        {
//            currentEnergy -= baseConsumption * Time.deltaTime;
//            if (currentEnergy < 0f)
//                currentEnergy = 0f;
//        }

//        if (Keyboard.current[absorbKey].isPressed && !isActive)
//        {
//            AbsorbEnergyFromTerminal();
//        }
//    }

//    // =========================
//    // POWER UP
//    // =========================
//    public void ActivatePower()
//    {
//        hasElectricPower = true;
//        Debug.Log("⚡ Poder eléctrico activado");
//    }

//    public bool HasPower() => hasElectricPower;

//    // =========================
//    // ELECTRICIDAD ACTIVA
//    // =========================
//    private void StartElectricity()
//    {
//        if (currentEnergy <= 0f || isActive)
//            return;

//        isActive = true;
//        auraTime = 0f;

//        if (electricityAura != null)
//        {
//            electricityAura.SetActive(true);
//            electricityAura.transform.localScale = Vector3.zero;
//        }
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
//    }

//    private void UpdateAuraVisual()
//    {
//        if (electricityAura == null || auraGrowth == null || auraGrowth.length == 0)
//        {
//            if (electricityAura != null)
//                electricityAura.transform.localScale = auraRadius;
//            return;
//        }

//        auraTime += Time.deltaTime;
//        float duration = auraGrowth[auraGrowth.length - 1].time;
//        float t = Mathf.Clamp(auraTime, 0, duration);
//        float scaleFactor = auraGrowth.Evaluate(t);

//        electricityAura.transform.localScale = new Vector3(
//            auraRadius.x * scaleFactor,
//            auraRadius.y * scaleFactor,
//            auraRadius.z * scaleFactor
//        );
//    }

//    private void ApplyElectricity()
//    {
//        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
//        foreach (var hit in hits)
//        {
//            if (hit.CompareTag("Electrifiable"))
//            {
//                I_Electrifiable e = hit.GetComponent<I_Electrifiable>();
//                if (e != null)
//                    e.PowerOn();
//            }
//        }
//    }

//    private void AbsorbEnergyFromTerminal()
//    {
//        if (currentEnergy >= maxEnergy)
//            return;

//        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
//        foreach (var hit in hits)
//        {
//            if (!hit.CompareTag("Electrifiable"))
//                continue;

//            T_Electrifiable terminal = hit.GetComponent<T_Electrifiable>();
//            if (terminal != null && terminal.IsPowered())
//            {
//                currentEnergy += absorbRate * Time.deltaTime;
//                if (currentEnergy > maxEnergy)
//                    currentEnergy = maxEnergy;
//                return;
//            }
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, radius);
//    }

//    // =========================
//    // RECARGAR ENERGÍA MANUAL
//    // =========================
//    public void RechargeEnergy(float amount)
//    {
//        if (!hasElectricPower)
//            return;

//        currentEnergy += amount;

//        if (currentEnergy > maxEnergy)
//            currentEnergy = maxEnergy;

//        Debug.Log($"🔋 Energía recargada a {currentEnergy}/{maxEnergy}");
//    }
//}
using UnityEngine;
using UnityEngine.InputSystem;

public class P_ElectricityArea : MonoBehaviour
{
    [Header("Power Up")]
    private bool hasElectricPower = false; // 🔴 NO serializado

    [Header("Configuración de energía")]
    public float maxEnergy = 10f;
    public float currentEnergy = 10f;
    public float baseConsumption = 0.5f;
    public float activeConsumption = 1f;

    [Header("Absorción de energía")]
    public float absorbRate = 3f;

    [Header("Feedback Visual (Prefab)")]
    [Tooltip("Nombre exacto del objeto visual que se instancia (ej: ElectricPowerVisual(Clone))")]
    [SerializeField] private string visualObjectName = "ElectricPowerVisual(Clone)";
    [SerializeField][ColorUsage(true, true)] private Color energyColor = Color.cyan;
    [SerializeField] private float maxEmissionIntensity = 2f;
    private Material dynamicMaterial;

    [Header("Aura visual")]
    public GameObject electricityAura;
    public AnimationCurve auraGrowth;
    private float auraTime;

    [Header("Radio del aura")]
    public Vector3 auraRadius = new Vector3(3f, 3f, 3f);

    [Header("Controles")]
    public Key usePowerKey = Key.Q;
    public Key absorbKey = Key.F;

    public float radius = 3f;

    private bool isActive = false;

    private void Start()
    {
        // 🔄 RESET SEGURO AL SPAWN
        hasElectricPower = false;
        isActive = false;
        currentEnergy = maxEnergy;

        if (electricityAura != null)
        {
            electricityAura.SetActive(false);
            electricityAura.transform.localScale = Vector3.zero;
        }

        // 🔁 REAPLICAR DESDE INVENTARIO
        if (PlayerInventory.Instance != null && PlayerInventory.Instance.hasElectricPower)
        {
            ActivatePower();
        }
    }

    private void Update()
    {
        if (!hasElectricPower)
            return;

        // 🎨 BUSCAR EL MATERIAL SI AÚN NO LO TENEMOS (Para prefabs instanciados después)
        if (dynamicMaterial == null)
        {
            TryGetDynamicMaterial();
        }

        if (Keyboard.current[usePowerKey].wasPressedThisFrame)
            StartElectricity();

        if (Keyboard.current[usePowerKey].wasReleasedThisFrame)
            StopElectricity();

        if (isActive)
        {
            float consumption = baseConsumption + activeConsumption;
            currentEnergy -= consumption * Time.deltaTime;

            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                StopElectricity();
                UpdateEnergyVisual(); // Actualizar a 0 al apagar
                return;
            }

            ApplyElectricity();
            UpdateAuraVisual();
        }
        else
        {
            currentEnergy -= baseConsumption * Time.deltaTime;
            if (currentEnergy < 0f)
                currentEnergy = 0f;
        }

        if (Keyboard.current[absorbKey].isPressed && !isActive)
        {
            AbsorbEnergyFromTerminal();
        }

        // Actualizar el brillo de la textura en cada frame
        UpdateEnergyVisual();
    }

    // =========================
    // LÓGICA DE TEXTURA EMISIVA
    // =========================
    private void TryGetDynamicMaterial()
    {
        // Buscamos en los hijos del objeto donde está este script
        Transform visualTransform = transform.Find(visualObjectName);

        // Si no es hijo directo, buscamos en toda la jerarquía del objeto
        if (visualTransform == null)
        {
            Renderer[] childrenRenderers = GetComponentsInChildren<Renderer>(true);
            foreach (var r in childrenRenderers)
            {
                if (r.gameObject.name == visualObjectName)
                {
                    energyRenderer = r;
                    dynamicMaterial = r.material;
                    dynamicMaterial.EnableKeyword("_EMISSION");
                    return;
                }
            }
        }
        else
        {
            Renderer r = visualTransform.GetComponent<Renderer>();
            if (r != null)
            {
                dynamicMaterial = r.material;
                dynamicMaterial.EnableKeyword("_EMISSION");
            }
        }
    }

    private Renderer energyRenderer; // Auxiliar para la búsqueda

    private void UpdateEnergyVisual()
    {
        if (dynamicMaterial == null) return;

        float energyPercent = currentEnergy / maxEnergy;

        // La intensidad baja gradualmente con la energía
        float intensity = energyPercent * maxEmissionIntensity;

        // Aplicar color e intensidad HDR
        Color finalColor = energyColor * intensity;
        dynamicMaterial.SetColor("_EmissionColor", finalColor);
    }

    // =========================
    // POWER UP
    // =========================
    public void ActivatePower()
    {
        hasElectricPower = true;
        Debug.Log("⚡ Poder eléctrico activado");
    }

    public bool HasPower() => hasElectricPower;

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
    }

    private void UpdateAuraVisual()
    {
        if (electricityAura == null || auraGrowth == null || auraGrowth.length == 0)
        {
            if (electricityAura != null)
                electricityAura.transform.localScale = auraRadius;
            return;
        }

        auraTime += Time.deltaTime;
        float duration = auraGrowth[auraGrowth.length - 1].time;
        float t = Mathf.Clamp(auraTime, 0, duration);
        float scaleFactor = auraGrowth.Evaluate(t);

        electricityAura.transform.localScale = new Vector3(
            auraRadius.x * scaleFactor,
            auraRadius.y * scaleFactor,
            auraRadius.z * scaleFactor
        );
    }

    private void ApplyElectricity()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Electrifiable"))
            {
                I_Electrifiable e = hit.GetComponent<I_Electrifiable>();
                if (e != null)
                    e.PowerOn();
            }
        }
    }

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
                currentEnergy += absorbRate * Time.deltaTime;
                if (currentEnergy > maxEnergy)
                    currentEnergy = maxEnergy;
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // =========================
    // RECARGAR ENERGÍA MANUAL
    // =========================
    public void RechargeEnergy(float amount)
    {
        if (!hasElectricPower)
            return;

        currentEnergy += amount;

        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        Debug.Log($"🔋 Energía recargada a {currentEnergy}/{maxEnergy}");
    }
}