using UnityEngine;

public class P_ElectricityArea : MonoBehaviour
{
    [Header("Configuración")]
    public float electricityRadius = 3f;     // Radio de efecto
    public float electricityConsumption = 5f; // Cuánto gasta al activar
    public LayerMask electrifiableLayer;     // Layer de objetos que pueden recibir electricidad

    [HideInInspector] public float currentEnergy = 100f;  // Energía del jugador
    public float maxEnergy = 100f;
    public float rechargeRate = 10f;         // Recarga por segundo

    private void Update()
    {
        // Recargar energía pasiva
        if (currentEnergy < maxEnergy)
            currentEnergy += rechargeRate * Time.deltaTime;

        // Activar electricidad con tecla (por ejemplo, E)
        if (Input.GetKey(KeyCode.E) && currentEnergy > 0f)
        {
            currentEnergy -= electricityConsumption * Time.deltaTime;
            SendElectricity();
        }
    }

    private void SendElectricity()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, electricityRadius, electrifiableLayer);

        foreach (Collider col in hits)
        {
            I_Electrifiable electrifiable = col.GetComponent<I_Electrifiable>();
            if (electrifiable != null)
            {
                electrifiable.ReceiveElectricity(Time.deltaTime); // Cantidad de energía por frame
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, electricityRadius);
    }
}
