using UnityEngine;

public class MinecartController : MonoBehaviour
{
    public enum CartDirection { None, Left, Right }

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Player")]
    public Transform playerSeat;
    public bool isPlayerInside = false;

    private Rigidbody rb;
    private CartDirection currentDirection = CartDirection.None;

    private static bool cartExists = false; // solo una bagoneta activa

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cartExists = true;
    }

    private void OnDestroy()
    {
        cartExists = false;
    }

    /// ---------------------------------------------------------
    ///   MÉTODO QUE LLAMA LA TERMINAL  (NO,UP → Spawnear)
    /// ---------------------------------------------------------
    public static bool CanSpawnCart()
    {
        return !cartExists;
    }

    /// ---------------------------------------------------------
    ///     Control desde terminal dentro de la bagoneta
    /// ---------------------------------------------------------

    // Cuando la terminal hace "NO"
    public void Command_NO()
    {
        currentDirection = CartDirection.Left;
    }

    // Cuando la terminal hace "LEFT"
    public void Command_LEFT()
    {
        currentDirection = CartDirection.Right;
    }

    // Para parar si hace falta
    public void Command_Stop()
    {
        currentDirection = CartDirection.None;
    }

    /// ---------------------------------------------------------
    ///     Entrada del jugador a la bagoneta
    /// ---------------------------------------------------------
    public void EnterCart(GameObject player)
    {
        if (isPlayerInside) return;

        isPlayerInside = true;

        // Poner al jugador fijo a la bagoneta
        player.transform.SetParent(playerSeat);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        // Desactivar movimiento del PlayerController
        var controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;
    }

    public void ExitCart(GameObject player)
    {
        if (!isPlayerInside) return;

        isPlayerInside = false;

        player.transform.SetParent(null);

        var controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = true;
    }

    private void FixedUpdate()
    {
        if (currentDirection == CartDirection.Left)
        {
            rb.linearVelocity = new Vector3(-moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else if (currentDirection == CartDirection.Right)
        {
            rb.linearVelocity = new Vector3(moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }
}
