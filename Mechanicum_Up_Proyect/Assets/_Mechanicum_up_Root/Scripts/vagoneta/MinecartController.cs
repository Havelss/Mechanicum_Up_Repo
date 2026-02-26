using UnityEngine;

public class MinecartController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerSeat;
    public MinecartTerminalUI cartTerminalUI;
    public MinecartTerminalAnimator terminalAnimator;
    public Rigidbody rb;

    [Header("Movimiento")]
    public float lateralSpeed = 8f;
    public float lateralAcceleration = 12f;
    public float rotationSmoothing = 15f;

    [Header("Player")]
    public bool isPlayerInside = false;
    public Transform player;
    private PlayerController playerController;
    private Animator playerAnimator;
    public float seatHeightOffset = 1f;

    private float lateralDirection = 0f;
    private float currentLateralSpeed = 0f;
    private Vector3 lastLookDir = Vector3.right;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        rb.constraints =
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        // --- Movimiento lateral suave
        float targetSpeed = lateralDirection * lateralSpeed;
        currentLateralSpeed = Mathf.MoveTowards(
            currentLateralSpeed,
            targetSpeed,
            lateralAcceleration * Time.fixedDeltaTime
        );

        Vector3 vel = rb.linearVelocity;
        vel.x = currentLateralSpeed;
        rb.linearVelocity = vel;

        // --- Posición del asiento
        if (playerSeat != null)
            playerSeat.position = rb.position + Vector3.up * seatHeightOffset;

        // --- Seguimiento del Player
        if (isPlayerInside && player != null)
        {
            if (player.localScale != Vector3.one)
                player.localScale = Vector3.one;

            player.position = playerSeat.position;

            if (lateralDirection > 0.1f) lastLookDir = Vector3.right;
            else if (lateralDirection < -0.1f) lastLookDir = Vector3.left;

            Quaternion targetRotation = Quaternion.LookRotation(lastLookDir);
            player.rotation = Quaternion.Slerp(
                player.rotation,
                targetRotation,
                Time.fixedDeltaTime * rotationSmoothing
            );
        }

        if (terminalAnimator != null && !isPlayerInside)
            terminalAnimator.HideTerminal();
    }

    // --- MÉTODOS TERMINAL ---
    public void MoveLeft() => lateralDirection = -1;
    public void MoveRight() => lateralDirection = 1;
    public void StopLateral() => lateralDirection = 0;
    // ------------------------

    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();

        // 🔥 1. Forzar detener input
        if (playerController != null)
            playerController.SetInputEnabled(false);

        // 🔥 2. Resetear física completamente
        Rigidbody prb = player.GetComponent<Rigidbody>();
        Collider pcol = player.GetComponent<Collider>();

        if (prb != null)
        {
            prb.linearVelocity = Vector3.zero;
            prb.angularVelocity = Vector3.zero;
            prb.isKinematic = true;
        }

        if (pcol != null)
            pcol.enabled = false;

        // 🔥 3. Forzar parámetros del Animator (Idle natural)
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger("JumpStart");
            playerAnimator.ResetTrigger("JumpEnd");

            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.SetBool("IsGrounded", true);
        }

        // 🔥 4. Ahora sí desactivar controller
        if (playerController != null)
            playerController.enabled = false;

        player.SetParent(null);
        player.localScale = Vector3.one;

        lastLookDir = Vector3.right;
        isPlayerInside = true;

        if (terminalAnimator != null)
            terminalAnimator.ShowTerminal();
        else if (cartTerminalUI != null)
            cartTerminalUI.gameObject.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitCart()
    {
        if (!isPlayerInside) return;

        isPlayerInside = false;

        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.SetInputEnabled(true);
        }

        Rigidbody prb = player.GetComponent<Rigidbody>();
        Collider pcol = player.GetComponent<Collider>();

        if (prb != null)
            prb.isKinematic = false;

        if (pcol != null)
            pcol.enabled = true;

        player.position += Vector3.up * 0.5f + transform.right * 1f;

        if (terminalAnimator != null)
            terminalAnimator.HideTerminal();
        else if (cartTerminalUI != null)
            cartTerminalUI.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = null;
    }

    private void OnDestroy()
    {
        if (isPlayerInside && player != null)
        {
            if (playerController != null)
                playerController.enabled = true;

            Rigidbody prb = player.GetComponent<Rigidbody>();
            Collider pcol = player.GetComponent<Collider>();

            if (prb != null)
                prb.isKinematic = false;

            if (pcol != null)
                pcol.enabled = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}