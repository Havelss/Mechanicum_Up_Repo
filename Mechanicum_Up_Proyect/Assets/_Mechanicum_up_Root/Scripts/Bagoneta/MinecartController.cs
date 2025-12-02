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
    private GameObject currentPlayer;

    private static bool cartExists = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cartExists = true;

        // Crear playerSeat automático si no está asignado
        if (playerSeat == null)
        {
            GameObject seatGO = new GameObject("PlayerSeat");
            seatGO.transform.SetParent(transform);
            seatGO.transform.localPosition = Vector3.zero;
            seatGO.transform.localRotation = Quaternion.identity;
            playerSeat = seatGO.transform;

            Debug.LogWarning("[MinecartController] playerSeat no asignado, se creó automáticamente un empty GameObject.");
        }
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

    public void FinalizeEnter(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("[MinecartController] Player es null en FinalizeEnter!");
            return;
        }

        if (playerSeat == null)
        {
            Debug.LogError("[MinecartController] playerSeat no asignado!");
            return;
        }

        isPlayerInside = true;
        currentPlayer = player;

        // Desactivar Rigidbody y bloquear input completo
        var rbPlayer = player.GetComponent<Rigidbody>();
        if (rbPlayer != null)
        {
            rbPlayer.linearVelocity = Vector3.zero;
            rbPlayer.angularVelocity = Vector3.zero;
            rbPlayer.isKinematic = true;
        }

        LockPlayerInput();

        // Teletransportar al asiento
        player.transform.SetParent(playerSeat);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
    }

    public void ExitCart()
    {
        if (!isPlayerInside || currentPlayer == null) return;

        isPlayerInside = false;

        // Reactivar Rigidbody y PlayerController
        var rbPlayer = currentPlayer.GetComponent<Rigidbody>();
        if (rbPlayer != null)
        {
            rbPlayer.isKinematic = false;
        }

        UnlockPlayerInput();

        // Separar del carrito y colocar delante
        currentPlayer.transform.SetParent(null);
        currentPlayer.transform.position = playerSeat.position + cartSeatForwardOffset;

        // Cerrar terminal
        if (cartTerminalUI != null)
            cartTerminalUI.SetActive(false);

        currentPlayer = null;
    }

    // Bloquea todo el input (caminar, correr, saltar)
    public void LockPlayerInput()
    {
        if (currentPlayer == null) return;
        var controller = currentPlayer.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;
    }

    // Desbloquea todo el input
    public void UnlockPlayerInput()
    {
        if (currentPlayer == null) return;
        var controller = currentPlayer.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = true;
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
