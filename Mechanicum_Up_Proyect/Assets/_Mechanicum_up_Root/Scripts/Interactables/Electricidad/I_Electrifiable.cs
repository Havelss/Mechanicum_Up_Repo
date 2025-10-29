using UnityEngine;

public interface I_Electrifiable
{
    // Método que activa el objeto con electricidad
    void ReceiveElectricity(float amount);

    // Método para consultar si está activo
    bool IsElectrified();
}
