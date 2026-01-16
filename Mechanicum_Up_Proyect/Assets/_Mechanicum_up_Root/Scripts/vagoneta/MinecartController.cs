using UnityEngine;

[System.Serializable]
public class CartSpeedSettings
{
    public float lateralSpeed = 8f;
}

public class MinecartController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerSeat;                      // Empty dentro de la vagoneta
    public MinecartTerminalUI cartTerminalUI;
    public MinecartTerminalAnimator terminalAnimator;
    public Rigidbody rb;                              // Rigidbody de la vagoneta

    [Header("Movimiento")]
    public float lateralSpeed = 8f;
    public float lateralAcceleration = 5f;

    [Header("Player")]
    public bool isPlayerInside = false;
    private Transform player;
    private PlayerController playerController;

    private float lateralDirection = 0f;
    private float currentLateralSpeed = 0f;
    public float seatHeightOffset = 1f; // altura del player respecto al Rigidbody

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        // Mover la vagoneta lateralmente
        MoveCartLateral();

        // Mantener el playerSeat en la vagoneta
        if (playerSeat != null)
        {
            playerSeat.position = rb.position + Vector3.up * seatHeightOffset;
            playerSeat.rotation = transform.rotation;
        }

        // Actualizar posición y rotación del player
        if (isPlayerInside && player != null)
        {
            player.position = playerSeat.position;
            player.rotation = playerSeat.rotation;
        }
    }

    private void MoveCartLateral()
    {
        float targetSpeed = lateralDirection * lateralSpeed;
        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetSpeed, lateralAcceleration * Time.fixedDeltaTime);

        Vector3 vel = rb.linearVelocity;
        vel.x = currentLateralSpeed;
        rb.linearVelocity = vel;
    }

    #region Métodos de movimiento lateral
    public void MoveLeft() => lateralDirection = -1f;
    public void MoveRight() => lateralDirection = 1f;
    public void StopLateral() => lateralDirection = 0f;
    #endregion

    #region Player Enter/Exit
    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();

        if (playerController != null) playerController.enabled = false;

        Rigidbody prb = player.GetComponent<Rigidbody>();
        Collider pcol = player.GetComponent<Collider>();

        if (prb != null) prb.isKinematic = true;
        if (pcol != null) pcol.enabled = false;

        // Posicionar en el seat
        player.localPosition = Vector3.zero; // local respecto al seat
        player.localRotation = Quaternion.identity;

        isPlayerInside = true;

        if (terminalAnimator != null) terminalAnimator.ShowTerminal();
        else if (cartTerminalUI != null) cartTerminalUI.gameObject.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitCart()
    {
        if (!isPlayerInside) return;

        isPlayerInside = false;

        if (playerController != null) playerController.enabled = true;

        Rigidbody prb = player.GetComponent<Rigidbody>();
        Collider pcol = player.GetComponent<Collider>();

        if (prb != null) prb.isKinematic = false;
        if (pcol != null) pcol.enabled = true;

        player.SetParent(null);
        player.position += transform.right * 1f + Vector3.up * 0.5f;

        if (terminalAnimator != null) terminalAnimator.HideTerminal();
        else if (cartTerminalUI != null) cartTerminalUI.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = null;
        playerController = null;
    }

    private void OnDestroy()
    {
        // Si la vagoneta se destruye con el player dentro
        if (isPlayerInside && player != null)
        {
            Debug.Log("[MinecartController] Vagoneta destruida con el player dentro. Liberando player.");

            // Reactivar control del player
            if (playerController != null)
                playerController.enabled = true;

            Rigidbody prb = player.GetComponent<Rigidbody>();
            Collider pcol = player.GetComponent<Collider>();

            if (prb != null) prb.isKinematic = false;
            if (pcol != null) pcol.enabled = true;

            player.SetParent(null);

            // Pequeño offset de seguridad
            player.position += Vector3.up * 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    #endregion
}