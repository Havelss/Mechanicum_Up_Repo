using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

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

    #region Salto Normal  
    [Header("Salto Normal")]
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float fallMultiplier = 2.8f; // Peso robótico en la caída
    #endregion

    #region GroundCheck  
    [Header("GroundCheck")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    bool wasGrounded;
    #endregion

    #region Muerte y Respawn
    [Header("Estado")]
    private bool isDead = false;
    [HideInInspector] public bool isRespawning = false;
    #endregion

    #region PowerUp Visual
    [Header("PowerUp Visual")]
    [SerializeField] private GameObject electricPowerVisualPrefab;
    [SerializeField] private string powerAnchorName = "PowerUpHolder";
    #endregion

    #region Respawn VFX
    [Header("Respawn VFX")]
    [SerializeField] private GameObject respawnVfxPrefab;
    #endregion

    #region Partículas
    [Header("Partículas")]
    [SerializeField] ParticleSystem runParticles;
    [SerializeField] float minMoveVelocity = 0.1f;
    #endregion

    #region
    [Header("Audio Muerte")]
    [SerializeField] AudioSource deathAudioSource;
    [SerializeField] AudioClip deathClip;
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
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
    }

    void HandleRotation()
    {
        if (moveInput == Vector2.zero) return;

        Vector3 lookDir = new Vector3(moveInput.x, 0, moveInput.y);
        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    // --- SALTO NORMAL CON PERSONALIDAD ---
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead || !inputEnabled) return;

        if (context.performed && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        // Resetear velocidad vertical
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Empuje inicial
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Pequeño impulso extra para dar sensación robótica
        rb.linearVelocity += Vector3.up * (jumpForce * 0.1f);

        // Animación
        if (playerAnimator)
            playerAnimator.SetTrigger("JumpStart");
    }

    void HandleFalling()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    void CheckIfGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            if (playerAnimator)
                playerAnimator.SetTrigger("JumpEnd");

            // Suavizar impacto
            rb.linearVelocity *= 0.9f;
        }
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
        else if (!walking && footstepSource.isPlaying) footstepSource.Stop();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isDead && inputEnabled) moveInput = ctx.ReadValue<Vector2>();
    }

    public void SetInputEnabled(bool value) => inputEnabled = value;

    void HandleRunParticles()
    {
        if (!runParticles) return;
        Vector3 horizVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        bool shouldPlay = isGrounded && horizVel.magnitude > minMoveVelocity && !isDead;

        if (shouldPlay && !runParticles.isPlaying) runParticles.Play();
        else if (!shouldPlay && runParticles.isPlaying) runParticles.Stop();
    }

    // --- MÉTODOS DE COMPATIBILIDAD ---
    public bool IsDead() => isDead;

    public void Die(string cause = "")
    {
        if (isDead) return;
        isDead = true;

        if (deathClip)
            AudioSource.PlayClipAtPoint(deathClip, transform.position);

        MinecartController cart = GetComponentInParent<MinecartController>();
        if (cart != null) Destroy(cart.gameObject);

        transform.SetParent(null);
        PlayerManager.Instance.OnPlayerDeath();

        Destroy(gameObject);
    }

    public void ApplyPowerUpVisuals()
    {
        if (PlayerInventory.Instance == null || !PlayerInventory.Instance.hasElectricPower) return;

        Transform anchor = FindChildRecursive(transform, powerAnchorName);
        if (anchor == null || electricPowerVisualPrefab == null || anchor.childCount > 0) return;

        GameObject visual = Instantiate(electricPowerVisualPrefab, anchor.position, anchor.rotation, anchor);
        visual.name = electricPowerVisualPrefab.name;
    }

    public void PlayRespawnVFX()
    {
        if (respawnVfxPrefab == null) return;
        GameObject vfx = Instantiate(respawnVfxPrefab, transform.position, Quaternion.identity);
        Destroy(vfx, 5f);
    }

    private Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }
}
