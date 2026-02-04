using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarehouseLightsController : MonoBehaviour
{
    [System.Serializable]
    public class LightGroup
    {
        public Light lightSource;
        public Renderer lightRenderer;
        [HideInInspector] public float originalIntensity; // Aquí guardaremos el valor del Inspector
    }

    [Header("Configuración de Luces")]
    public List<LightGroup> lights;

    [Header("Ajustes de Encendido")]
    public float delayBetweenLights = 0.5f;
    public float fadeDuration = 0.3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip lightClickSound;

    private bool isTriggered = false;

    private void Awake()
    {
        // PASO CLAVE: Antes de apagar nada, guardamos la intensidad que configuraste
        foreach (var group in lights)
        {
            if (group.lightSource != null)
            {
                // Memorizamos si era 150, 1000, etc.
                group.originalIntensity = group.lightSource.intensity;

                // Ahora sí, las apagamos para el inicio del juego
                group.lightSource.intensity = 0;
                group.lightSource.enabled = false;
            }

            if (group.lightRenderer != null)
            {
                group.lightRenderer.material.DisableKeyword("_EMISSION");
                group.lightRenderer.material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    public void ActivateLights()
    {
        if (isTriggered) return;
        isTriggered = true;
        StartCoroutine(TurnOnLightsRoutine());
    }

    private IEnumerator TurnOnLightsRoutine()
    {
        foreach (var group in lights)
        {
            if (group.lightSource != null)
            {
                if (audioSource != null && lightClickSound != null)
                    audioSource.PlayOneShot(lightClickSound);

                StartCoroutine(FadeLightIn(group));
            }
            yield return new WaitForSeconds(delayBetweenLights);
        }
    }

    private IEnumerator FadeLightIn(LightGroup group)
    {
        float elapsed = 0;
        group.lightSource.enabled = true;

        if (group.lightRenderer != null)
            group.lightRenderer.material.EnableKeyword("_EMISSION");

        // El color base para la emisión visual
        Color baseEmission = group.lightRenderer != null ? group.lightRenderer.material.color : Color.white;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            // Interpolamos hacia su intensidad original guardada
            group.lightSource.intensity = Mathf.Lerp(0, group.originalIntensity, t);

            if (group.lightRenderer != null)
            {
                // Multiplicamos por la intensidad original para el brillo HDR del material
                group.lightRenderer.material.SetColor("_EmissionColor", baseEmission * Mathf.Lerp(0, group.originalIntensity, t));
            }

            yield return null;
        }

        // Aseguramos el valor exacto del Inspector al final
        group.lightSource.intensity = group.originalIntensity;
    }
}