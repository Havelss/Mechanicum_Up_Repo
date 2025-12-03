using UnityEngine;

public class MinecartController : MonoBehaviour
{
    public enum CartDirection { None, Left, Right }

    [Header("Referencias")]
    public Transform playerSeat;         // Asiento del jugador
    public GameObject cartTerminalUI;    // UI del terminal
    public Rigidbody rb;                 // Rigidbody de la bagoneta

    [Header("Movimiento")]
    public float forwardSpeed = 12f;     // Velocidad hacia adelante
    public float gravity = 40f;          // Gravedad manual
    public LayerMask groundMask;
    public float groundCheckDistance = 0.3f;

    [Header("Player")]
    public bool isPlayerInside = false;

    private Transform player;
    private PlayerController playerController;
    private Animator playerAnimator;
    private CartDirection currentDirection = CartDirection.None;
    private bool grounded;

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
            HandleCartMovement();
        }
    }

    private void HandleGroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        Vector3 vel = rb.linearVelocity;

        if (!grounded)
            vel.y -= gravity * Time.deltaTime;
        else if (vel.y < -1f)
            vel.y = -1f;

        rb.linearVelocity = vel;
    }

    private void HandleCartMovement()
    {
        Vector3 vel = rb.linearVelocity;

        if (currentDirection == CartDirection.Left)
            vel.x = -forwardSpeed;
        else if (currentDirection == CartDirection.Right)
            vel.x = forwardSpeed;
        else
            vel.x = 0;

        rb.linearVelocity = vel;
    }

    // -------------------------
    // Métodos para terminal
    // -------------------------
    public void MoveLeft() => currentDirection = CartDirection.Left;
    public void MoveRight() => currentDirection = CartDirection.Right;

    // -------------------------
    // Entrar en la bagoneta
    // -------------------------
    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponentInChildren<Animator>();

        if (playerController != null)
            playerController.enabled = false;

        if (playerAnimator != null)
        {
            playerAnimator.applyRootMotion = true; // reproducir animación de entrada
        }

        // Posicionar jugador
        player.position = playerSeat.position;
        player.rotation = playerSeat.rotation;

        isPlayerInside = true;

        // Mostrar terminal UI
        if (cartTerminalUI != null)
            cartTerminalUI.SetActive(true);
    }

    // -------------------------
    // Salir de la bagoneta
    // -------------------------
    public void ExitCart()
    {
        if (!isPlayerInside) return;

        isPlayerInside = false;

        if (playerController != null)
            playerController.enabled = true;

        if (playerAnimator != null)
        {
            playerAnimator.applyRootMotion = false;
            playerAnimator.speed = 1f;
            playerAnimator.Play("Idle"); // dejar idle
        }

        // Sacar jugador del asiento un poco
        player.position += transform.right * 1f + Vector3.up * 0.5f;

        // Ocultar terminal UI
        if (cartTerminalUI != null)
            cartTerminalUI.SetActive(false);
    }

    // -------------------------
    // Terminal UI
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
