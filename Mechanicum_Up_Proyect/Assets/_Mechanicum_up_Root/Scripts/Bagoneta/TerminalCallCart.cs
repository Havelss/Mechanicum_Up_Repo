using UnityEngine;

public class TerminalCallCart : MonoBehaviour
{
    public GameObject cartPrefab;
    public Transform spawnPoint;

    public void ExecuteCommand(string command)
    {
        Debug.Log($"[TerminalCallCart] ExecuteCommand recibido: '{command}'");

        // Normalizamos el texto
        command = command.ToLower().Trim();

        // Comando esperado
        if (command == "no,up")
        {
            Debug.Log("[TerminalCallCart] Comando correcto -> TrySpawnCart()");
            TrySpawnCart();
        }
        else
        {
            Debug.Log("[TerminalCallCart] Comando incorrecto o desconocido");
        }
    }

    void TrySpawnCart()
    {
        if (!MinecartController.CanSpawnCart())
        {
            Debug.Log("[TerminalCallCart] No se puede spawnear la bagoneta ahora.");
            return;
        }

        Debug.Log("[TerminalCallCart] Instanciando bagoneta...");
        Instantiate(cartPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
