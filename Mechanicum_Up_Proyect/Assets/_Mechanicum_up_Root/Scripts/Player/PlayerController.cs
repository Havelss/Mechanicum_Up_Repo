using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Referencias
    [Header("Referencias")]
    [SerializeField] Transform camTransform;
    [SerializeField] Animator playerAnimator;
<<<<<<< HEAD

    [Header("Movimiento")]
    [SerializeField] float speed = 10f;

    [Header("Salto")]
    [SerializeField] float jumpForce = 8f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

=======
>>>>>>> Alexander_Vagoneta_Merge_V2
    [Header("Audio de pasos")]
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip footstepClip;
    #endregion

<<<<<<< HEAD
    [Header("Muerte y Respawn")]
    public Transform currentCheckpoint;
    public float respawnDelay = 1.5f;

    [Header("Caída pesada")]
    public float normalMass = 1f;
    public float fallingMass = 4f; // ← masa al caer

    Rigidbody playerRB;
=======
    #region Movimiento  
    [Header("Movimiento")]
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 720f;
>>>>>>> Alexander_Vagoneta_Merge_V2
    Vector2 moveInput;
    #endregion

    #region Salto Cargado  
    [Header("Salto cargado")]
    [SerializeField] float minJumpForce = 8f;
    [SerializeField] float maxJumpForce = 16f;
    [SerializeField] float maxChargeTime = 1.5f;
    [SerializeField] float chargeDownAmount = 0.4f;
    [SerializeField] float squashSpeed = 5f;
    bool isChargingJump = false;
    float chargeTimer = 0f;
    Vector3 targetScale;
    #endregion

    #region GroundCheck  
    [Header("GroundCheck")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    bool wasGrounded;
<<<<<<< HEAD
    bool isJumping;
    bool isTouchingWall;

    bool isDead = false;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();

        if (camTransform == null)
            camTransform = Camera.main.transform;

        playerRB.freezeRotation = true;
        playerRB.mass = normalMass;
=======
    #endregion

    #region Caída Pesada  
    [Header("Caída pesada")]
    [SerializeField] float fallMultiplier = 2.5f;
    #endregion

    #region Muerte y Respawn  
    [Header("Muerte y Respawn")]
    public Transform currentCheckpoint;
    public float respawnDelay = 1.5f;
    bool isDead = false;
    #endregion

    Rigidbody rb;
    Vector3 originalScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        originalScale = transform.localScale;

        if (camTransform == null)
            camTransform = Camera.main.transform;
>>>>>>> Alexander_Vagoneta_Merge_V2
    }

    private void Update()
    {
        if (isDead) return;

        CheckIfGrounded();
        HandleFallingMass();
        UpdateAnimator();
        HandleFootsteps();

        if (isChargingJump)
        {
            chargeTimer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * squashSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

<<<<<<< HEAD
        if (!isTouchingWall || isGrounded)
        {
            HandleMovement();
            HandleRotation();
        }
        else
        {
            playerRB.linearVelocity = new Vector3(0, playerRB.linearVelocity.y, 0);
        }
=======
        HandleMovement();
        HandleRotation();
        HandleFalling();
>>>>>>> Alexander_Vagoneta_Merge_V2
    }

    void HandleMovement()
    {
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

<<<<<<< HEAD
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection =
            (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        playerRB.linearVelocity = new Vector3(
            moveDirection.x * speed,
            playerRB.linearVelocity.y,
            moveDirection.z * speed
        );
=======
        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        if (!isChargingJump)
        {
            rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
>>>>>>> Alexander_Vagoneta_Merge_V2
    }

    void HandleRotation()
    {
        if (moveInput == Vector2.zero) return;

<<<<<<< HEAD
        Vector3 moveDirection = new Vector3(playerRB.linearVelocity.x, 0, playerRB.linearVelocity.z);
        if (moveDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = targetRotation;
=======
        Vector3 lookDir = new Vector3(moveInput.x, 0, moveInput.y);
        if (lookDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead) return;

        if (context.performed) StartChargingJump();
        else if (context.canceled) ReleaseJump();
    }

    void StartChargingJump()
    {
        if (!isGrounded || isChargingJump) return;

        isChargingJump = true;
        chargeTimer = 0f;
        targetScale = new Vector3(originalScale.x, originalScale.y - chargeDownAmount, originalScale.z);
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (playerAnimator)
            playerAnimator.SetBool("ChargingJump", true);
    }

    void ReleaseJump()
    {
        if (!isChargingJump) return;

        isChargingJump = false;
        transform.localScale = originalScale;

        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, Mathf.Clamp01(chargeTimer / maxChargeTime));
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (playerAnimator)
        {
            playerAnimator.SetBool("ChargingJump", false);
            playerAnimator.SetTrigger("JumpStart");
        }

        chargeTimer = 0f;
>>>>>>> Alexander_Vagoneta_Merge_V2
    }

    void CheckIfGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && isGrounded && playerAnimator != null)
            playerAnimator.SetTrigger("JumpEnd");
    }

<<<<<<< HEAD
    // ------------------ CAÍDA PESADA ------------------

    void HandleFallingMass()
    {
        if (!isGrounded)
        {
            // En el aire = masa pesada
            playerRB.mass = fallingMass;
        }
        else
        {
            // En el suelo = masa normal
            playerRB.mass = normalMass;
        }
    }

    // ------------------ SALTO ------------------

    void Jump()
    {
        if (isGrounded && !isDead)
        {
            playerRB.linearVelocity = new Vector3(
                playerRB.linearVelocity.x,
                0,
                playerRB.linearVelocity.z
            );

            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;

            playerAnimator.ResetTrigger("JumpEnd");
            playerAnimator.SetTrigger("JumpStart");
        }
    }

    // ------------------ ANIMACIONES ------------------
=======
    void HandleFalling()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }
>>>>>>> Alexander_Vagoneta_Merge_V2

    void UpdateAnimator()
    {
        if (!playerAnimator) return;

        playerAnimator.SetFloat("Speed", moveInput.magnitude);
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }

    void HandleFootsteps()
    {
        if (playerAnimator == null || footstepSource == null || footstepClip == null) return;

        bool walking = playerAnimator.GetFloat("Speed") > 0.1f && isGrounded;

        if (walking)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.clip = footstepClip;
                footstepSource.loop = true;
                footstepSource.Play();
            }
        }
        else if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }

<<<<<<< HEAD
    // ------------------ MUERTE ------------------

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

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isDead)
            moveInput = context.ReadValue<Vector2>();
=======
    public bool IsDead() => isDead;

    public void DieInstant(string cause)
    {
        if (isDead) return;
        isDead = true;
        RespawnAtCheckpoint();
    }

    public void Die(string cause = "")
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector3.zero;

        if (playerAnimator)
        {
            if (cause == "crush") playerAnimator.Play("DeathCrushed");
            else playerAnimator.Play("Death");
        }

        StartCoroutine(RespawnAfterDeath());
    }

    IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSeconds(respawnDelay);
        RespawnAtCheckpoint();
>>>>>>> Alexander_Vagoneta_Merge_V2
    }

    void RespawnAtCheckpoint()
    {
<<<<<<< HEAD
        if (context.performed && !isDead)
            Jump();
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
=======
        if (currentCheckpoint != null)
            transform.position = currentCheckpoint.position;

        isDead = false;
        if (playerAnimator) playerAnimator.Play("Idle");
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isDead) moveInput = ctx.ReadValue<Vector2>();
    }
}
>>>>>>> Alexander_Vagoneta_Merge_V2
