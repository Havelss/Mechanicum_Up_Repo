using UnityEngine;

public class T_Electrifiable : MonoBehaviour
{
    [Header("Terminal apagada")]
    public SO_Terminal terminal;

    private bool isPowered = false;

    public void PowerOn()
    {
        if (isPowered) return;  // Una vez encendida no se apaga

        isPowered = true;
        if (terminal != null)
        {
            terminal.terminalCanvas.SetActive(true); // O simplemente permitir que funcione
            Debug.Log($"{name} ha recibido energía y ahora funciona.");
        }
    }

    public void PowerOff()
    {
        // Opcional: si quieres, pero según tu diseño, una vez encendida no se apaga
    }
}
