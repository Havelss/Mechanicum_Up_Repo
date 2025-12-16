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
//    private Transform player;
//    private PlayerController playerController;

//    private float lateralDirection = 0f;
//    private float currentLateralSpeed = 0f;

//    private void Awake()
//    {
//        if (rb == null) rb = GetComponent<Rigidbody>();
//        rb.useGravity = true;
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//    }

//    private void Update()
//    {
//        if (isPlayerInside)
//        {
//            MoveCartLateral();
//        }
//    }

//    private void FixedUpdate()
//    {
//        // Solo mover vagoneta
//        MoveCartLateral();
//    }

//    private void MoveCartLateral()
//    {
//        float targetSpeed = lateralDirection * lateralSpeed;
//        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetSpeed, lateralAcceleration * Time.fixedDeltaTime);

//        // Movimiento con rb.velocity
//        Vector3 vel = rb.linearVelocity;
//        vel.x = currentLateralSpeed;
//        rb.linearVelocity = vel;
//    }

//    #region Métodos de movimiento lateral
//    public void MoveLeft() => lateralDirection = -1f;
//    public void MoveRight() => lateralDirection = 1f;
//    public void StopLateral() => lateralDirection = 0f;
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

//        // Parent al seat
//        //player.SetParent(playerSeat);
//        //player.localPosition = Vector3.zero;
//        //player.localRotation = Quaternion.identity;

//        player.position = playerSeat.position;
//        player.rotation = playerSeat.rotation;

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
//    #endregion
//}

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
    private Transform player;
    private PlayerController playerController;

    private float lateralDirection = 0f;
    private float currentLateralSpeed = 0f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.WakeUp(); // 🔥 importantísimo al instanciar
    }

    private void Update()
    {
        // Aquí NO movemos el Rigidbody
        // Solo mantenemos estado (dirección ya viene de la UI)
    }

    private void FixedUpdate()
    {
        if (!isPlayerInside) return;

        MoveCartLateral();
    }

    private void MoveCartLateral()
    {
        float targetSpeed = lateralDirection * lateralSpeed;

        currentLateralSpeed = Mathf.MoveTowards(
            currentLateralSpeed,
            targetSpeed,
            lateralAcceleration * Time.fixedDeltaTime
        );

        Vector3 vel = rb.linearVelocity;
        vel.x = currentLateralSpeed;
        rb.linearVelocity = vel;
    }

    #region Métodos de movimiento lateral
    public void MoveLeft() => lateralDirection = -1f;
    public void MoveRight() => lateralDirection = 1f;
    public void StopLateral() => lateralDirection = 0f;
    #endregion

    #region Player Enter / Exit
    public void FinalizeEnter(Transform playerObj)
    {
        player = playerObj;
        playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
            playerController.enabled = false;

        Rigidbody prb = player.GetComponent<Rigidbody>();
        Collider pcol = player.GetComponent<Collider>();

        // 🔒 El player deja de existir para la física
        if (prb != null)
        {
            prb.linearVelocity = Vector3.zero;
            prb.angularVelocity = Vector3.zero;
            prb.isKinematic = true;
            prb.detectCollisions = false;
        }

        if (pcol != null)
            pcol.enabled = false;

        // 🔥 ANCLA REAL
        player.SetParent(playerSeat, worldPositionStays: false);
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;

        isPlayerInside = true;

        rb.WakeUp(); // por si estaba dormido

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
        Collider pcol = player.GetComponent<Collider>();

        player.SetParent(null, true);

        if (prb != null)
        {
            prb.isKinematic = false;
            prb.detectCollisions = true;
        }

        if (pcol != null)
            pcol.enabled = true;

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
}
