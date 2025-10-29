using UnityEngine;

public class T_Recharge : MonoBehaviour, I_Electrifiable
{
    [Header("Configuración")]
    public float energyProvided = 50f;  // Cuánta energía da al jugador
    public bool isUsed = false;         // Una vez usada, no se puede usar otra vez

    public void ReceiveElectricity(float amount)
    {
        if (isUsed) return;

        // Aquí podrías activar el objeto electrificado (por ejemplo, encender la terminal)
        isUsed = true;

        // Notificar al jugador
        P_ElectricityArea player = FindFirstObjectByType<P_ElectricityArea>();
        if (player != null)
        {
            player.currentEnergy = Mathf.Min(player.currentEnergy + energyProvided, player.maxEnergy);
        }

        // Opcional: animación o efecto visual
        Debug.Log($"{name} ha sido recargada con electricidad.");
    }

    public bool IsElectrified()
    {
        return isUsed;
    }
}
