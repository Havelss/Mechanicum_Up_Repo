using UnityEngine;

public class Lanzallama : MonoBehaviour
{
    public float duracion = 1.5f;      // Tiempo que dura la llama antes de desaparecer
    public float intervalo = 2f;       // Tiempo entre “explosiones” de la misma chimenea
    public Transform spawnPoint;       // Punto de salida, opcional si quieres reutilizar el prefab

    void Start()
    {
        // Llama la primera vez
        StartCoroutine(CicloLlamas());
    }

    System.Collections.IEnumerator CicloLlamas()
    {
        while (true) // Bucle infinito
        {
            // Instancia la llama (puede ser el mismo prefab o un efecto visual)
            GameObject llama = Instantiate(gameObject, spawnPoint.position, spawnPoint.rotation);

            // Destruye la llama después de 'duracion'
            Destroy(llama, duracion);

            // Espera 'intervalo' segundos antes de volver a disparar
            yield return new WaitForSeconds(intervalo);
        }
    }
}
