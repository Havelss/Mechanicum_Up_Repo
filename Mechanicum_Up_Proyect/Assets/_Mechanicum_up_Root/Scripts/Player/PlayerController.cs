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
    [SerializeField] float rotationSpeed = 10f; // para giro instantáneo 180°

    [Header("Salto cargado")]
    [SerializeField] float minJumpForce = 6f;
    [SerializeField] float maxJumpForce = 12f;
    [SerializeField] float maxChargeTime = 1f;
    [SerializeField] float chargeDownAmount = 0.4f;

    [Header("GroundCheck")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Caída pesada")]
    public float normalMass = 1f;
    public float fallingMass = 6f;

    [Header("Muerte y Respawn")]
    public Transform currentCheckpoint;
    public float respawnDelay = 1.5f;

    Rigidbody playerRB;
    Vector2 moveInput;
    bool isGrounded;
    bool wasGrounded;

    bool isChargingJump = false;
    float chargeTimer = 0f;
    Vector3 originalScale;

    bool isDead = false;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        originalScale = transform.localScale;

        if (camTransform == null)
            camTransform = Camera.main.transform;

        playerRB.freezeRotation = true;
        playerRB.mass = normalMass;
    }

    private void Update()
    {
        if (isDead) return;

        CheckIfGrounded();
        HandleFallingMass();
        UpdateAnimator();

        if (isChargingJump)
        {
            chargeTimer += Time.deltaTime;
            // Ajusta escala mientras carga
            float scaleY = Mathf.Lerp(originalScale.y, originalScale.y - chargeDownAmount, chargeTimer / maxChargeTime);
            transform.localScale = new Vector3(originalScale.x, scaleY, originalScale.z);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (!isChargingJump)
            HandleMovement();

        HandleRotation180();
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

        playerRB.linearVelocity = new Vector3(
            moveDir.x * speed,
            playerRB.linearVelocity.y,
            moveDir.z * speed
        );
    }

    // Giro instantáneo 180° sobre Y
    void HandleRotation180()
    {
        if (moveInput.x > 0.1f)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        else if (moveInput.x < -0.1f)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        // Si no hay input X no rota
    }

    // ---------------- SALTO CARGADO ----------------

    void StartChargingJump()
    {
        if (!isGrounded || isChargingJump) return;

        isChargingJump = true;
        chargeTimer = 0f;

        // Congelar vertical y horizontal mientras carga salto
        playerRB.linearVelocity = Vector3.zero;

        // Animación
        if (playerAnimator) playerAnimator.SetBool("ChargingJump", true);
    }

    void ReleaseJump()
    {
        if (!isChargingJump) return;

        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, Mathf.Clamp01(chargeTimer / maxChargeTime));

        playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0f, playerRB.linearVelocity.z);
        playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isChargingJump = false;
        transform.localScale = originalScale;

        // Animación
        if (playerAnimator)
        {
            playerAnimator.SetBool("ChargingJump", false);
            playerAnimator.SetTrigger("JumpStart");
        }
    }

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
            playerRB.mass = normalMass;
            transform.localScale = originalScale;

            if (playerAnimator) playerAnimator.SetTrigger("JumpEnd");
        }
    }

    // ---------------- CAÍDA PESADA ----------------

    void HandleFallingMass()
    {
        if (!isGrounded && !isChargingJump)
            playerRB.mass = fallingMass;
        else
            playerRB.mass = normalMass;
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
        if (!isDead && !isChargingJump)
            moveInput = ctx.ReadValue<Vector2>();
        else if (isChargingJump)
            moveInput.y = 0; // bloquea adelante/atrás
    }

    // ---------------- MUERTE ----------------

    public void Die(string cause = "")
    {
        if (isDead) return;

        isDead = true;
        moveInput = Vector2.zero;
        playerRB.linearVelocity = Vector3.zero;

        if (cause == "crush")
            playerAnimator.Play("DeathCrushed");
        else
            playerAnimator.Play("Death");

        StartCoroutine(RespawnAfterDeath());
    }

    IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (currentCheckpoint != null)
            transform.position = currentCheckpoint.position;

        isDead = false;
        playerAnimator.Play("Idle");
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void DieInstant(string cause)
    {
        if (isDead) return;
        isDead = true;
        RespawnAtCheckpoint();
    }

    private void RespawnAtCheckpoint()
    {
        if (currentCheckpoint != null)
            transform.position = currentCheckpoint.position;

        isDead = false;
        playerAnimator.Play("Idle");
    }


}
