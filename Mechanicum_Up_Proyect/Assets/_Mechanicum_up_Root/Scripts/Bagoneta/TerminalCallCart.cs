//using UnityEngine;

//public class TerminalCallCart : MonoBehaviour
//{
//    public GameObject cartPrefab;
//    public Transform spawnPoint;

//    public void ExecuteCommand(string command)
//    {
//        command = command.ToLower().Trim();
//        if (command == "no,up")
//            TrySpawnCart();
//    }

//    void TrySpawnCart()
//    {
//        if (!MinecartController.CanSpawnCart())
//        {
//            Debug.Log("[TerminalCallCart] No se puede spawnear la bagoneta ahora.");
//            return;
//        }

//        // Instanciamos la bagoneta y la ajustamos sobre el suelo
//        GameObject cart = Instantiate(cartPrefab, spawnPoint.position, spawnPoint.rotation);
//        Rigidbody rb = cart.GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            Vector3 vel = rb.linearVelocity;
//            vel.y = -0.1f;  // Empuj�n inicial para que caiga
//            rb.linearVelocity = vel;
//        }

//        // Ajuste opcional de altura: raycast
//        RaycastHit hit;
//        if (Physics.Raycast(cart.transform.position + Vector3.up * 5f, Vector3.down, out hit, 50f, LayerMask.GetMask("Ground")))
//        {
//            cart.transform.position = hit.point + Vector3.up * 0.1f;
//        }
//    }
//}

using UnityEngine;

public class TerminalCallCart : MonoBehaviour
{
    public GameObject cartPrefab;
    public Transform spawnPoint;

    public void ExecuteCommand(string command)
    {
        command = command.ToLower().Trim();
        if (command == "no,up")
            TrySpawnCart();
    }

    void TrySpawnCart()
    {
        // Verificar si ya existe otra bagoneta en la escena
        if (FindObjectsOfType<MinecartController>().Length > 0)
        {
            Debug.Log("[TerminalCallCart] No se puede spawnear la bagoneta ahora.");
            return;
        }

        GameObject cartObj = Instantiate(cartPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = cartObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 vel = rb.linearVelocity;
            vel.y = -0.1f;
            rb.linearVelocity = vel;
        }

        // Ajuste opcional de altura con raycast
        RaycastHit hit;
        if (Physics.Raycast(cartObj.transform.position + Vector3.up * 5f, Vector3.down, out hit, 50f, LayerMask.GetMask("Ground")))
        {
            cartObj.transform.position = hit.point + Vector3.up * 0.1f;
        }

        // Asignar la terminal en la escena al cart
        MinecartController cart = cartObj.GetComponent<MinecartController>();
        MinecartTerminalUI terminalUI = GameObject.FindWithTag("CartTerminal")?.GetComponent<MinecartTerminalUI>();
        if (terminalUI != null)
        {
            cart.cartTerminalUI = terminalUI;   // Referencia al script de UI
            terminalUI.SetCart(cart);           // Asigna la bagoneta al UI
        }
        else
        {
            Debug.LogWarning("No se encontró la terminal en la escena");
        }
    }
}


