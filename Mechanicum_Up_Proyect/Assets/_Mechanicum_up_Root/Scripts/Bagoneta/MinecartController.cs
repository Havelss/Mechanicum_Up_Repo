using UnityEngine;

public class MinecartController : MonoBehaviour
{
    public enum CartDirection { None, Left, Right }

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Player")]
    public Transform playerSeat;
    public bool isPlayerInside = false;

    [Header("Terminal interna de la bagoneta")]
    public GameObject cartTerminalUI;

    [Header("Desmontaje")]
    public Vector3 cartSeatForwardOffset = new Vector3(0, 0, 2f);

    private Rigidbody rb;
    private CartDirection currentDirection = CartDirection.None;

    private static bool cartExists = false;

    private GameObject currentPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cartExists = true;
    }

    private void OnDestroy()
    {
        cartExists = false;
    }

    public static bool CanSpawnCart()
    {
        return !cartExists;
    }

    public void Command_NO() => currentDirection = CartDirection.Left;
    public void Command_LEFT() => currentDirection = CartDirection.Right;
    public void Command_Stop() => currentDirection = CartDirection.None;

    public void LockPlayerInput()
    {
        if (currentPlayer == null) return;

        var controller = currentPlayer.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false; // Esto bloquea todo el input de movimiento
        }
    }

    public void UnlockPlayerInput()
    {
        if (currentPlayer == null) return;

        var controller = currentPlayer.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = true; // Vuelve a habilitar el input
        }
    }


    // Llamada desde CartRiderTrigger después de mover al player al asiento
    public void FinalizeEnter(GameObject player)
    {
        if (player == null || playerSeat == null)
        {
            Debug.LogError("[MinecartController] Player o playerSeat no asignado!");
            return;
        }

        isPlayerInside = true;
        currentPlayer = player;

        // Bloquear Rigidbody y PlayerController
        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        LockPlayerInput(); // Bloquea el input de movimiento

        player.transform.SetParent(playerSeat);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
    }


    // Método para desmontar al jugador
    public void ExitCart()
    {
        if (!isPlayerInside || currentPlayer == null) return;

        isPlayerInside = false;

        var rb = currentPlayer.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        UnlockPlayerInput(); // Desbloquea el input al salir

        currentPlayer.transform.SetParent(null);
        currentPlayer.transform.position = playerSeat.position + cartSeatForwardOffset;

        if (cartTerminalUI != null)
            cartTerminalUI.SetActive(false);

        currentPlayer = null;
    }


    public void OpenInternalTerminal()
    {
        if (cartTerminalUI != null)
            cartTerminalUI.SetActive(true);
    }

    private void FixedUpdate()
    {
        Vector3 vel = rb.linearVelocity;

        if (currentDirection == CartDirection.Left)
            vel.x = -moveSpeed;
        else if (currentDirection == CartDirection.Right)
            vel.x = moveSpeed;
        else
            vel.x = 0;

        rb.linearVelocity = vel;
    }
}
