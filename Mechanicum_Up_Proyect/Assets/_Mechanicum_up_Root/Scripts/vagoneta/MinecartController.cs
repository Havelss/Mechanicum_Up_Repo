//using UnityEngine;

//[System.Serializable]
//public class CartSpeedSettings
//{
//    public float lateralSpeed = 8f;
//}

//public class MinecartController : MonoBehaviour
//{
//    [Header("Referencias")]
//    public Transform playerSeat;
//    public MinecartTerminalUI cartTerminalUI;
//    public MinecartTerminalAnimator terminalAnimator;
//    public Rigidbody rb;

//    [Header("Movimiento")]
//    public float lateralSpeed = 8f;
//    public float lateralAcceleration = 5f;

//    [Header("Player")]
//    public bool isPlayerInside = false;
//    public Transform player;
//    private PlayerController playerController;

//    private float lateralDirection = 0f;
//    private float currentLateralSpeed = 0f;
//    public float seatHeightOffset = 1f;

//    private void Awake()
//    {
//        if (rb == null) rb = GetComponent<Rigidbody>();
//        rb.useGravity = true;
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//    }

//    private void FixedUpdate()
//    {
//        // Movimiento lateral progresivo
//        float targetSpeed = lateralDirection * lateralSpeed;
//        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetSpeed, lateralAcceleration * Time.fixedDeltaTime);

//        Vector3 vel = rb.linearVelocity;
//        vel.x = currentLateralSpeed;
//        rb.linearVelocity = vel;

//        // Mantener playerSeat en la vagoneta
//        if (playerSeat != null)
//        {
//            playerSeat.position = rb.position + Vector3.up * seatHeightOffset;
//            playerSeat.rotation = transform.rotation;
//        }

//        // Actualizar posición y rotación del player
//        if (isPlayerInside && player != null)
//        {
//            player.position = playerSeat.position;
//            player.rotation = playerSeat.rotation;
//        }

//        // 🔥 Ocultar terminal si el player ya no es hijo
//        if (terminalAnimator != null && (player == null || player.parent != playerSeat))
//        {
//            terminalAnimator.HideTerminal();
//        }
//    }

//    #region Métodos de movimiento lateral (compatibilidad UI)
//    public void MoveLeft() => lateralDirection = -1;
//    public void MoveRight() => lateralDirection = 1;
//    public void StopLateral() => lateralDirection = 0;
//    #endregion

//    #region Player Enter/Exit
//    public void FinalizeEnter(Transform playerObj)
//    {
//        player = playerObj;
//        playerController = player.GetComponent<PlayerController>();

//        if (playerController != null) playerController.enabled = false;

//        Rigidbody prb = player.GetComponent<Rigidbody>();
//        Collider pcol = player.GetComponent<Collider>();

//        if (prb != null) prb.isKinematic = true;
//        if (pcol != null) pcol.enabled = false;

//        // Guardar escala global
//        Vector3 originalScale = player.localScale;

//        // Poner como hijo del seat para que siga la vagoneta
//        if (playerSeat != null)
//            player.SetParent(playerSeat);

//        // Posicionar y rotar sin cambiar escala
//        player.localPosition = Vector3.zero;
//        player.localRotation = Quaternion.identity;
//        player.localScale = originalScale; // restaurar escala original

//        isPlayerInside = true;

//        if (terminalAnimator != null) terminalAnimator.ShowTerminal();
//        else if (cartTerminalUI != null) cartTerminalUI.gameObject.SetActive(true);

//        Cursor.visible = true;
//        Cursor.lockState = CursorLockMode.None;
//    }



//    public void ExitCart()
//    {
//        if (!isPlayerInside) return;

//        isPlayerInside = false;

//        if (playerController != null) playerController.enabled = true;

//        Rigidbody prb = player.GetComponent<Rigidbody>();
//        Collider pcol = player.GetComponent<Collider>();

//        if (prb != null) prb.isKinematic = false;
//        if (pcol != null) pcol.enabled = true;

//        player.SetParent(null);
//        player.position += transform.right * 1f + Vector3.up * 0.5f;

//        if (terminalAnimator != null) terminalAnimator.HideTerminal();
//        else if (cartTerminalUI != null) cartTerminalUI.gameObject.SetActive(false);

//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;

//        player = null;
//        playerController = null;
//    }

//    private void OnDestroy()
//    {
//        // Si la vagoneta se destruye con el player dentro
//        if (isPlayerInside && player != null)
//        {
//            Debug.Log("[MinecartController] Vagoneta destruida con el player dentro. Liberando player.");

