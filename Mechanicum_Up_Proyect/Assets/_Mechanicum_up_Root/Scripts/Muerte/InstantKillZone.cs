using UnityEngine;

public class InstantKillZone : MonoBehaviour
{
    [Header("Opciones de movimiento (opcional)")]
    [Tooltip("Si quieres que esta zona se mueva junto a otro objeto, por ejemplo un elevador o gato.")]
    public Transform movingParent;

    [Header("Configuración básica")]
    public string playerTag = "Player";

    [Header("Zona de Detección")]
    [Tooltip("Tamaño del Box donde el jugador muere instantáneamente")]
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);

    [Tooltip("Capas que se detectarán dentro del box")]
    public LayerMask crushLayers = ~0; // Detecta todo por defecto

    [Header("Animación al Activarse (opcional)")]
    public Animator optionalAnimator;
    public string triggerAnimationName;

    [Header("Sonido al Activarse (opcional)")]
    public AudioSource optionalAudioSource;
    public AudioClip activationSFX;

    private bool effectTriggered = false;

    private void Start()
    {
        // Si quieres que la zona siga un objeto en movimiento (ej. elevador)
        if (movingParent != null)
            transform.SetParent(movingParent);
    }

    private void FixedUpdate()
    {
        // Detecta dentro del Box
        Collider[] hits = Physics.OverlapBox(transform.position, boxSize * 0.5f, Quaternion.identity, crushLayers);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag(playerTag))
                continue;

            //     PlayerController pc = hit.GetComponent<PlayerController>();

            // if (pc != null && !pc.IsDead())
            {
                TriggerEffects();   // animación + sonido (sin retrasar la muerte)
                                    //       pc.DieInstant("crush");
            }
        }
    }

    private void TriggerEffects()
    {
        if (effectTriggered) return; // Para evitar repetir
        effectTriggered = true;

        // Animación opcional
        if (optionalAnimator != null && !string.IsNullOrEmpty(triggerAnimationName))
            optionalAnimator.Play(triggerAnimationName);

        // Sonido opcional
        if (optionalAudioSource != null && activationSFX != null)
        {
            optionalAudioSource.PlayOneShot(activationSFX);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
