using UnityEngine;
using System.Collections;

public class TriggerAnimRespawn : MonoBehaviour
{
    [Header("Detección")]
    public string vagonetaTag = "Vagoneta";
    public bool triggerOnlyOnce = true;

    [Header("Animación")]
    public Animator animator;
    public string animationTriggerName = "Activate";
    public float animationDuration = 2f;

    [Header("Respawn")]
    public Transform respawnPoint;

    [Header("Opciones Player")]
    public bool disablePlayerController = true;
    public bool freezePlayerRigidbody = true;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated && triggerOnlyOnce) return;

        PlayerController pc = null;
        MinecartController cart = null;

        // 1. ¿Es el Player directamente?
        pc = other.GetComponentInParent<PlayerController>();

        // 2. ¿Es una vagoneta?
        if (pc == null)
        {
            cart = other.GetComponentInParent<MinecartController>();
            if (cart != null && cart.isPlayerInside)
            {
                // Si hay alguien dentro, obtenemos su PlayerController
                pc = cart.player.GetComponent<PlayerController>();
            }
        }

        // Si no encontramos ni player ni vagoneta con tag, salimos
        if (pc == null && !other.CompareTag(vagonetaTag))
            return;

        activated = true;
        StartCoroutine(PlayAnimationAndHandleTarget(pc, cart, other.gameObject));
    }

    private IEnumerator PlayAnimationAndHandleTarget(PlayerController pc, MinecartController cart, GameObject originalHit)
    {
        Debug.Log("[TriggerAnimRespawn] Iniciando secuencia de muerte/respawn");

        Rigidbody rb = null;

        // 🔒 Bloquear al Player si lo encontramos
        if (pc != null)
        {
            if (disablePlayerController) pc.enabled = false;
            rb = pc.GetComponent<Rigidbody>();
            if (freezePlayerRigidbody && rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }

        // 🎬 Ejecutar animación del pistón/trampa
        if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            animator.SetTrigger(animationTriggerName);
        }

        // ⏱️ Esperar el impacto de la animación
        yield return new WaitForSeconds(animationDuration);

        // 📍 Teletransportar al Player
        if (pc != null && respawnPoint != null)
        {
            // Si estaba en la vagoneta, primero lo sacamos de ella
            if (cart != null)
            {
                cart.ExitCart();
            }

            pc.transform.position = respawnPoint.position;
            pc.transform.rotation = respawnPoint.rotation;
            Debug.Log("[TriggerAnimRespawn] Player enviado al punto de respawn");
        }

        // 💥 Destruir la vagoneta
        if (cart != null)
        {
            Destroy(cart.gameObject);
        }
        else if (originalHit.CompareTag(vagonetaTag))
        {
            Destroy(originalHit);
        }

        // 🔓 Liberar al Player para que pueda moverse de nuevo
        if (pc != null)
        {
            if (rb != null) rb.isKinematic = false;
            if (disablePlayerController) pc.enabled = true;
        }
    }
}