using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform camTransform;
    [SerializeField] Animator playerAnimator;

    [Header("Movimiento")]
    [SerializeField] float speed = 10f;

    [Header("Salto cargado")]
    [SerializeField] float jumpForce = 12f;
    [SerializeField] float chargeDownAmount = 0.4f; // cuánto baja el personaje al cargar
    [SerializeField] float chargeTime = 0.25f;       // tiempo mínimo de carga antes de permitir soltar

    [Header("GroundCheck")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Caída pesada")]
    public float normalMass = 1f;
    public float fallingMass = 6f;

    Rigidbody rb;
    Vector2 moveInput;
    bool isGrounded;
    bool wasGrounded;

    bool isChargingJump = false;
    float chargeTimer = 0f;
    Vector3 originalScale;

    bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
        rb.freezeRotation = true;

        if (camTransform == null)
            camTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (isDead) return;

        CheckIfGrounded();
        HandleFallingMass();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        HandleMovement();
        HandleRotation();
    }

    // ---------------- MOVIMIENTO ----------------

    void HandleMovement()
    {
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;

        rb.linearVelocity = new Vector3(
            moveDir.x * speed,
            rb.linearVelocity.y,
            moveDir.z * speed
        );
    }

    void HandleRotation()
    {
        if (moveInput == Vector2.zero) return;

        Vector3 moveDir = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (moveDir.sqrMagnitude < 0.01f) return;

        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    // ---------------- SALTO CARGADO ----------------

    void StartChargingJump()
    {
        if (!isGrounded || isChargingJump) return;

        isChargingJump = true;
        chargeTimer = 0f;

        // Se "agacha" el jugador (resorte)
        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y - chargeDownAmount,
            originalScale.z
        );

        // Congelar subida/bajada mientras cargamos
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Animación
        if (playerAnimator) playerAnimator.SetBool("ChargingJump", true);
    }

    void ReleaseJump()
    {
        if (!isChargingJump) return;
        if (chargeTimer < chargeTime) return; // no permitir salto sin “carga”

        // Volver a tamaño normal
        transform.localScale = originalScale;

        // El salto real
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isChargingJump = false;

        // Animación
        if (playerAnimator) playerAnimator.SetBool("ChargingJump", false);
        if (playerAnimator) playerAnimator.SetTrigger("JumpStart");
    }


    // Input Actions
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) StartChargingJump();
        if (ctx.canceled) ReleaseJump();
    }

    // ---------------- GROUND CHECK ----------------

    void CheckIfGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            // volver del aire → reset masa y escala
            rb.mass = normalMass;
            transform.localScale = originalScale;
        }

        if (isChargingJump)
            chargeTimer += Time.deltaTime;
    }

    // ---------------- CAÍDA PESADA ----------------

    void HandleFallingMass()
    {
        if (!isGrounded && !isChargingJump)
            rb.mass = fallingMass;
        else
            rb.mass = normalMass;
    }

    // ---------------- ANIMACIONES ----------------

    void UpdateAnimator()
    {
        if (!playerAnimator) return;

        playerAnimator.SetFloat("Speed", moveInput.magnitude);
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }

    // ---------------- INPUT MOVIMIENTO ----------------

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isDead)
            moveInput = ctx.ReadValue<Vector2>();
    }

    // ---------------- DEBUG ----------------

    private void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // ---------------- CHECKPOINT SUPPORT ----------------
    [HideInInspector] public Transform currentCheckpoint;

    // Wrapper para compatibilidad con PlayerRespawn
    public void Die(string cause = "")
    {
        DieInstant(cause);
    }


    // ---------------- MUERTE GENERAL ----------------

    public bool IsDead()
    {
        return isDead;
    }

    public void DieInstant(string cause = "")
    {
        if (isDead) return;

        isDead = true;

        // Anular movimiento
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector3.zero;

        // Restaurar escala por si muere cargando salto
        transform.localScale = originalScale;

        // Animación de muerte
        if (playerAnimator != null)
        {
            if (cause == "crush")
                playerAnimator.Play("DeathCrushed");
            else
                playerAnimator.Play("Death");
        }

        // Respawn automático
        StartCoroutine(RespawnAfterDeath());
    }

    IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSeconds(1.2f); // ajusta si quieres

        // Llevar al checkpoint más adelante si tienes
        // Aquí solo reseteo para que no crashee
        isDead = false;
        transform.localScale = originalScale;

        if (playerAnimator)
            playerAnimator.Play("Idle");
    }
}
