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

    [Header("Salto")]
    [SerializeField] float jumpForce = 8f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Audio de pasos")]
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip footstepClip;

    [Header("Muerte y Respawn")]
    public Transform currentCheckpoint;
    public float respawnDelay = 1.5f;

    Rigidbody playerRB;
    Vector2 moveInput;
    bool isGrounded;
    bool wasGrounded;
    bool isJumping;
    bool isTouchingWall;

    bool isDead = false; // NUEVO

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        if (camTransform == null) camTransform = Camera.main.transform;
        playerRB.freezeRotation = true;
    }

    void Update()
    {
        if (isDead) return;

        CheckIfGrounded();
        UpdateAnimator();
        HandleFootsteps();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (!isTouchingWall || isGrounded)
        {
            HandleMovement();
            HandleRotation();
        }
        else
        {
            playerRB.linearVelocity = new Vector3(0, playerRB.linearVelocity.y, 0);
        }
    }

    void HandleMovement()
    {
        Vector3 cameraForward = camTransform.forward;
        Vector3 cameraRight = camTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        playerRB.linearVelocity = new Vector3(moveDirection.x * speed, playerRB.linearVelocity.y, moveDirection.z * speed);
    }

    void HandleRotation()
    {
        if (moveInput == Vector2.zero) return;

        Vector3 moveDirection = new Vector3(playerRB.linearVelocity.x, 0, playerRB.linearVelocity.z);
        if (moveDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = targetRotation;
    }

    void CheckIfGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            isJumping = false;
            playerAnimator.SetTrigger("JumpEnd");
        }
    }

    void Jump()
    {
        if (isGrounded && !isDead)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0, playerRB.linearVelocity.z);
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;

            playerAnimator.ResetTrigger("JumpEnd");
            playerAnimator.SetTrigger("JumpStart");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isGrounded && collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
            playerRB.linearVelocity = new Vector3(0, playerRB.linearVelocity.y, 0);
        }
        else
        {
            isTouchingWall = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }

    void UpdateAnimator()
    {
        if (playerAnimator == null) return;

        float moveMagnitude = moveInput.magnitude;
        playerAnimator.SetFloat("Speed", moveMagnitude);
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }

    private void HandleFootsteps()
    {
        if (playerAnimator == null || footstepSource == null || footstepClip == null)
            return;

        float speedValue = playerAnimator.GetFloat("Speed");

        if (speedValue > 0.1f && isGrounded)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.clip = footstepClip;
                footstepSource.loop = true;
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }
    }

    // --------------- SISTEMA DE MUERTE GENERAL -----------------

    public void Die(string cause = "")
    {
        if (isDead) return;

        isDead = true;

        // Detener movimiento e input
        moveInput = Vector2.zero;
        playerRB.linearVelocity = Vector3.zero;

        // Elegir animación según la causa
        if (cause == "crush")
            playerAnimator.Play("DeathCrushed");
        else
            playerAnimator.Play("Death");

        StartCoroutine(RespawnAfterDeath());
    }

    IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Respawn al checkpoint
        if (currentCheckpoint != null)
            transform.position = currentCheckpoint.position;

        // Resetear estados
        isDead = false;
        playerAnimator.Play("Idle");
    }

    // ---------------- INPUT METHODS -----------------

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isDead)
            moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isDead)
            Jump();
    }

    public bool IsDead()
    {
        return isDead;
    }

}
