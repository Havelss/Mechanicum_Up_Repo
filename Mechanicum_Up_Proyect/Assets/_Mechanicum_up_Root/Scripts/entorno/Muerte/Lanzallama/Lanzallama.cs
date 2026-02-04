using UnityEngine;
using System.Collections;

public class Lanzallama : MonoBehaviour
{
    [Header("Lanzallamas")]
    public GameObject lanzallamasPrefab; // El prefab "Lanzallamas"
    public Transform spawnPoint;         // Punto de salida del fuego
    public float fireInterval = 2f;      // Cada cuánto tiempo dispara

    [Header("Audio")]
    public AudioSource audioSource;      // Fuente de audio
    public AudioClip fireSFX;            // Sonido del lanzallamas
    public float fadeDuration = 0.5f;    // Duración del fade

    float timer;
    float tiempoSinDisparar;
    Coroutine fadeCoroutine;

    void Awake()
    {
        // 🔒 Seguridad: si no está asignado, lo busca o lo crea
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = fireSFX;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.spatialBlend = 1f; // Sonido 3D
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        tiempoSinDisparar += Time.deltaTime;

        if (timer >= fireInterval)
        {
            Disparar();
            timer = 0f;
        }

        // Si deja de disparar, apagamos el sonido suavemente
        if (tiempoSinDisparar > 1.2f && audioSource.isPlaying)
        {
            StartFade(0f);
        }
    }

    void Disparar()
    {
        // Instancia el fuego
        Instantiate(lanzallamasPrefab, spawnPoint.position, spawnPoint.rotation);

        tiempoSinDisparar = 0f;

        // Arranca el sonido con fade in
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            StartFade(1f);
        }
    }

    void StartFade(float targetVolume)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAudio(targetVolume));
    }

    IEnumerator FadeAudio(float targetVolume)
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0f)
            audioSource.Stop();
    }
}
