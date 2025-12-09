//using UnityEngine;
//using System.Collections;

//public class CartRiderTrigger : MonoBehaviour
//{
//    private MinecartController cart;
//    private GameObject player;
//    private bool canMount = false;
//    public float connectDuration = 1.5f;
//    private bool isConnecting = false;

//    private void Start() => cart = GetComponentInParent<MinecartController>();

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player") && !cart.isPlayerInside)
//        {
//            player = other.transform.root.gameObject;
//            canMount = true;
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player") && !cart.isPlayerInside)
//        {
//            canMount = false;
//            player = null;
//        }
//    }

//    private void Update()
//    {
//        if (canMount && !cart.isPlayerInside && !isConnecting && Input.GetKeyDown(KeyCode.E))
//        {
//            StartCoroutine(StartConnectionSequence());
//        }

//        if (cart.isPlayerInside && Input.GetKeyDown(KeyCode.E))
//        {
//            cart.ExitCart();
//        }
//    }

//    private IEnumerator StartConnectionSequence()
//    {
//        if (player == null) yield break;

//        isConnecting = true;

//        var controller = player.GetComponent<PlayerController>();
//        var rb = player.GetComponent<Rigidbody>();
//        var anim = player.GetComponentInChildren<Animator>();

//        if (controller != null) controller.enabled = false;
//        if (rb != null) { rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; rb.isKinematic = true; }
//        if (anim != null) anim.applyRootMotion = false;

//        if (cart.playerSeat != null)
//        {
//            player.transform.SetParent(cart.playerSeat);
//            player.transform.localPosition = Vector3.zero;
//            player.transform.localRotation = Quaternion.identity;
//        }

//        yield return new WaitForSeconds(connectDuration);

//        if (player != null) cart.FinalizeEnter(player.transform);
//        cart.OpenInternalTerminal();
//        isConnecting = false;
//    }
//}

using UnityEngine;
using System.Collections;

public class CartRiderTrigger : MonoBehaviour
{
    private MinecartController cart;
    private GameObject player;
    private bool canMount = false;
    public float connectDuration = 1f;
    private bool isConnecting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Buscar MinecartController dinámicamente si no está asignado
        if (cart == null)
            cart = GetComponentInParent<MinecartController>();

        if (cart != null && !cart.isPlayerInside)
        {
            player = other.transform.root.gameObject;
            canMount = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (cart != null && !cart.isPlayerInside)
        {
            canMount = false;
            player = null;
        }
    }

    private void Update()
    {
        if (canMount && !isConnecting && cart != null && !cart.isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartConnectionSequence());
        }

        if (cart != null && cart.isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            cart.ExitCart();
        }
    }

    private IEnumerator StartConnectionSequence()
    {
        if (player == null || cart == null) yield break;

        isConnecting = true;

        // Desactivar controles del jugador
        var controller = player.GetComponent<PlayerController>();
        var rb = player.GetComponent<Rigidbody>();
        var anim = player.GetComponentInChildren<Animator>();

        if (controller != null) controller.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        if (anim != null) anim.applyRootMotion = false;

        // Mover jugador al asiento
        if (cart.playerSeat != null)
        {
            player.transform.SetParent(cart.playerSeat);
            player.transform.localPosition = Vector3.zero;
            player.transform.localRotation = Quaternion.identity;
        }

        yield return new WaitForSeconds(connectDuration);

        // Finalizar entrada
        cart.FinalizeEnter(player.transform);

        isConnecting = false;
    }
}