//            if (playerController != null)
//                playerController.enabled = true;

//            Rigidbody prb = player.GetComponent<Rigidbody>();
//            Collider pcol = player.GetComponent<Collider>();

//            if (prb != null) prb.isKinematic = false;
//            if (pcol != null) pcol.enabled = true;

//            player.SetParent(null);
//            player.position += Vector3.up * 1f;

//            Cursor.visible = false;
//            Cursor.lockState = CursorLockMode.Locked;

//            // 🔥 Ocultar el canvas del terminal si existe
//            if (terminalAnimator != null)
//            {
//                Debug.Log("[MinecartController] Ocultando terminal al destruir la vagoneta.");
//                terminalAnimator.HideTerminal();
//            }
//            else if (cartTerminalUI != null)
//            {
//                cartTerminalUI.gameObject.SetActive(false);
//            }
//        }
//    }

//    #endregion
//}

//////using UnityEngine;

//////public class MinecartController : MonoBehaviour
//////{
//////    [Header("Referencias")]
//////    public Transform playerSeat;
//////    public MinecartTerminalUI cartTerminalUI;
//////    public MinecartTerminalAnimator terminalAnimator;
//////    public Rigidbody rb;

//////    [Header("Movimiento")]
//////    public float lateralSpeed = 8f;
//////    public float lateralAcceleration = 5f;
//////    public float rotationSmoothing = 15f; // Aumentado para un giro más responsivo

//////    [Header("Player")]
//////    public bool isPlayerInside = false;
//////    public Transform player;
//////    private PlayerController playerController;

//////    private float lateralDirection = 0f;
//////    private float currentLateralSpeed = 0f;
//////    public float seatHeightOffset = 1f;

//////    // Para recordar la última dirección X y que no intente mirar a Z al frenar
//////    private Vector3 lastLookDir = Vector3.right;

//////    private void Awake()
//////    {
//////        if (rb == null) rb = GetComponent<Rigidbody>();
//////        rb.useGravity = true;
//////        rb.constraints = RigidbodyConstraints.FreezeRotation;
//////    }

//////    private void FixedUpdate()
//////    {
//////        // 1. Movimiento
//////        float targetSpeed = lateralDirection * lateralSpeed;
//////        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetSpeed, lateralAcceleration * Time.fixedDeltaTime);

//////        Vector3 vel = rb.linearVelocity;
//////        vel.x = currentLateralSpeed;
//////        rb.linearVelocity = vel;

//////        if (playerSeat != null)
//////        {
//////            playerSeat.position = rb.position + Vector3.up * seatHeightOffset;
//////            playerSeat.rotation = transform.rotation;
//////        }

//////        // 2. Rotación estricta en X / -X
//////        if (isPlayerInside && player != null)
//////        {
//////            player.position = playerSeat.position;

//////            // Solo actualizamos la dirección si hay movimiento claro
//////            if (lateralDirection > 0.1f) lastLookDir = Vector3.right;
//////            else if (lateralDirection < -0.1f) lastLookDir = Vector3.left;

//////            // Aplicar la rotación basada únicamente en la dirección X
//////            Quaternion targetRotation = Quaternion.LookRotation(lastLookDir);
//////            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.fixedDeltaTime * rotationSmoothing);
//////        }

//////        if (terminalAnimator != null && (player == null || player.parent != playerSeat))
//////        {
//////            terminalAnimator.HideTerminal();
//////        }
//////    }

//////    public void MoveLeft() => lateralDirection = -1;
//////    public void MoveRight() => lateralDirection = 1;
//////    public void StopLateral() => lateralDirection = 0;

//////    public void FinalizeEnter(Transform playerObj)
//////    {
//////        player = playerObj;
//////        playerController = player.GetComponent<PlayerController>();

//////        if (playerController != null) playerController.enabled = false;

//////        Rigidbody prb = player.GetComponent<Rigidbody>();
//////        Collider pcol = player.GetComponent<Collider>();

//////        if (prb != null) prb.isKinematic = true;
//////        if (pcol != null) pcol.enabled = false;

//////        // FIX ESCALA
//////        Vector3 worldScale = player.lossyScale;
//////        if (playerSeat != null) player.SetParent(playerSeat);

//////        if (player.parent != null)
//////        {
//////            player.localScale = new Vector3(
//////                worldScale.x / player.parent.lossyScale.x,
//////                worldScale.y / player.parent.lossyScale.y,
//////                worldScale.z / player.parent.lossyScale.z
//////            );
//////        }

