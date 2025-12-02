using UnityEngine;

public class MinecartController : MonoBehaviour
{
    public enum CartDirection { None, Left, Right }

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Player")]
    public Transform playerSeat;
    public bool isPlayerInside = false;

    [Header("Terminal interna")]
    public GameObject cartTerminalUI;

    [Header("Desmontaje")]
    public Vector3 cartSeatForwardOffset = new Vector3(0, 0, 2f);

    // Preparado para plataformas móviles
    private Transform currentPlatform;
    private Vector3 currentPlatformLastPos;

    private Rigidbody rb;
    private CartDirection currentDirection = CartDirection.None;
    private GameObject currentPlayer;
    private int originalPlayerLayer = 0;

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
        }

        // Rigidbody estable
        rb.mass = 100f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnDestroy() => cartExists = false;

    public static bool CanSpawnCart() => !cartExists;

    public void Command_NO() => currentDirection = CartDirection.Left;
    public void Command_LEFT() => currentDirection = CartDirection.Right;
    public void Command_Stop() => currentDirection = CartDirection.None;

    public void FinalizeEnter(GameObject player)
    {
        if (player == null || playerSeat == null) return;

        isPlayerInside = true;
        currentPlayer = player;

        originalPlayerLayer = player.layer;

        // Rigidbody del jugador
        var rbPlayer = player.GetComponent<Rigidbody>();
        if (rbPlayer != null)
        {
            rbPlayer.linearVelocity = Vector3.zero;
            rbPlayer.angularVelocity = Vector3.zero;
            rbPlayer.isKinematic = true; // Bloquea física
        }

        // Collider del jugador
        var coll = player.GetComponent<Collider>();
        if (coll != null) coll.enabled = false;

        // Layer seguro
        int cartLayer = LayerMask.NameToLayer("PlayerInCart");
        if (cartLayer >= 0) player.layer = cartLayer;

        // Bloquear PlayerController (input, salto, groundcheck)
        var controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        // Root motion
        var anim = player.GetComponentInChildren<Animator>();
        if (anim != null) anim.applyRootMotion = false;

        // Teletransportar al asiento
        player.transform.SetParent(playerSeat);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
    }

    public void ExitCart()
    {
        if (!isPlayerInside || currentPlayer == null) return;

        isPlayerInside = false;

        // Rigidbody y collider
        var rbPlayer = currentPlayer.GetComponent<Rigidbody>();
        if (rbPlayer != null) rbPlayer.isKinematic = false;

        var coll = currentPlayer.GetComponent<Collider>();
        if (coll != null) coll.enabled = true;

        // Layer original
        currentPlayer.layer = originalPlayerLayer;

        // Reactivar PlayerController
        var controller = currentPlayer.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = true;

        // Root motion
        var anim = currentPlayer.GetComponentInChildren<Animator>();
        if (anim != null) anim.applyRootMotion = true;

        // Desmontar
        currentPlayer.transform.SetParent(null);
        currentPlayer.transform.position = playerSeat.position + cartSeatForwardOffset;

        // Cerrar terminal
        if (cartTerminalUI != null) cartTerminalUI.SetActive(false);

        currentPlayer = null;
    }

    public void OpenInternalTerminal()
    {
        if (cartTerminalUI != null) cartTerminalUI.SetActive(true);
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = Vector3.zero;
        if (currentDirection == CartDirection.Left) moveDir = Vector3.left;
        else if (currentDirection == CartDirection.Right) moveDir = Vector3.right;

        Vector3 platformOffset = Vector3.zero;
        if (currentPlatform != null)
        {
            platformOffset = currentPlatform.position - currentPlatformLastPos;
            currentPlatformLastPos = currentPlatform.position;
        }

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime + platformOffset);
    }

    // Preparado para futuras plataformas móviles
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = collision.transform;
            currentPlatformLastPos = currentPlatform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == currentPlatform) currentPlatform = null;
    }
    */
}
