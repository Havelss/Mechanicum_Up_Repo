using UnityEngine;

public class TerminalCallCart : MonoBehaviour
{
    public GameObject cartPrefab;
    public Transform spawnPoint;

    public void OnTerminalCommand(string[] symbols)
    {
        // Comando exacto que quieres: NO, UP
        if (symbols.Length == 2 &&
            symbols[0] == "NO" &&
            symbols[1] == "UP")
        {
            TrySpawnCart();
        }
    }

    void TrySpawnCart()
    {
        if (!MinecartController.CanSpawnCart())
            return;

        Instantiate(cartPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
