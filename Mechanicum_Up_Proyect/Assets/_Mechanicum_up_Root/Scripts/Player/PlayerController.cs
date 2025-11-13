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

    Rigidbody playerRB;
    Vector2 moveInput;
    bool isGrounded;
    bool wasGrounded;
    bool isJumping;

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
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
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

        // Detectar aterrizaje
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

    void UpdateAnimator()
    {
        if (playerAnimator == null) return;

        float moveMagnitude = moveInput.magnitude;
        playerAnimator.SetFloat("Speed", moveMagnitude);
        playerAnimator.SetBool("IsGrounded", isGrounded);
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
