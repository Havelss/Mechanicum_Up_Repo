using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform camTransform;
    [SerializeField] Animator playerAnimator;

    [Header("Movimiento")]
    [SerializeField] float speed = 10f;
    [SerializeField] float rotSpeed = 15f;

    [Header("Salto")]
    [SerializeField] float jumpForce = 8f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Audio de pasos")]
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip footstepClip;

    Rigidbody playerRB;
    Vector2 moveInput;
    bool isGrounded;
    bool wasGrounded;
    bool isJumping;
    bool isTouchingWall; // Nuevo: Para detectar si está tocando una pared

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        if (camTransform == null) camTransform = Camera.main.transform;
        playerRB.freezeRotation = true;
    }

    void Update()
    {
        CheckIfGrounded();
        UpdateAnimator();
        HandleFootsteps();
    }

    private void FixedUpdate()
    {
        if (!isTouchingWall || isGrounded) // Solo mover si no está tocando una pared o está en el suelo
        {
            HandleMovement();
            HandleRotation();
        }
        else
        {
            // Si está tocando una pared en el aire, detener el movimiento horizontal
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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);
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
        if (isGrounded)
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
        // Detectar si el jugador está tocando una pared
        if (!isGrounded && collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;

            // Detener el movimiento horizontal
            playerRB.linearVelocity = new Vector3(0, playerRB.linearVelocity.y, 0);
        }
        else
        {
            isTouchingWall = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Detectar cuando el jugador deja de tocar una pared
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump();
    }
    #endregion
}