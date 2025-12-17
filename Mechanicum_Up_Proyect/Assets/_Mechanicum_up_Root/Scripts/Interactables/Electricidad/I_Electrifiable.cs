using UnityEngine;

// Interfaz para cualquier objeto que pueda recibir electricidad
public interface I_Electrifiable
{
    // Se llama para activar el objeto con electricidad
    void PowerOn();

    // Opcional: desactivar el objeto (si quieres implementar apagado)
    void PowerOff();
}
