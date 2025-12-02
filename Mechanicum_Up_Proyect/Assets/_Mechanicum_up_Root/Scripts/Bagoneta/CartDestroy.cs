using UnityEngine;

public class CartDestroy : MonoBehaviour
{
    [Header("Límite de destrucción")]
    public float destroyYLimit = -10f;

    private MinecartController cart;

    private void Awake()
    {
        cart = GetComponent<MinecartController>();
    }

    private void Update()
    {
        if (transform.position.y < destroyYLimit)
        {
            SafeDestroyCart();
        }
    }

    private void SafeDestroyCart()
    {
        // 1. Si hay jugador dentro, desmontarlo y desbloquear input
        if (cart != null && cart.isPlayerInside)
        {
            cart.ExitCart();
        }

        // 2. Cerrar la terminal si está abierta
        if (cart != null && cart.cartTerminalUI != null)
        {
            cart.cartTerminalUI.SetActive(false);
        }

        // 3. Destruir la bagoneta
        Destroy(gameObject);

        Debug.Log("[CartDestroy] La bagoneta fue destruida de manera segura al pasar el límite del mapa.");
    }
}
