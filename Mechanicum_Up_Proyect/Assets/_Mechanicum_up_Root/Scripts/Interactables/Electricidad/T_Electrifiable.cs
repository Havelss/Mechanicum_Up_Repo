using UnityEngine;

// Implementa la interfaz para objetos que pueden recibir electricidad
public class T_Electrifiable : MonoBehaviour, I_Electrifiable
{
    [Header("Terminal apagada")]
    public SO_Terminal terminal;   // Terminal asociada (opcional)

    public bool isPowered = false; // estado interno de energía

    // =========================
    // ENCENDER EL OBJETO
    // =========================
    public void PowerOn()
{
    if (isPowered) return;
    isPowered = true;

    if (terminal != null)
    {
        terminal.SetElectrified(true); // 🔥 ENCENDIDA PARA SIEMPRE
        terminal.terminalCanvas.SetActive(false);
    }

    Debug.Log($"{name} ha sido electrificada permanentemente.");
}


    // =========================
    // APAGAR EL OBJETO (opcional)
    // =========================
    public void PowerOff()
    {
        // Aquí puedes implementar apagar si lo deseas
        // Por ahora, según tu diseño, no hace nada
    }

    // =========================
    // CHEQUEAR SI ESTÁ ENCENDIDO
    // =========================
    public bool IsPowered()
    {
        return isPowered;
    }

    // =========================
    // OPCIONAL: MÉTODO PARA RESETEAR (si quieres)
    // =========================
    public void ResetPower()
    {
        isPowered = false;
        if (terminal != null && terminal.terminalCanvas != null)
        {
            terminal.terminalCanvas.SetActive(false);
        }
        Debug.Log($"{name} ha sido reseteada y apagada.");
    }
}
