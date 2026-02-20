using UnityEngine;
using System.Collections;

public class CartRiderTrigger : MonoBehaviour
{
    [Header("Opcional")]
    public float connectDuration = 0.5f; // Delay al subir

    private MinecartController cart;
    private GameObject player;
    private bool isConnecting = false;

    private void Start()
    {
        cart = GetComponentInParent<MinecartController>();
        if (cart == null)
            Debug.LogError("[CartRiderTrigger] No se encontró MinecartController en el padre.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && cart != null && !cart.isPlayerInside && !isConnecting)
        {
            player = other.gameObject;
            StartCoroutine(StartConnectionSequence());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Opcional: si quieres que se cancele al salir antes de subir
        if (other.CompareTag("Player") && player == other.gameObject && isConnecting)
        {
            StopAllCoroutines();
            isConnecting = false;
            player = null;
        }
    }

    private IEnumerator StartConnectionSequence()
    {
        if (player == null) yield break;

        isConnecting = true;

        PlayerController controller = player.GetComponent<PlayerController>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        Collider col = player.GetComponent<Collider>();
        Animator anim = player.GetComponentInChildren<Animator>();

        if (controller != null) controller.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // Desactivar física
        }

        if (col != null) col.enabled = false;
        if (anim != null) anim.applyRootMotion = false;

        yield return new WaitForSeconds(connectDuration);

        if (cart != null)
            cart.FinalizeEnter(player.transform);

        isConnecting = false;
        player = null;
    }
}