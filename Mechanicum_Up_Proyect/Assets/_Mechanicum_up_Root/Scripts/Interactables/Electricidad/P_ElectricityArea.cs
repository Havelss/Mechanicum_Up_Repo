using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P_ElectricityArea : MonoBehaviour
{
    [Header("Configuración de energía")]
    public float maxEnergy = 10f;         // Energía máxima
    public float currentEnergy = 10f;     // Energía actual
    public float baseConsumption = 0.5f;  // Consumo por segundo al estar activo
    public float activeConsumption = 1f;  // Consumo extra al usar electricidad
    public float radius = 3f;             // Radio de efecto para objetos electrificables

    [Header("Controles")]
    public Key activateKey = Key.F;       // Activar/desactivar electricidad
    public Key interactKey = Key.E;       // Interactuar con terminales

    private bool isActive = false;
    private P_ElectricityPowerUp powerUp;

    private void Start()
    {
        powerUp = GetComponent<P_ElectricityPowerUp>();
    }

    private void Update()
    {
        // Activar o desactivar electricidad
        if (Keyboard.current[activateKey].wasPressedThisFrame)
            isActive = !isActive;

        if (isActive)
        {
            // Consumo de energía
            float consumption = baseConsumption + activeConsumption;
            currentEnergy -= consumption * Time.deltaTime;
            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                isActive = false;
                Debug.Log("Energía agotada!");
            }

            // Aplicar electricidad a objetos cercanos
            ApplyElectricity();
        }
        else
        {
            // Consumo pasivo
            currentEnergy -= baseConsumption * Time.deltaTime;
            if (currentEnergy < 0f)
                currentEnergy = 0f;
        }

        // Interactuar con terminales si se tiene el poder
        if (powerUp != null && Keyboard.current[interactKey].wasPressedThisFrame)
        {
            TryInteractWithTerminal();
        }
    }

    private void ApplyElectricity()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
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
            var terminal = hit.GetComponent<T_Electrifiable>();
            if (terminal != null)
            {
                if (terminal.IsPowered())
                {
                    powerUp.AbsorbEnergyFromTerminal(terminal, 2f); // cantidad que absorbe
                }
                else
                {
                    powerUp.TransferEnergyToTerminal(terminal, 2f); // cantidad que transfiere
                }
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
