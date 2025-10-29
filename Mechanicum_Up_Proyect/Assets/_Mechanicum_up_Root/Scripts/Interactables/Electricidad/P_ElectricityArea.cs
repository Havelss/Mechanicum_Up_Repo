using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class P_ElectricityArea : MonoBehaviour
{
    [Header("Electricidad")]
    public float maxCharge = 10f;         // Duración máxima de la carga
    public float currentCharge = 0f;
    public float rechargeRate = 2f;       // Velocidad de recarga
    public bool hasElectricity => currentCharge > 0f;

    [Header("Área de efecto")]
    public float pulseRadius = 5f;        // Radio de energía
    public LayerMask electrifiableLayer;  // Capa de objetos que pueden recibir energía
    public float pulseCooldown = 2f;      // Tiempo entre pulsos automáticos

    private bool isRecharging = false;
    private bool canPulse = true;

    private void Update()
    {
        if (hasElectricity)
        {
            currentCharge -= Time.deltaTime;
            if (currentCharge <= 0f)
                currentCharge = 0f;

            if (canPulse)
                StartCoroutine(ElectricPulse());
        }
    }

    public void Recharge()
    {
        if (isRecharging) return;
        StartCoroutine(RechargeCoroutine());
    }

    private IEnumerator RechargeCoroutine()
    {
        isRecharging = true;
        while (currentCharge < maxCharge)
        {
            currentCharge += rechargeRate * Time.deltaTime;
            yield return null;
        }
        currentCharge = maxCharge;
        isRecharging = false;
    }

    private IEnumerator ElectricPulse()
    {
        canPulse = false;

        // Buscar todos los colliders cercanos en el radio definido
        Collider[] hits = Physics.OverlapSphere(transform.position, pulseRadius, electrifiableLayer);

        foreach (Collider hit in hits)
        {
            I_Electrifiable electrifiable = hit.GetComponent<I_Electrifiable>();
            if (electrifiable != null && !electrifiable.IsPowered)
            {
                electrifiable.PowerOn();
                Debug.Log($"⚡ {hit.name} energizado por el pulso eléctrico.");
            }
        }

        yield return new WaitForSeconds(pulseCooldown);
        canPulse = true;
    }
}
