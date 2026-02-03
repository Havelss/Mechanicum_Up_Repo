//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections;

//public class PlayerController : MonoBehaviour
//{
//    #region Referencias
//    [Header("Referencias")]
//    [SerializeField] Transform camTransform;
//    [SerializeField] Animator playerAnimator;
//    [Header("Audio de pasos")]
//    [SerializeField] AudioSource footstepSource;
//    [SerializeField] AudioClip footstepClip;
//    #endregion

//    #region Movimiento  
//    [Header("Movimiento")]
//    [SerializeField] float speed = 10f;
//    [SerializeField] float rotationSpeed = 720f;
//    Vector2 moveInput;
//    bool inputEnabled = true;
//    #endregion

//    #region Salto Cargado  
//    [Header("Salto cargado")]
//    [SerializeField] float minJumpForce = 8f;
//    [SerializeField] float maxJumpForce = 16f;
//    [SerializeField] float maxChargeTime = 1.5f;
//    [SerializeField] float chargeDownAmount = 0.4f;
//    [SerializeField] float squashSpeed = 5f;
//    bool isChargingJump = false;
//    float chargeTimer = 0f;
//    Vector3 targetScale;
//    #endregion

//    #region GroundCheck  
//    [Header("GroundCheck")]
//    [SerializeField] Transform groundCheck;
//    [SerializeField] float groundCheckRadius = 0.2f;
//    [SerializeField] LayerMask groundLayer;
//    bool isGrounded;
//    bool wasGrounded;
//    #endregion

//    #region Caída Pesada  
//    [Header("Caída pesada")]
//    [SerializeField] float fallMultiplier = 2.5f;
//    #endregion

//    #region Muerte  
//    [Header("Muerte")]
//    bool isDead = false;
//    #endregion

//    Rigidbody rb;
//    Vector3 originalScale;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true;
//        originalScale = transform.localScale;

//        if (camTransform == null)
//            camTransform = Camera.main.transform;
//    }

//    private void Update()
//    {
//        if (isDead) return;

//        CheckIfGrounded();
//        UpdateAnimator();
//        HandleFootsteps();

//        if (isChargingJump)
//        {
//            chargeTimer += Time.deltaTime;
//            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * squashSpeed);
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (isDead) return;

//        HandleMovement();
//        HandleRotation();
//        HandleFalling();
//    }

//    void HandleMovement()
//    {
//        Vector3 camForward = camTransform.forward;
//        Vector3 camRight = camTransform.right;
//        camForward.y = 0;
//        camRight.y = 0;
//        camForward.Normalize();
//        camRight.Normalize();

//        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

//        if (!isChargingJump)
//            rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
//        else
//            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
//    }

//    void HandleRotation()
//    {
//        if (moveInput == Vector2.zero) return;

//        Vector3 lookDir = new Vector3(moveInput.x, 0, moveInput.y);
//        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
//        transform.rotation = Quaternion.Euler(0, angle, 0);
//    }

//    public void OnJump(InputAction.CallbackContext context)
//    {
//        if (isDead || !inputEnabled) return;

//        if (context.performed) StartChargingJump();
//        else if (context.canceled) ReleaseJump();
//    }

//    void StartChargingJump()
//    {
//        if (!isGrounded || isChargingJump) return;

//        isChargingJump = true;
//        chargeTimer = 0f;
//        targetScale = new Vector3(
//            originalScale.x,
//            originalScale.y - chargeDownAmount,
//            originalScale.z
//        );

//        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

//        if (playerAnimator)
//            playerAnimator.SetBool("ChargingJump", true);
//    }

//    void ReleaseJump()
//    {
//        if (!isChargingJump) return;

//        isChargingJump = false;
//        transform.localScale = originalScale;

//        float jumpForce = Mathf.Lerp(
//            minJumpForce,
//            maxJumpForce,
//            Mathf.Clamp01(chargeTimer / maxChargeTime)
//        );

//        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

//        if (playerAnimator)
//        {
//            playerAnimator.SetBool("ChargingJump", false);
//            playerAnimator.SetTrigger("JumpStart");
//        }

//        chargeTimer = 0f;
//    }

//    void CheckIfGrounded()
//    {
//        wasGrounded = isGrounded;
//        isGrounded = Physics.CheckSphere(
//            groundCheck.position,
//            groundCheckRadius,
//            groundLayer
//        );

//        if (!wasGrounded && isGrounded && playerAnimator != null)
//            playerAnimator.SetTrigger("JumpEnd");
//    }

//    void HandleFalling()
//    {
//        if (!isGrounded && rb.linearVelocity.y < 0)
//            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
//    }

//    void UpdateAnimator()
//    {
//        if (!playerAnimator) return;

//        playerAnimator.SetFloat("Speed", moveInput.magnitude);
//        playerAnimator.SetBool("IsGrounded", isGrounded);
//    }

//    void HandleFootsteps()
//    {
//        if (!playerAnimator || !footstepSource || !footstepClip) return;

//        bool walking = playerAnimator.GetFloat("Speed") > 0.1f && isGrounded;

//        if (walking && !footstepSource.isPlaying)
//        {
//            footstepSource.clip = footstepClip;
//            footstepSource.loop = true;
//            footstepSource.Play();
//        }
//        else if (!walking && footstepSource.isPlaying)
//        {
//            footstepSource.Stop();
//        }
//    }

//    public bool IsDead() => isDead;

//    public void Die(string cause = "")
//    {
//        if (isDead) return;
//        isDead = true;

//        // 🔥 SI ESTÁ EN UNA VAGONETA, DESTRUIRLA
//        MinecartController cart = GetComponentInParent<MinecartController>();
//        if (cart != null)
//        {
//            Debug.Log("[PlayerController] Player muere dentro de vagoneta → destruyéndola.");
//            Destroy(cart.gameObject);
//        }

