using UnityEngine;
using System.Collections;

public class TriggerAnimRespawn : MonoBehaviour
{
    [Header("Detección")]
    public string playerTag = "Player";
    public bool triggerOnlyOnce = true;

    [Header("Animación")]
    [Tooltip("Animator que ejecutará la animación (puede estar en este objeto o en otro)")]
    public Animator animator;

    [Tooltip("Nombre EXACTO del Trigger en el Animator")]
    public string animationTriggerName = "Activate";

    [Tooltip("Duración real de la animación en segundos")]
    public float animationDuration = 2f;

    [Header("Respawn")]
    [Tooltip("Lugar donde reaparecerá el player tras la animación")]
    public Transform respawnPoint;

    [Header("Opciones Player")]
    public bool disablePlayerController = true;
    public bool freezePlayerRigidbody = true;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated && triggerOnlyOnce) return;
        if (!other.CompareTag(playerTag)) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        activated = true;
        StartCoroutine(PlayAnimationAndRespawn(other.transform, pc));
    }

    private IEnumerator PlayAnimationAndRespawn(Transform player, PlayerController pc)
    {
        Debug.Log("[TriggerAnimRespawn] Trigger activado");

        // 🔒 Bloquear player
        if (disablePlayerController)
            pc.enabled = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (freezePlayerRigidbody && rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 🎬 Ejecutar animación
        if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            Debug.Log("[TriggerAnimRespawn] Ejecutando animación: " + animationTriggerName);
            animator.SetTrigger(animationTriggerName);
        }
        else
        {
            Debug.LogWarning("[TriggerAnimRespawn] Animator o Trigger no configurados");
        }

        // ⏱️ Esperar a que termine
        yield return new WaitForSeconds(animationDuration);

        // 📍 Respawn
        if (respawnPoint != null)
        {
            player.position = respawnPoint.position;
            player.rotation = respawnPoint.rotation;
            Debug.Log("[TriggerAnimRespawn] Player respawneado");
        }
        else
        {
            Debug.LogWarning("[TriggerAnimRespawn] No hay Respawn Point asignado");
        }

        // 🔓 Liberar player
        if (rb != null)
            rb.isKinematic = false;

        if (disablePlayerController)
            pc.enabled = true;
    }
}