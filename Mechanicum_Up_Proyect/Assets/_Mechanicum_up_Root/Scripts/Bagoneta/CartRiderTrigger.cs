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
            player = other.transform.root.gameObject; // Tomar objeto raíz por si el collider está en un hijo
            canMount = true;
            Debug.Log($"[CartRiderTrigger] Player detected: {player.name}");
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
        // Montar
        if (canMount && !cart.isPlayerInside && !isConnecting && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartConnectionSequence());
        }

        // Desmontar
        if (cart.isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            cart.ExitCart();
        }
    }

    private System.Collections.IEnumerator StartConnectionSequence()
    {
        if (player == null)
        {
            Debug.LogError("[CartRiderTrigger] Player es null al iniciar coroutine!");
            yield break;
        }

        GameObject playerLocal = player; // Guardamos referencia local
        isConnecting = true;

        var controller = playerLocal.GetComponent<PlayerController>();
        var rb = playerLocal.GetComponent<Rigidbody>();
        var anim = playerLocal.GetComponentInChildren<Animator>();

        // Bloquear input y Rigidbody
        if (controller != null) controller.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Teletransportar al asiento
        if (cart.playerSeat != null)
        {
            playerLocal.transform.SetParent(cart.playerSeat);
            playerLocal.transform.localPosition = Vector3.zero;
            playerLocal.transform.localRotation = Quaternion.identity;
            Debug.Log($"[CartRiderTrigger] Player {playerLocal.name} moved to seat {cart.playerSeat.name}");
        }

        // Animación (solo si existe)
        if (anim != null && anim.HasParameterOfType("ConnectToCart", UnityEngine.AnimatorControllerParameterType.Trigger))
            anim.SetTrigger("ConnectToCart");

        yield return new WaitForSeconds(connectDuration);

        // Finalizar entrada
        if (playerLocal != null)
            cart.FinalizeEnter(playerLocal);
        else
            Debug.LogError("[CartRiderTrigger] Player se perdió antes de llamar a FinalizeEnter");

        cart.OpenInternalTerminal();
        isConnecting = false;
    }
}

// Método auxiliar para verificar si un trigger existe
public static class AnimatorExtensions
{
    public static bool HasParameterOfType(this Animator animator, string paramName, AnimatorControllerParameterType type)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == type && param.name == paramName) return true;
        }
        return false;
    }
}
