using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [Header("VFX cuando está a pie")]
    [SerializeField] private GameObject deathVfxPrefab;

    [Header("Animador del coche")]
    [SerializeField] private Animator cartAnimator; // asigna el Animator del coche si aplica

    [Header("Nombre del trigger de animación")]
    [SerializeField] private string deathTriggerName = "Death";

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("[PlayerDeathHandler] No se encontró PlayerController en este GameObject");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("InstantKillZone"))
            return;

        Debug.Log($"[PlayerDeathHandler] Player tocó InstantKillZone: {other.name}");

        // Determinar si está en un coche
        MinecartController cart = GetComponentInParent<MinecartController>();

        if (cart != null)
        {
            if (cartAnimator != null)
            {
                Debug.Log("[PlayerDeathHandler] Player está en coche → activando animación de muerte");
                cartAnimator.SetTrigger(deathTriggerName);
            }
            else
            {
                Debug.LogWarning("[PlayerDeathHandler] Player está en coche pero no hay Animator asignado");
            }
        }
        else
        {
            if (deathVfxPrefab != null)
            {
                Debug.Log("[PlayerDeathHandler] Player a pie → instanciando VFX de muerte");
                GameObject vfx = Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);

                // Destruir VFX después de su duración
                ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
                if (ps != null)
                    Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
                else
                    Destroy(vfx, 5f); // fallback si no es ParticleSystem
            }
            else
            {
                Debug.LogWarning("[PlayerDeathHandler] deathVfxPrefab no asignado");
            }
        }

        // Llamar a la función de muerte del Player
        if (playerController != null)
        {
            playerController.Die("InstantKillZone");
        }
        else
        {
            Debug.LogWarning("[PlayerDeathHandler] PlayerController no encontrado al morir");
        }
    }
}
