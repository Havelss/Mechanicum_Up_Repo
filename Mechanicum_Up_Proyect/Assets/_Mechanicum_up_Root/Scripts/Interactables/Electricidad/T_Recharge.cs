using UnityEngine;

public class T_Recharge : MonoBehaviour, I_Electrifiable
{
    public bool IsPowered { get; private set; } = true; // siempre encendida

    private void OnTriggerEnter(Collider other)
    {
        var playerElec = other.GetComponent<P_ElectricityArea>();
        if (playerElec != null && IsPowered)
        {
            playerElec.Recharge();
            Debug.Log("⚡ Energía recargada desde terminal.");
        }
    }

    public void PowerOn()
    {
        IsPowered = true;
    }
}
