using UnityEngine;
using System.Collections;

public class TriggerAnimRespawn : MonoBehaviour
{
    [Header("Detección")]
    public string vagonetaTag = "Vagoneta";
    public bool triggerOnlyOnce = true;

    [Header("Configuración de Animación")]
    [Tooltip("Nombre de la animación del Player")]
    public string playerAnimName = "rig_IBM_Vagoneta";

    [Tooltip("Nombre de la animación de la Vagoneta")]
    public string vagonetaAnimName = "Vagoneta_Rig_Vagoneta_RigAction";

    public float animationDuration = 2f;

    [Header("Respawn")]
    public Transform respawnPoint;

    [Header("Opciones de Bloqueo")]
    public bool disablePlayerController = true;
    public bool freezePlayerRigidbody = true;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated && triggerOnlyOnce) return;

        PlayerController pc = null;
        MinecartController cart = null;

        // 1. Detectar si es el Player o la Vagoneta
        pc = other.GetComponentInParent<PlayerController>();
        cart = other.GetComponentInParent<MinecartController>();

        // 2. Si es vagoneta, buscar al player dentro
        if (cart != null && pc == null)
        {
            if (cart.isPlayerInside && cart.player != null)
            {
                pc = cart.player.GetComponent<PlayerController>();
            }
        }

        // Si no hay nadie válido, fuera
        if (pc == null && cart == null) return;

        activated = true;
        StartCoroutine(PerformDeathSequence(pc, cart));
    }

    private IEnumerator PerformDeathSequence(PlayerController pc, MinecartController cart)
    {
        Debug.Log("[TriggerAnimRespawn] 🎬 Iniciando secuencia simultánea");

        Animator playerAnim = null;
        Animator cartAnim = null;
        Rigidbody playerRb = null;

        // --- PREPARACIÓN Y BLOQUEO ---
        if (pc != null)
        {
            if (disablePlayerController) pc.enabled = false;
            playerRb = pc.GetComponent<Rigidbody>();
            if (freezePlayerRigidbody && playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.isKinematic = true;
            }
            playerAnim = pc.GetComponentInChildren<Animator>();
        }

        if (cart != null)
        {
            cartAnim = cart.GetComponentInChildren<Animator>();
            // Si la vagoneta tiene RB, la frenamos para que la animación se vea bien
            if (cart.rb != null) cart.rb.isKinematic = true;
        }

        // --- EJECUCIÓN SIMULTÁNEA ---
        // Usamos Play() para disparar el nombre exacto del estado en el Animator
        if (playerAnim != null)
        {
            playerAnim.Play(playerAnimName);
            Debug.Log($"[TriggerAnimRespawn] Animando Player: {playerAnimName}");
        }

        if (cartAnim != null)
        {
            cartAnim.Play(vagonetaAnimName);
            Debug.Log($"[TriggerAnimRespawn] Animando Vagoneta: {vagonetaAnimName}");
        }

        // Esperar a que la "escena" termine
        yield return new WaitForSeconds(animationDuration);

        // --- RESPAWN Y LIMPIEZA ---
        if (pc != null && respawnPoint != null)
        {
            // Salir de la vagoneta antes de moverlo
            if (cart != null) cart.ExitCart();

            pc.transform.position = respawnPoint.position;
            pc.transform.rotation = respawnPoint.rotation;
        }

        // Destruir la vagoneta después del respawn
        if (cart != null)
        {
            Destroy(cart.gameObject);
        }

        // Liberar al jugador
        if (pc != null)
        {
            if (playerRb != null) playerRb.isKinematic = false;
            if (disablePlayerController) pc.enabled = true;
        }
    }
}