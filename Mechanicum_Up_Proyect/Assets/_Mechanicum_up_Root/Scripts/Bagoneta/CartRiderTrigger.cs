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
    [Header("Opcional")]
    public float connectDuration = 1f; // Delay de animación al entrar

    private MinecartController cart;
    private GameObject player;
    private bool canMount = false;
    private bool isConnecting = false;

    private void Start()
    {
        cart = GetComponentInParent<MinecartController>();
        if (cart == null)
            Debug.LogError("[CartRiderTrigger] No se encontró MinecartController en el padre.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in trigger!");
            player = other.gameObject;
            canMount = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left trigger");
            player = null;
            canMount = false;
        }
    }


    private void Update()
    {
        if (cart == null) return;

        // Mostrar el estado
        Debug.Log($"canMount: {canMount}, isPlayerInside: {cart.isPlayerInside}, isConnecting: {isConnecting}");

        // Detectar tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");

            if (canMount && !cart.isPlayerInside && !isConnecting)
            {
                Debug.Log("Conditions met, starting connection sequence");
                StartCoroutine(StartConnectionSequence());
            }
            else
            {
                Debug.Log("Conditions NOT met to enter cart");
            }
        }
    }


    private IEnumerator StartConnectionSequence()
    {
        if (player == null) yield break;

        isConnecting = true;

        PlayerController controller = player.GetComponent<PlayerController>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        Animator anim = player.GetComponentInChildren<Animator>();

        if (controller != null) controller.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        if (anim != null) anim.applyRootMotion = false;

        // Animación opcional de entrar
        if (anim != null)
            anim.SetTrigger("EnterCart");

        yield return new WaitForSeconds(connectDuration);

        cart.FinalizeEnter(player.transform);

        isConnecting = false;
    }
}

