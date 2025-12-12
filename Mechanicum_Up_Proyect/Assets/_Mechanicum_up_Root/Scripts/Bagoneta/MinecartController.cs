//using UnityEngine;

//[System.Serializable]
//public class CartSpeedSettings
//{

//    public float lateralSpeed = 8f;
//}

//public class MinecartController : MonoBehaviour
//{
//    [Header("Referencias")]
//    public Transform playerSeat;                      // Donde se sienta el jugador
//    public MinecartTerminalUI cartTerminalUI;         // UI de la terminal
//    public MinecartTerminalAnimator terminalAnimator; // Animator opcional del panel
//    public Rigidbody rb;                              // Rigidbody del carrito

//    [Header("Movimiento")]
//    public float forwardSpeed = 12f;
//    public float lateralSpeed = 8f;
//    public float lateralAcceleration = 5f; // Ajusta la aceleración lateral

//    [Header("Player")]
//    public bool isPlayerInside = false;
//    private Transform player;
//    private PlayerController playerController;

//    private float lateralDirection = 0f;        // -1 = izquierda, 1 = derecha
//    private float currentLateralSpeed = 0f;     // Velocidad actual lateral progresiva

//    private void Awake()
//    {
//        if (rb == null) rb = GetComponent<Rigidbody>();
//        rb.useGravity = false;
//        rb.constraints = RigidbodyConstraints.FreezeRotation;

//        // Buscar la terminal en la escena si no se asignó
//        if (cartTerminalUI == null)
//        {
//            var terminal = GameObject.FindWithTag("CartTerminal");
//            if (terminal != null)
//            {
//                cartTerminalUI = terminal.GetComponent<MinecartTerminalUI>();
//                cartTerminalUI.SetCart(this);
//            }
//        }

//        if (terminalAnimator == null && cartTerminalUI != null)
//            terminalAnimator = cartTerminalUI.GetComponentInChildren<MinecartTerminalAnimator>();
//    }

//    private void Update()
//    {
//        if (isPlayerInside)
//        {
//            MoveCartLateral();
//        }
//    }

//    private void MoveCartLateral()
//    {
//        // Calculamos la velocidad objetivo según la dirección
//        float targetSpeed = lateralDirection * lateralSpeed;

//        // Aceleramos o desaceleramos progresivamente hacia targetSpeed
//        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetSpeed, lateralAcceleration * Time.deltaTime);

//        // Aplicamos la velocidad lateral
//        Vector3 vel = rb.linearVelocity;
//        vel += transform.right * (currentLateralSpeed - vel.x); // Ajuste local
//        rb.linearVelocity = vel;
//    }

//    // Métodos para controlar la dirección desde la terminal o input
//    public void MoveLeft() => lateralDirection = -1f;
//    public void MoveRight() => lateralDirection = 1f;
//    public void StopLateral() => lateralDirection = 0f;

//    #region Player Enter/Exit
//    public void FinalizeEnter(Transform playerObj)
//    {
//        player = playerObj;
//        playerController = player.GetComponent<PlayerController>();

//        if (playerController != null)
//            playerController.enabled = false;

//        Rigidbody prb = player.GetComponent<Rigidbody>();
//        if (prb != null) prb.isKinematic = true;

//        player.position = playerSeat.position;
//        player.rotation = playerSeat.rotation;

//        isPlayerInside = true;

//        // Mostrar terminal
//        if (terminalAnimator != null)
//            terminalAnimator.ShowTerminal();
//        else if (cartTerminalUI != null)
//            cartTerminalUI.gameObject.SetActive(true);

//        // Cursor visible
//        Cursor.visible = true;
//        Cursor.lockState = CursorLockMode.None;
//    }

//    public void ExitCart()
//    {
//        if (!isPlayerInside) return;

//        isPlayerInside = false;

//        if (playerController != null)
//            playerController.enabled = true;

//        Rigidbody prb = player.GetComponent<Rigidbody>();
//        if (prb != null) prb.isKinematic = false;

//        player.position += transform.right * 1f + Vector3.up * 0.5f;

