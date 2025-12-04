using UnityEngine;

public class MinecartController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerSeat;                  // Donde se sienta el player
    public GameObject cartTerminalUI;             // UI del terminal (opcional si solo activas/desactivas)
    public MinecartTerminalAnimator terminalAnimator; // Animator del panel para la animación tipo tablet
    public Rigidbody rb;                          // Rigidbody del carrito

    [Header("Movimiento")]
    public float forwardSpeed = 12f;
    public float lateralSpeed = 8f;
    public float gravity = 40f;
    public LayerMask groundMask;
    public float groundCheckDistance = 0.3f;

    [Header("Player")]
    public bool isPlayerInside = false;
    Transform player;
    PlayerController playerController;

    bool grounded;
    private float lateralDirection = 0f; // -1 = izquierda, 1 = derecha

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        HandleGroundCheck();

        if (isPlayerInside)
        {
            MoveCartForward();
            MoveCartLateral();
        }
    }

    // -------------------------
    //       FÍSICAS
    // -------------------------
    void HandleGroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        Vector3 vel = rb.linearVelocity;

        if (!grounded)
            vel.y -= gravity * Time.deltaTime;
        else if (vel.y < -1f)
            vel.y = -1f;

        rb.linearVelocity = vel;
    }

    void MoveCartForward()
    {
        Vector3 vel = rb.linearVelocity;
        vel.x = transform.forward.x * forwardSpeed;
        vel.z = transform.forward.z * forwardSpeed;
        rb.linearVelocity = vel;
    }

    void MoveCartLateral()
    {
        Vector3 vel = rb.linearVelocity;
        vel += transform.right * lateralDirection * lateralSpeed;
        rb.linearVelocity = vel;
    }

    public void MoveLeft() => lateralDirection = -1f;
    public void MoveRight() => lateralDirection = 1f;
    public void StopLateral() => lateralDirection = 0f;

    // -------------------------
    //      ENTRAR AL CART
    // -------------------------
    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
        }

        player.position = playerSeat.position;
        player.rotation = playerSeat.rotation;

        isPlayerInside = true;

        // ---------- Mostrar terminal ----------
        if (terminalAnimator != null)
            terminalAnimator.ShowTerminal();
        else if (cartTerminalUI != null)
            cartTerminalUI.SetActive(true);
    }

    // -------------------------
    //      SALIR DEL CART
    // -------------------------
    public void ExitCart()
    {
        if (!isPlayerInside) return;

        isPlayerInside = false;

        if (playerController != null)
        {
            playerController.enabled = true;
            Rigidbody prb = player.GetComponent<Rigidbody>();
            prb.isKinematic = false;
        }

        player.position += transform.right * 1f + Vector3.up * 0.5f;

        // ---------- Ocultar terminal ----------
        if (terminalAnimator != null)
            terminalAnimator.HideTerminal();
        else if (cartTerminalUI != null)
            cartTerminalUI.SetActive(false);
    }

    // -------------------------
    //      TERMINAL UI (opcional)
    // -------------------------
    public void OpenInternalTerminal()
    {
        if (terminalAnimator != null)
            terminalAnimator.ShowTerminal();
        else if (cartTerminalUI != null)
            cartTerminalUI.SetActive(true);
    }

    public void CloseInternalTerminal()
    {
        if (terminalAnimator != null)
            terminalAnimator.HideTerminal();
        else if (cartTerminalUI != null)
            cartTerminalUI.SetActive(false);
    }

    // -------------------------
    //      SPAWN
    // -------------------------
    public static bool CanSpawnCart()
    {
        return FindObjectsByType<MinecartController>(FindObjectsSortMode.None).Length == 0;
    }
}
