using UnityEngine;

public class P_ElectricityPowerUp : MonoBehaviour
{
    private bool hasElectricPower = false;
    private P_ElectricityArea playerElectricity;

    private void Start()
    {
        playerElectricity = GetComponent<P_ElectricityArea>();
    }

    public void ActivatePower()
    {
        hasElectricPower = true;
        Debug.Log("El jugador ha obtenido el poder de electricidad!");
    }

    public bool HasPower()
    {
        return hasElectricPower;
    }

    // Absorber energía de una terminal encendida (sin agotarla)
    public void AbsorbEnergyFromTerminal(T_Electrifiable terminal, float amount)
    {
        if (!hasElectricPower || terminal == null)
            return;

        if (terminal.IsPowered())
        {
            playerElectricity.RechargeEnergy(amount);
            Debug.Log($"Energía absorbida desde {terminal.name}");
        }
    }

    // Transferir energía a una terminal sin energía
    public void TransferEnergyToTerminal(T_Electrifiable terminal, float amount)
    {
        if (!hasElectricPower || terminal == null)
            return;

        if (!terminal.IsPowered() && playerElectricity.currentEnergy >= amount)
        {
            terminal.PowerOn();
            playerElectricity.currentEnergy -= amount;
            Debug.Log($"Energía transferida a {terminal.name}");
        }
    }
}
