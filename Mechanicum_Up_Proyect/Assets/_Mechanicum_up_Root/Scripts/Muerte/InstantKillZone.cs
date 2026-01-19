using UnityEngine;
using System.Collections.Generic;

public class InstantKillZone : MonoBehaviour
{
    [Header("Opciones de movimiento (opcional)")]
    [Tooltip("Si quieres que esta zona se mueva junto a otro objeto, por ejemplo un elevador o gato.")]
    public Transform movingParent;

    [Header("Configuración básica")]
    public string playerTag = "Player"; // compatibilidad antigua

    [Tooltip("Tags válidos que pueden morir en esta zona")]
    public List<string> validTags = new List<string>() { "Player" };

    [Header("Zona de Detección")]
    [Tooltip("Tamaño del Box donde el jugador muere instantáneamente")]
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);

    [Tooltip("Capas que se detectarán dentro del box")]
    public LayerMask crushLayers = ~0;

    [Header("Animación al Activarse (opcional)")]
    public Animator optionalAnimator;
    public string triggerAnimationName;

    [Header("Sonido al Activarse (opcional)")]
    public AudioSource optionalAudioSource;
    public AudioClip activationSFX;

    private bool effectTriggered = false;

    private void Start()
    {
        if (movingParent != null)
            transform.SetParent(movingParent);
    }

    // 🔥 SISTEMA POR TRIGGER
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[InstantKillZone] Trigger enter con: {other.name}");

        if (!IsValidTag(other.tag))
        {
            Debug.Log($"[InstantKillZone] Tag '{other.tag}' no válido.");
            return;
        }

        PlayerController pc = other.GetComponent<PlayerController>();

        if (pc == null)
        {
            Debug.LogWarning("[InstantKillZone] Objeto válido pero sin PlayerController.");
            return;
        }

        if (pc.IsDead())
        {
            Debug.Log("[InstantKillZone] Player ya estaba muerto.");
            return;
        }

        Debug.Log("[InstantKillZone] Player muere por zona instantánea.");
        TriggerEffects();
        pc.Die("crush");
    }

    // ✅ NUEVO MÉTODO
    private bool IsValidTag(string tag)
    {
        if (tag == playerTag) return true;

        for (int i = 0; i < validTags.Count; i++)
        {
            if (tag == validTags[i])
                return true;
        }

        return false;
    }

    private void TriggerEffects()
    {
        if (effectTriggered) return;
        effectTriggered = true;

        if (optionalAnimator != null && !string.IsNullOrEmpty(triggerAnimationName))
            optionalAnimator.Play(triggerAnimationName);

        if (optionalAudioSource != null && activationSFX != null)
            optionalAudioSource.PlayOneShot(activationSFX);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