//////        player.localPosition = Vector3.zero;

//////        // Al entrar, decidimos una dirección inicial (por ejemplo, derecha)
//////        lastLookDir = Vector3.right;
//////        player.rotation = Quaternion.LookRotation(lastLookDir);

//////        isPlayerInside = true;

//////        if (terminalAnimator != null) terminalAnimator.ShowTerminal();
//////        else if (cartTerminalUI != null) cartTerminalUI.gameObject.SetActive(true);

//////        Cursor.visible = true;
//////        Cursor.lockState = CursorLockMode.None;
//////    }

//////    public void ExitCart()
//////    {
//////        if (!isPlayerInside) return;
//////        isPlayerInside = false;

//////        if (playerController != null) playerController.enabled = true;

//////        Rigidbody prb = player.GetComponent<Rigidbody>();
//////        Collider pcol = player.GetComponent<Collider>();

//////        if (prb != null) prb.isKinematic = false;
//////        if (pcol != null) pcol.enabled = true;

//////        Vector3 currentWorldScale = player.lossyScale;
//////        player.SetParent(null);
//////        player.localScale = currentWorldScale;

//////        player.position += transform.right * 1f + Vector3.up * 0.5f;

//////        if (terminalAnimator != null) terminalAnimator.HideTerminal();
//////        else if (cartTerminalUI != null) cartTerminalUI.gameObject.SetActive(false);

//////        Cursor.visible = false;
//////        Cursor.lockState = CursorLockMode.Locked;

//////        player = null;
//////    }
//////}
///
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
    public float lateralAcceleration = 5f;
    public float rotationSmoothing = 15f;

    [Header("Player")]
    public bool isPlayerInside = false;
    public Transform player;
    private PlayerController playerController;

    private float lateralDirection = 0f;
    private float currentLateralSpeed = 0f;
    public float seatHeightOffset = 1f;
    private Vector3 lastLookDir = Vector3.right;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        // 1. Lógica de movimiento de la vagoneta
        float targetSpeed = lateralDirection * lateralSpeed;
        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetSpeed, lateralAcceleration * Time.fixedDeltaTime);

        Vector3 vel = rb.linearVelocity;
        vel.x = currentLateralSpeed;
        rb.linearVelocity = vel;

        if (playerSeat != null)
        {
            playerSeat.position = rb.position + Vector3.up * seatHeightOffset;
        }

        // 2. Seguimiento del Player (Escala y Rotación X)
        if (isPlayerInside && player != null)
        {
            // Forzar escala 1,1,1
            if (player.localScale != Vector3.one) player.localScale = Vector3.one;

            // Pegar a la posición del asiento
            player.position = playerSeat.position;

            // Rotación suave solo en X / -X
            if (lateralDirection > 0.1f) lastLookDir = Vector3.right;
            else if (lateralDirection < -0.1f) lastLookDir = Vector3.left;

            Quaternion targetRotation = Quaternion.LookRotation(lastLookDir);
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.fixedDeltaTime * rotationSmoothing);
        }

        if (terminalAnimator != null && !isPlayerInside)
        {
            terminalAnimator.HideTerminal();
        }
    }

    // --- MÉTODOS PARA EL TERMINAL (SOLUCIONA TUS ERRORES) ---
    public void MoveLeft() => lateralDirection = -1;
    public void MoveRight() => lateralDirection = 1;
    public void StopLateral() => lateralDirection = 0;
    // -------------------------------------------------------

    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();

        if (playerController != null) playerController.enabled = false;

        Rigidbody prb = player.GetComponent<Rigidbody>();
        Collider pcol = player.GetComponent<Collider>();

        if (prb != null) prb.isKinematic = true;
        if (pcol != null) pcol.enabled = false;

        // Liberamos al player de cualquier jerarquía para proteger la escala
        player.SetParent(null);
        player.localScale = Vector3.one;

        lastLookDir = Vector3.right;
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

        player.position += Vector3.up * 0.5f + transform.right * 1f;

        if (terminalAnimator != null) terminalAnimator.HideTerminal();
        else if (cartTerminalUI != null) cartTerminalUI.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = null;
    }

    private void OnDestroy()
    {
        if (isPlayerInside && player != null)
        {
            if (playerController != null) playerController.enabled = true;
            Rigidbody prb = player.GetComponent<Rigidbody>();
            Collider pcol = player.GetComponent<Collider>();
            if (prb != null) prb.isKinematic = false;
            if (pcol != null) pcol.enabled = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}