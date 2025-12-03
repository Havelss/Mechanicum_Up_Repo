using UnityEngine;

public class MinecartController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerSeat;
    public GameObject cartTerminalUI;
    public Rigidbody rb;

    [Header("Movimiento")]
    public float forwardSpeed = 12f;
    public float gravity = 40f;
    public LayerMask groundMask;
    public float groundCheckDistance = 1f;

    [Header("Player")]
    public bool isPlayerInside = false;
    Transform player;
    PlayerController playerController;

    bool grounded;

    // Control de instancia
    private static bool cartExists = false;
    private void OnEnable() => cartExists = true;
    private void OnDestroy() => cartExists = false;
    public static bool CanSpawnCart() => !cartExists;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        // Ajustar posición inicial sobre el suelo
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out hit, 50f, groundMask))
        {
            transform.position = hit.point + Vector3.up * 0.1f; // un poquito arriba del suelo
        }

        // Empujón inicial para que la gravedad manual funcione
        Vector3 vel = rb.linearVelocity;
        vel.y = -0.1f;
        rb.linearVelocity = vel;
    }

    private void FixedUpdate()
    {
        HandleGroundCheck();
        ApplyGravity();

        if (isPlayerInside)
            MoveCartForward();
    }

    void HandleGroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, grounded ? Color.green : Color.red);
    }

    void ApplyGravity()
    {
        Vector3 vel = rb.linearVelocity;
        if (!grounded)
            vel.y -= gravity * Time.fixedDeltaTime;
        else if (vel.y < 0f)
            vel.y = -0.1f;
        rb.linearVelocity = vel;
    }

    void MoveCartForward()
    {
        Vector3 vel = rb.linearVelocity;
        vel.x = transform.forward.x * forwardSpeed;
        vel.z = transform.forward.z * forwardSpeed;
        rb.linearVelocity = vel;
    }

    // -------------------------
    //      ENTRAR AL CART
    // -------------------------
    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();
        Animator anim = player.GetComponentInChildren<Animator>();

        if (playerController != null)
        {
            playerController.enabled = false;
            Rigidbody prb = player.GetComponent<Rigidbody>();
            if (prb != null) prb.isKinematic = true;
        }

        // Forzar al Animator a estado Idle/neutral
        if (anim != null)
        {
            anim.ResetTrigger("JumpStart");
            anim.ResetTrigger("JumpEnd");
            anim.SetFloat("Speed", 0f);
            anim.SetBool("IsGrounded", true);
            anim.Play("Idle"); // Ajusta según tu animación Idle
        }

        // Colocar en asiento
        player.position = playerSeat.position;
        player.rotation = playerSeat.rotation;

        isPlayerInside = true;
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
            playerController.enabled = true; // Reactiva movimiento
            Rigidbody prb = player.GetComponent<Rigidbody>();
            if (prb != null) prb.isKinematic = false;
        }

        // Restaurar Animator
        Animator anim = player.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.ResetTrigger("JumpStart");
            anim.ResetTrigger("JumpEnd");
            anim.SetFloat("Speed", 0f);
            anim.SetBool("IsGrounded", true);
            anim.Play("Idle"); // Ajusta según tu animación Idle
        }

        // Dar un pequeño empujón para que no caiga dentro del carrito
        player.position += transform.right * 1f + Vector3.up * 0.5f;

        CloseInternalTerminal();
    }

    // -------------------------
    //      TERMINAL UI
    // -------------------------
    public void OpenInternalTerminal()
    {
        if (cartTerminalUI != null)
            cartTerminalUI.SetActive(true);
    }

    public void CloseInternalTerminal()
    {
        if (cartTerminalUI != null)
            cartTerminalUI.SetActive(false);
    }
}
