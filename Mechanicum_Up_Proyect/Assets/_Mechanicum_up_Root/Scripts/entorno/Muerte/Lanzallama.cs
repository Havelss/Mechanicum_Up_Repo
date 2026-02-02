using UnityEngine;

public class ChimeneaLanzallamas : MonoBehaviour
{
    [Header("Lanzallamas")]
    public GameObject lanzallamasPrefab; // El prefab "Lanzallamas"
    public Transform spawnPoint;         // Punto de salida del fuego
    public float fireInterval = 2f;      // Cada cuánto tiempo dispara

    [Header("Audio")]
    public AudioSource audioSource;      // Fuente de audio
    public AudioClip fireSFX;            // Sonido de disparo del lanzallamas

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireInterval)
        {
            Disparar();
            timer = 0f;
        }
    }

    void Disparar()
    {
        // Instancia el prefab en el spawnPoint
        Instantiate(lanzallamasPrefab, spawnPoint.position, spawnPoint.rotation);

        // Reproduce el SFX
        if (audioSource != null && fireSFX != null)
        {
            audioSource.PlayOneShot(fireSFX);
        }
    }
}
