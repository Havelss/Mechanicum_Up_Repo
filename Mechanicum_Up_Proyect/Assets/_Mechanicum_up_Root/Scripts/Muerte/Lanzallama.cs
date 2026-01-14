using UnityEngine;

public class ChimeneaLanzallamas : MonoBehaviour
{
    public GameObject lanzallamasPrefab; // El prefab "Lanzallamas"
    public Transform spawnPoint;         // Punto de salida del fuego
    public float fireInterval = 2f;      // Cada cuánto tiempo dispara

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
        // Instancia el prefab en el spawnPoint, sin moverse
        Instantiate(lanzallamasPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