//        // Limpieza por seguridad
//        transform.SetParent(null);

//        // Respawn vía manager
//        PlayerManager.Instance.OnPlayerDeath();

//        Destroy(gameObject);
//    }

//    public void OnMove(InputAction.CallbackContext ctx)
//    {
//        if (!isDead && inputEnabled)
//            moveInput = ctx.ReadValue<Vector2>();
//    }

//    public void SetInputEnabled(bool value)
//    {
//        inputEnabled = value;
//    }
//}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Referencias
    [Header("Referencias")]
    [SerializeField] Transform camTransform;
    [SerializeField] Animator playerAnimator;
    [Header("Audio de pasos")]
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip footstepClip;
    #endregion

    #region Movimiento  
    [Header("Movimiento")]
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 720f;
    Vector2 moveInput;
    bool inputEnabled = true;
    #endregion

    #region Salto Cargado  
    [Header("Salto cargado")]
    [SerializeField] float minJumpForce = 8f;
    [SerializeField] float maxJumpForce = 16f;
    [SerializeField] float maxChargeTime = 1.5f;
    bool isChargingJump = false;
    float chargeTimer = 0f;
    #endregion

    #region GroundCheck  
    [Header("GroundCheck")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    bool wasGrounded;
    #endregion

    #region Caída Pesada  
    [Header("Caída pesada")]
    [SerializeField] float fallMultiplier = 2.5f;
    #endregion

    #region Muerte  
    [Header("Muerte")]
    bool isDead = false;
    #endregion

    #region Particulas
    [Header ("Particulas")]
    [SerializeField] ParticleSystem runParticles;
    [SerializeField] float minMoveVelocity = 0.1f;
    #endregion

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (camTransform == null)
            camTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (isDead) return;

        CheckIfGrounded();
        UpdateAnimator();
        HandleFootsteps();
        HandleRunParticles();

        // Carga de salto
        if (isChargingJump)
        {
            chargeTimer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        HandleMovement();
        HandleRotation();
        HandleFalling();
    }

    void HandleMovement()
    {
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        if (!isChargingJump)
            rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
        else
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    void HandleRotation()
    {
        if (moveInput == Vector2.zero) return;

        Vector3 lookDir = new Vector3(moveInput.x, 0, moveInput.y);
        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead || !inputEnabled) return;

        if (context.performed) StartChargingJump();
        else if (context.canceled) ReleaseJump();
    }

    void StartChargingJump()
    {
        if (!isGrounded || isChargingJump) return;

        isChargingJump = true;
        chargeTimer = 0f;

        // Cancelar velocidad vertical para que la carga sea limpia
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (playerAnimator)
            playerAnimator.SetBool("ChargingJump", true);
    }

    void ReleaseJump()
    {
        if (!isChargingJump) return;

        isChargingJump = false;

        float jumpForce = Mathf.Lerp(
            minJumpForce,
            maxJumpForce,
            Mathf.Clamp01(chargeTimer / maxChargeTime)
        );

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (playerAnimator)
        {
            playerAnimator.SetBool("ChargingJump", false);
            playerAnimator.SetTrigger("JumpStart");
        }

        chargeTimer = 0f;
    }

    void CheckIfGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (!wasGrounded && isGrounded && playerAnimator != null)
            playerAnimator.SetTrigger("JumpEnd");
    }

    void HandleFalling()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    void UpdateAnimator()
    {
        if (!playerAnimator) return;

        Vector3 horizontalVelocity = new Vector3(
            rb.linearVelocity.x,
            0,
            rb.linearVelocity.z
        );

        bool isWalking =
            horizontalVelocity.magnitude > 0.1f &&
            isGrounded &&
            !isChargingJump &&
            !isDead;

        playerAnimator.SetBool("IsWalking", isWalking);
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }


    void HandleFootsteps()
    {
        if (!playerAnimator || !footstepSource || !footstepClip) return;

        bool walking = playerAnimator.GetBool("IsWalking");

        if (walking && !footstepSource.isPlaying)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true;
            footstepSource.Play();
        }
        else if (!walking && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }


    public bool IsDead() => isDead;

    public void Die(string cause = "")
    {
        if (isDead) return;
        isDead = true;

        // 🔥 SI ESTÁ EN UNA VAGONETA, DESTRUIRLA
        MinecartController cart = GetComponentInParent<MinecartController>();
        if (cart != null)
        {
            Debug.Log("[PlayerController] Player muere dentro de vagoneta → destruyéndola.");
            Destroy(cart.gameObject);
        }
        if (playerAnimator)
        {
            playerAnimator.SetBool("IsWalking", false);
            playerAnimator.SetBool("IsGrounded", false);
        }

        // Limpieza por seguridad
        transform.SetParent(null);

        // Respawn vía manager
        PlayerManager.Instance.OnPlayerDeath();

        Destroy(gameObject);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isDead && inputEnabled)
            moveInput = ctx.ReadValue<Vector2>();
    }

    public void SetInputEnabled(bool value)
    {
        inputEnabled = value;
    }

    void HandleRunParticles()
    {
        if (!runParticles) return;

        // Velocidad horizontal (X/Z)
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        bool isMoving = horizontalVelocity.magnitude > minMoveVelocity;

        bool shouldPlay =
            isGrounded &&
            isMoving &&
            !isChargingJump &&
            !isDead;

        if (shouldPlay && !runParticles.isPlaying)
        {
            runParticles.Play();
        }
        else if (!shouldPlay && runParticles.isPlaying)
        {
            runParticles.Stop();
        }
    }

}
