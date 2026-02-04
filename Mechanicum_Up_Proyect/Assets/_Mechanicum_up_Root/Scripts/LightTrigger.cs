using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    public WarehouseLightsController controller;

    [Header("Opciones")]
    [Tooltip("Si está activo, el GameObject que contiene este trigger se destruye después de activar las luces.")]
    public bool destroyOnActivate = true;
    [Tooltip("Retraso en segundos antes de destruir el GameObject (0 = inmediato).")]
    public float destroyDelay = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu player tenga el tag Player
        {
            controller.ActivateLights();

            if (destroyOnActivate)
                Destroy(gameObject, Mathf.Max(0f, destroyDelay));
        }
    }
}