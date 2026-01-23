using UnityEngine;
using System.Collections.Generic;

public class InstantKillZone : MonoBehaviour
{
    [Header("Opciones de movimiento (opcional)")]
    public Transform movingParent;

    [Header("Configuración básica")]
    public string playerTag = "Player";
    public List<string> validTags = new List<string>() { "Player" };

    [Header("Zona de Detección")]
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[InstantKillZone] Trigger enter con: {other.name}");

        // 1️⃣ Primero chequea si es player normal
        if (IsValidTag(other.tag))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null && !pc.IsDead())
            {
                TriggerEffects();
                pc.Die("crush");
                return;
            }
        }

        // 2️⃣ Si es una vagoneta con player dentro
        MinecartController cart = other.GetComponent<MinecartController>();
        if (cart != null && cart.isPlayerInside && cart.player != null)
        {
            PlayerController pcInside = cart.player.GetComponent<PlayerController>();
            if (pcInside != null && !pcInside.IsDead())
            {
                TriggerEffects();
                pcInside.Die("crush");
            }
        }
    }

    private bool IsValidTag(string tag)
    {
        if (tag == playerTag) return true;
        for (int i = 0; i < validTags.Count; i++)
            if (tag == validTags[i]) return true;
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