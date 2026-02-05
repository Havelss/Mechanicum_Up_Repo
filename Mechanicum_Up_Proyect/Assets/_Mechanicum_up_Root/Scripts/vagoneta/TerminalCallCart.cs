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



//        Rigidbody rb = cartObj.GetComponent<Rigidbody>();

//        if (rb != null)
//        {
//            rb.useGravity = true;
//            rb.isKinematic = false;
//            rb.WakeUp(); // 🔥 CLAVE
//        }


//        // Ajuste de altura CORRECTO con Rigidbody
//        RaycastHit hit;
//        if (rb != null && Physics.Raycast(
//            cartObj.transform.position + Vector3.up * 5f,
//            Vector3.down,
//            out hit,
//            50f,
//            LayerMask.GetMask("Ground")))
//        {
//            rb.position = hit.point + Vector3.up * 0.1f;
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

using System.Collections;
using UnityEngine;

public class TerminalCallCart : MonoBehaviour
{
    public GameObject cartPrefab;
    public Transform spawnPoint;

    [Header("Spawn on Start")]
    [Tooltip("Si está activado, se intentará spawnear la vagoneta al inicio del juego.")]
    public bool spawnOnStart = false;
    [Tooltip("Retraso en segundos antes de spawnear al inicio (usar 0 para inmediato).")]
    public float spawnDelay = 0f;

    private void Start()
    {
        if (spawnOnStart)
        {
            if (spawnDelay > 0f)
                StartCoroutine(SpawnDelayed(spawnDelay));
            else
                RespawnAtStart();
        }
    }

    // Función pública solicitada: se puede llamar desde otros scripts para spawnear al inicio.
    public void RespawnAtStart()
    {
        TrySpawnCart();
    }

    public void ExecuteCommand(string command)
    {
        if (command.ToLower().Trim() == "no,up")
            TrySpawnCart();
    }

    void TrySpawnCart()
    {
        if (FindObjectsOfType<MinecartController>().Length > 0) return;

        // 1. Instancia en el lugar EXACTO del Empty
        GameObject cartObj = Instantiate(cartPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = cartObj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.WakeUp();
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

    private IEnumerator SpawnDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        TrySpawnCart();
    }
}