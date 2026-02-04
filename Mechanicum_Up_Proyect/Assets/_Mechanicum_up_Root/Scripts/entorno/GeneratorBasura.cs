using UnityEngine;
using System.Collections;

public class GeneratorBasura : MonoBehaviour
{
    [Header("Configuración del Prefab")]
    [Tooltip("El objeto que quieres instanciar")]
    public GameObject prefabBasura;

    [Tooltip("¿Cuántos segundos vive cada objeto antes de desaparecer?")]
    public float tiempoDeVida = 5.0f;

    [Header("Configuración del Tiempo")]
    [Tooltip("Tiempo de espera entre cada generación")]
    public float spawnInterval = 3.0f;

    [Tooltip("¿Debe empezar a generar basura nada más iniciar el juego?")]
    public bool spawnOnStart = true;

    [Header("Opciones de Posición")]
    [Tooltip("Añade una rotación aleatoria en el eje Y")]
    public bool randomRotation = true;

    private bool isSpawning = false;

    private void Start()
    {
        if (spawnOnStart)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            // 1. Creamos la instancia y la guardamos en una variable temporal 'basura'
            GameObject basura = Instantiate(prefabBasura, transform.position, transform.rotation);

            // 2. Le aplicamos la rotación si está activado
            if (randomRotation)
            {
                basura.transform.Rotate(0, Random.Range(0, 360), 0);
            }

            // 3. ¡La clave! Programamos su destrucción en el momento de nacer
            // Esto no detiene el script, solo le dice a Unity: "Borra esto en X segundos"
            Destroy(basura, tiempoDeVida);

            // 4. Esperamos para el siguiente spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}