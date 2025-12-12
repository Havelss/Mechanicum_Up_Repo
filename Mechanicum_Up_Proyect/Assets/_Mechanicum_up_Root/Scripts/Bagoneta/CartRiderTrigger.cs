//using UnityEngine;
//using System.Collections;

//public class CartRiderTrigger : MonoBehaviour
//{
//    [Header("Opcional")]
//    public float connectDuration = 1f; // Delay de animación al entrar

//    private MinecartController cart;
//    private GameObject player;
//    private bool canMount = false;
//    private bool isConnecting = false;

//    private void Start()
//    {
//        cart = GetComponentInParent<MinecartController>();
//        if (cart == null)
//            Debug.LogError("[CartRiderTrigger] No se encontró MinecartController en el padre.");
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("Trigger Enter: " + other.name);
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("Player detected in trigger!");
//            player = other.gameObject;
//            canMount = true;
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        Debug.Log("Trigger Exit: " + other.name);
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("Player left trigger");
//            player = null;
//            canMount = false;
//        }
//    }


//    private void Update()
//    {
//        if (cart == null) return;

//        // Mostrar el estado
//        Debug.Log($"canMount: {canMount}, isPlayerInside: {cart.isPlayerInside}, isConnecting: {isConnecting}");

//        // Detectar tecla E
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            Debug.Log("E key pressed");

//            if (canMount && !cart.isPlayerInside && !isConnecting && Input.GetKey(KeyCode.E))
//            {
//                Debug.Log("Conditions met, starting connection sequence");
//                StartCoroutine(StartConnectionSequence());
//            }
//            else
//            {
//                Debug.Log("Conditions NOT met to enter cart");
//            }
//        }
//    }


//    private IEnumerator StartConnectionSequence()
//    {
//        if (player == null) yield break;

//        isConnecting = true;

//        PlayerController controller = player.GetComponent<PlayerController>();
//        Rigidbody rb = player.GetComponent<Rigidbody>();
//        Animator anim = player.GetComponentInChildren<Animator>();

//        if (controller != null) controller.enabled = false;
//        if (rb != null)
//        {
//            rb.linearVelocity = Vector3.zero;
//            rb.angularVelocity = Vector3.zero;
//            rb.isKinematic = true;
//        }
//        if (anim != null) anim.applyRootMotion = false;

//        // Animación opcional de entrar
//        //if (anim != null)
//        //    anim.SetTrigger("EnterCart");

//        yield return new WaitForSeconds(connectDuration);

//        cart.FinalizeEnter(player.transform);

//        isConnecting = false;
//    }
//}

// CartRiderTrigger.cs
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
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            canMount = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            canMount = false;
        }
    }

    private void Update()
    {
        if (cart == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canMount && !cart.isPlayerInside && !isConnecting)
            {
                StartCoroutine(StartConnectionSequence());
            }
        }
    }

    private IEnumerator StartConnectionSequence()
    {
        if (player == null) yield break;

        isConnecting = true;

        // Componentes del player
        PlayerController controller = player.GetComponent<PlayerController>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        Collider col = player.GetComponent<Collider>();
        Animator anim = player.GetComponentInChildren<Animator>();

        if (controller != null) controller.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;   // Desactivamos física
        }

        if (col != null)
            col.enabled = false;     // Collider no interfiere

        if (anim != null)
            anim.applyRootMotion = false;

        yield return new WaitForSeconds(connectDuration);

        cart.FinalizeEnter(player.transform);

        isConnecting = false;
    }
}

