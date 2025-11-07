using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P_ElectricityArea : MonoBehaviour
{
    [Header("Configuración de energía")]
    public float maxEnergy = 10f;
    public float currentEnergy = 10f;
    public float baseConsumption = 0.5f;
    public float activeConsumption = 1f;
    public float radius = 3f;

    [Header("Aura visual")]
    public GameObject auraVisual; // asigna aquí un objeto visual (por ejemplo, una esfera transparente con material brillante)
    public float auraExpandSpeed = 5f; // qué tan rápido aparece/desaparece el aura

    [Header("Controles")]
    public Key activateKey = Key.F;
    public Key interactKey = Key.E;

    private bool isActive = false;
    private P_ElectricityPowerUp powerUp;
    private Vector3 targetScale;

    private void Start()
    {
        powerUp = GetComponent<P_ElectricityPowerUp>();

        if (auraVisual != null)
        {
            auraVisual.SetActive(false);
            auraVisual.transform.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        // Activar o desactivar electricidad (aura)
        if (Keyboard.current[activateKey].wasPressedThisFrame)
        {
            isActive = !isActive;
            if (auraVisual != null)
                auraVisual.SetActive(isActive);
        }

        // Ajustar tamaño del aura visual suavemente
        if (auraVisual != null)
        {
            targetScale = isActive ? Vector3.one * (radius * 2f) : Vector3.zero;
            auraVisual.transform.localScale = Vector3.Lerp(auraVisual.transform.localScale, targetScale, Time.deltaTime * auraExpandSpeed);
        }

        // Manejar consumo y efectos
        if (isActive)
        {
            float consumption = baseConsumption + activeConsumption;
            currentEnergy -= consumption * Time.deltaTime;

            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                isActive = false;
                if (auraVisual != null) auraVisual.SetActive(false);
                Debug.Log("Energía agotada!");
                return;
            }

            // Aplicar electricidad a objetos cercanos con tag "Electrifiable"
            ApplyElectricityAura();
        }
        else
        {
            currentEnergy -= baseConsumption * Time.deltaTime;
            if (currentEnergy < 0f)
                currentEnergy = 0f;
        }

        // Interacción con terminales (solo si tiene power-up)
        if (powerUp != null && Keyboard.current[interactKey].wasPressedThisFrame)
        {
            TryInteractWithTerminal();
        }
    }

    private void ApplyElectricityAura()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Electrifiable")) continue; // solo afecta objetos con el tag

            I_Electrifiable electrifiable = hit.GetComponent<I_Electrifiable>();
            if (electrifiable != null)
            {
                electrifiable.PowerOn();
            }
        }
    }

    private void TryInteractWithTerminal()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Electrifiable")) continue;

            var terminal = hit.GetComponent<T_Electrifiable>();
            if (terminal != null)
            {
                if (terminal.IsPowered())
                    powerUp.AbsorbEnergyFromTerminal(terminal, 2f);
                else
                    powerUp.TransferEnergyToTerminal(terminal, 2f);

                break;
            }
        }
    }

    public void RechargeEnergy(float amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        Debug.Log($"Energía recargada a {currentEnergy}/{maxEnergy}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
