using UnityEngine;

[System.Serializable]
public class CartSpeedSettings
{
    public float lateralSpeed = 8f;
}

public class MinecartController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerSeat;
    public MinecartTerminalUI cartTerminalUI;
    public MinecartTerminalAnimator terminalAnimator;
    public Rigidbody rb;

    [Header("Movimiento")]
    public float lateralSpeed = 8f;
    public float lateralAcceleration = 5f;

    [Header("Player")]
    public bool isPlayerInside = false;
    public Transform player;
    private PlayerController playerController;

    private float lateralDirection = 0f;
    private float currentLateralSpeed = 0f;
    public float seatHeightOffset = 1f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        // Movimiento lateral progresivo
        float targetSpeed = lateralDirection * lateralSpeed;
        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetSpeed, lateralAcceleration * Time.fixedDeltaTime);

        Vector3 vel = rb.linearVelocity;
        vel.x = currentLateralSpeed;
        rb.linearVelocity = vel;

        // Mantener playerSeat en la vagoneta
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

        // 🔥 Ocultar terminal si el player ya no es hijo
        if (terminalAnimator != null && (player == null || player.parent != playerSeat))
        {
            terminalAnimator.HideTerminal();
        }
    }

    #region Métodos de movimiento lateral (compatibilidad UI)
    public void MoveLeft() => lateralDirection = -1;
    public void MoveRight() => lateralDirection = 1;
    public void StopLateral() => lateralDirection = 0;
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

        // Guardar escala global
        Vector3 originalScale = player.localScale;

        // Poner como hijo del seat para que siga la vagoneta
        if (playerSeat != null)
            player.SetParent(playerSeat);

        // Posicionar y rotar sin cambiar escala
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;
        player.localScale = originalScale; // restaurar escala original

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

            if (playerController != null)
                playerController.enabled = true;

            Rigidbody prb = player.GetComponent<Rigidbody>();
            Collider pcol = player.GetComponent<Collider>();

            if (prb != null) prb.isKinematic = false;
            if (pcol != null) pcol.enabled = true;

            player.SetParent(null);
            player.position += Vector3.up * 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // 🔥 Ocultar el canvas del terminal si existe
            if (terminalAnimator != null)
            {
                Debug.Log("[MinecartController] Ocultando terminal al destruir la vagoneta.");
                terminalAnimator.HideTerminal();
            }
            else if (cartTerminalUI != null)
            {
                cartTerminalUI.gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
