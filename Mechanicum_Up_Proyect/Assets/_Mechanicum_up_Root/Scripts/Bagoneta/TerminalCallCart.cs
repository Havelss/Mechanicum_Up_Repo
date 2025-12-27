//using UnityEngine;

//public class TerminalCallCart : MonoBehaviour
//{
//    public GameObject cartPrefab;
//    public Transform spawnPoint;

//    public void ExecuteCommand(string command)
//    {
//        if (command.ToLower().Trim() == "no,up")
//            TrySpawnCart();
//    }

//    void TrySpawnCart()
//    {
//        if (FindObjectsOfType<MinecartController>().Length > 0)
//        {
//            Debug.Log("[TerminalCallCart] No se puede spawnear la bagoneta ahora.");
//            return;
//        }

//        GameObject cartObj = Instantiate(cartPrefab, spawnPoint.position, spawnPoint.rotation);

//        // Ajustar Rigidbody
//        Rigidbody rb = cartObj.GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            Vector3 vel = rb.linearVelocity;
//            vel.y = -0.1f;
//            rb.linearVelocity = vel;
//        }

//        // Ajuste de altura con raycast
//        RaycastHit hit;
//        if (Physics.Raycast(cartObj.transform.position + Vector3.up * 5f, Vector3.down, out hit, 50f, LayerMask.GetMask("Ground")))
//        {
//            cartObj.transform.position = hit.point + Vector3.up * 0.1f;
//        }

//        // Vincular terminal UI y Animator
//        MinecartController cart = cartObj.GetComponent<MinecartController>();
//        GameObject terminalObj = GameObject.FindWithTag("CartTerminal");
//        if (terminalObj != null)
//        {
//            cart.cartTerminalUI = terminalObj.GetComponentInChildren<MinecartTerminalUI>();
//            cart.terminalAnimator = terminalObj.GetComponentInChildren<MinecartTerminalAnimator>();

//            cart.cartTerminalUI?.SetCart(cart);
//        }
//        else
//        {
//            Debug.LogWarning("[TerminalCallCart] No se encontró terminal con tag 'CartTerminal'");
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
        if (command.ToLower().Trim() == "no,up")
            TrySpawnCart();
    }

    void TrySpawnCart()
    {
        if (FindObjectsOfType<MinecartController>().Length > 0)
        {
            Debug.Log("[TerminalCallCart] No se puede spawnear la bagoneta ahora.");
            return;
        }

        GameObject cartObj = Instantiate(cartPrefab, spawnPoint.position, spawnPoint.rotation);

        //Rigidbody rb = cartObj.GetComponent<Rigidbody>();

        //if (rb != null && rb.useGravity)
        //{
        //    Vector3 vel = rb.linearVelocity;
        //    vel.y = -0.1f;
        //    rb.linearVelocity = vel;
        //}

        Rigidbody rb = cartObj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.WakeUp(); // 🔥 CLAVE
        }


        // Ajuste de altura CORRECTO con Rigidbody
        RaycastHit hit;
        if (rb != null && Physics.Raycast(
            cartObj.transform.position + Vector3.up * 5f,
            Vector3.down,
            out hit,
            50f,
            LayerMask.GetMask("Ground")))
        {
            rb.position = hit.point + Vector3.up * 0.1f;
        }

        // Vincular terminal UI y Animator
        MinecartController cart = cartObj.GetComponent<MinecartController>();
        GameObject terminalObj = GameObject.FindWithTag("CartTerminal");

        if (terminalObj != null)
        {
            cart.cartTerminalUI = terminalObj.GetComponentInChildren<MinecartTerminalUI>();
            cart.terminalAnimator = terminalObj.GetComponentInChildren<MinecartTerminalAnimator>();
            cart.cartTerminalUI?.SetCart(cart);
        }
        else
        {
            Debug.LogWarning("[TerminalCallCart] No se encontró terminal con tag 'CartTerminal'");
        }
    }
}