using UnityEngine;
using System.Collections;

public class TriggerAnimRespawn : MonoBehaviour
{
    [Header("Detección")]
    public string playerTag = "Player";
    public string vagonetaTag = "Vagoneta";
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

        // Detectar player aunque esté dentro de otro objeto
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null)
            pc = other.GetComponentInChildren<PlayerController>();
        if (pc == null)
            pc = other.GetComponentInParent<PlayerController>();

        bool isPlayer = pc != null;
        bool isVagoneta = other.CompareTag(vagonetaTag);

        if (!isPlayer && !isVagoneta)
            return;

        Transform target = isPlayer ? pc.transform : other.transform;

        activated = true;
        StartCoroutine(PlayAnimationAndHandleTarget(target, pc, isVagoneta));
    }

    private IEnumerator PlayAnimationAndHandleTarget(Transform target, PlayerController pc, bool isVagoneta)
    {
        Debug.Log("[TriggerAnimRespawn] Trigger activado");

        // 🔒 Bloquear player si existe
        Rigidbody rb = null;

        if (pc != null)
        {
            if (disablePlayerController)
                pc.enabled = false;

            rb = pc.GetComponent<Rigidbody>();
            if (freezePlayerRigidbody && rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
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

        // 📍 Respawn del player si existe
        if (pc != null && respawnPoint != null)
        {
            pc.transform.position = respawnPoint.position;
            pc.transform.rotation = respawnPoint.rotation;
            Debug.Log("[TriggerAnimRespawn] Player respawneado");
        }

        // 💥 Destruir vagoneta si es vagoneta
        if (isVagoneta)
        {
            Debug.Log("[TriggerAnimRespawn] Vagoneta destruida");
            Destroy(target.gameObject);
        }

        // 🔓 Liberar player
        if (rb != null)
            rb.isKinematic = false;

        if (pc != null && disablePlayerController)
            pc.enabled = true;
    }
}
