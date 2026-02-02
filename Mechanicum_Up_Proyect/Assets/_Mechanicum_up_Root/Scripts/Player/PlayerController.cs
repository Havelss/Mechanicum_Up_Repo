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
    [HideInInspector] public bool isRespawning = false;
    #endregion

    #region PowerUp Visual
    [Header("PowerUp Visual")]
    [SerializeField] private GameObject electricPowerVisualPrefab; // SM_Pila IBM
    [SerializeField] private string powerAnchorName = "PowerUpHolder";
    #endregion

    #region Respawn VFX
    [Header("Respawn VFX")]
    [SerializeField] private GameObject respawnVfxPrefab; // Prefab "Respawn System"
    #endregion

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (camTransform == null)
            camTransform = Camera.main.transform;
    }

    private void Start()
    {
        // 🔹 NO aplicamos visual de powerup ni VFX aquí para que solo ocurra al respawn
    }

    private void Update()
    {
        if (isDead) return;

        CheckIfGrounded();
        UpdateAnimator();
        HandleFootsteps();

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

        playerAnimator.SetFloat("Speed", moveInput.magnitude);
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }

    void HandleFootsteps()
    {
        if (!playerAnimator || !footstepSource || !footstepClip) return;

        bool walking = playerAnimator.GetFloat("Speed") > 0.1f && isGrounded;

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

        MinecartController cart = GetComponentInParent<MinecartController>();
        if (cart != null)
        {
            Debug.Log("[PlayerController] Player muere dentro de vagoneta → destruyéndola.");
            Destroy(cart.gameObject);
        }

        transform.SetParent(null);
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

    // =========================
    // POWERUP VISUAL
    // =========================
    public void ApplyPowerUpVisuals()
    {
        if (PlayerInventory.Instance == null)
            return;

        if (!PlayerInventory.Instance.hasElectricPower)
            return;

        Transform anchor = FindChildRecursive(transform, powerAnchorName);
        if (anchor == null || electricPowerVisualPrefab == null)
            return;

        if (anchor.childCount > 0)
            return;

        GameObject visual = Instantiate(
            electricPowerVisualPrefab,
            anchor.position,
            anchor.rotation,
            anchor
        );

        visual.name = electricPowerVisualPrefab.name;
    }

    // =========================
    // RESPAWN VFX
    // =========================
    public void PlayRespawnVFX()
    {
        if (respawnVfxPrefab == null) return;

        GameObject vfx = Instantiate(respawnVfxPrefab, transform.position, Quaternion.identity);
        Destroy(vfx, 5f); // dura 5 segundos, ajusta según tu prefab
    }

    // =========================
    // FIND CHILD RECURSIVE
    // =========================
    private Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, name);
            if (result != null)
                return result;
        }

        return null;
    }
}