//        // Ocultar terminal
//        if (terminalAnimator != null)
//            terminalAnimator.HideTerminal();
//        else if (cartTerminalUI != null)
//            cartTerminalUI.gameObject.SetActive(false);

//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;

//        player = null;
//        playerController = null;
//    }
//    #endregion
//}

using UnityEngine;

[System.Serializable]
public class CartSpeedSettings
{
    public float lateralSpeed = 8f; // velocidad lateral
}

public class MinecartController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerSeat;                      // Donde se sienta el jugador
    public MinecartTerminalUI cartTerminalUI;         // UI de la terminal
    public MinecartTerminalAnimator terminalAnimator; // Animator opcional del panel
    public Rigidbody rb;                              // Rigidbody de la vagoneta

    [Header("Movimiento")]
    public float lateralSpeed = 8f;
    public float lateralAcceleration = 5f; // aceleración progresiva lateral

    [Header("Player")]
    public bool isPlayerInside = false;
    private Transform player;
    private PlayerController playerController;

    private float lateralDirection = 0f;    // -1 izquierda, 1 derecha
    private float currentLateralSpeed = 0f; // velocidad lateral progresiva

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float minHeight = 0.01f;
    private bool isGrounded;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Solo freeze de rotación
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Asignar terminal si no está asignada
        if (cartTerminalUI == null)
        {
            var terminal = GameObject.FindWithTag("CartTerminal");
            if (terminal != null)
            {
                cartTerminalUI = terminal.GetComponent<MinecartTerminalUI>();
                cartTerminalUI.SetCart(this);
            }
        }

        if (terminalAnimator == null && cartTerminalUI != null)
            terminalAnimator = cartTerminalUI.GetComponentInChildren<MinecartTerminalAnimator>();

        // Crear groundCheck si no existe
        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = gc.transform;
        }
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            // Guardamos la dirección de entrada para usarla en FixedUpdate
            currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed,
                lateralDirection * lateralSpeed,
                lateralAcceleration * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        MoveCartLateral();
        FollowSeatFix();
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isGrounded) return;

        Vector3 pos = rb.position;
        if (pos.y < minHeight)
        {
            pos.y = minHeight;
            rb.position = pos;
        }
    }

    private void MoveCartLateral()
    {
        Vector3 vel = rb.linearVelocity;

        // Solo modificar el eje X
        vel.x = currentLateralSpeed;

        // Mantener la gravedad en Y y la Z (si aplica)
        rb.linearVelocity = vel;

        Debug.Log($"[MoveCartLateral] lateralDirection={lateralDirection}, currentLateralSpeed={currentLateralSpeed}, rb.pos={rb.position}");
    }

    #region Métodos de movimiento lateral (Terminal)
    public void MoveLeft() => lateralDirection = -1f;
    public void MoveRight() => lateralDirection = 1f;
    public void StopLateral() => lateralDirection = 0f;
    #endregion

    #region Player Enter/Exit
    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
            playerController.enabled = false;

        Rigidbody prb = player.GetComponent<Rigidbody>();
        if (prb != null)
        {
            prb.isKinematic = true;
            prb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Parent al asiento
        player.SetParent(playerSeat);
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;

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
            playerController.enabled = true;

        Rigidbody prb = player.GetComponent<Rigidbody>();
        if (prb != null)
        {
            prb.isKinematic = false;
            prb.constraints = RigidbodyConstraints.None;
        }

        player.SetParent(null);
        player.position += transform.right * 1f + Vector3.up * 0.5f;

        if (terminalAnimator != null)
            terminalAnimator.HideTerminal();
        else if (cartTerminalUI != null)
            cartTerminalUI.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = null;
        playerController = null;
    }
    #endregion

    #region PlayerSeat Follow
    private void FollowSeatFix()
    {
        if (playerSeat == null) return;

        // Solo mover el seat, no la vagoneta
        playerSeat.position = rb.position + Vector3.up * 1f;
        playerSeat.rotation = transform.rotation;
    }
    #endregion
}
