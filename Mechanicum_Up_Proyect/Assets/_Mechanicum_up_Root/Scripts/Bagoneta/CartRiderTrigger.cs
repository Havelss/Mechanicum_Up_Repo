using UnityEngine;

public class CartRiderTrigger : MonoBehaviour
{
    private MinecartController cart;
    private GameObject player;
    private bool canMount = false;

    [Header("Tiempo de animación de conexión")]
    public float connectDuration = 1.5f;

    private bool isConnecting = false;

    private void Start()
    {
        cart = GetComponentInParent<MinecartController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !cart.isPlayerInside)
        {
            player = other.gameObject;
            canMount = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !cart.isPlayerInside)
        {
            canMount = false;
            player = null;
        }
    }

    private void Update()
    {
        if (canMount && !cart.isPlayerInside && !isConnecting && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartConnectionSequence());
        }

        // Desmontaje con E si ya está dentro
        if (cart.isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            cart.ExitCart();
        }
    }

    private System.Collections.IEnumerator StartConnectionSequence()
    {
        isConnecting = true;

        var controller = player.GetComponent<PlayerController>();
        var rb = player.GetComponent<Rigidbody>();
        var anim = player.GetComponentInChildren<Animator>();

        // Bloquear control y Rigidbody
        if (controller != null) controller.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Mover player al asiento
        if (cart.playerSeat != null)
        {
            player.transform.SetParent(cart.playerSeat);
            player.transform.localPosition = Vector3.zero;
            player.transform.localRotation = Quaternion.identity;

            Debug.Log($"[CartRiderTrigger] Player {player.name} moved to seat {cart.playerSeat.name}");
        }
        else
        {
            Debug.LogWarning("[CartRiderTrigger] playerSeat no asignado en MinecartController");
        }

        // Reproducir animación si existe
        if (anim != null) anim.SetTrigger("ConnectToCart");

        yield return new WaitForSeconds(connectDuration);

        // Finalizar entrada
        cart.FinalizeEnter(player);

        // Abrir terminal
        cart.OpenInternalTerminal();

        isConnecting = false;
    }
}
