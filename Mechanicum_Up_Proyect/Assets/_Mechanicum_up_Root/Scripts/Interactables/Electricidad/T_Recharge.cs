using UnityEngine;

public class T_Recharge : MonoBehaviour
{
    [Header("Configuración")]
    public float rechargeAmount = 1f;      // Energía que da al jugador
    public bool isUsed = false;            // Si ya fue usada, no se puede volver a descargar

    private void OnTriggerEnter(Collider other)
    {
        P_ElectricityArea playerEnergy = other.GetComponent<P_ElectricityArea>();
        if (playerEnergy != null && !isUsed)
        {
            playerEnergy.RechargeEnergy(rechargeAmount);
            isUsed = true;
            Debug.Log($"{name} ha recargado al jugador.");
        }
    }
}
