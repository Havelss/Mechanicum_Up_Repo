using UnityEngine;
using System.Collections;

public class LanzallamasFade : MonoBehaviour
{
    public float tiempoAntesDeApagar = 1.2f;

    private ParticleSystem ps;

    void Awake()
    {
        // Busca el ParticleSystem en este objeto o en hijos
        ps = GetComponentInChildren<ParticleSystem>();

        if (ps == null)
        {
            Debug.LogError("No se encontró ParticleSystem en el lanzallamas.", this);
        }
    }

    void Start()
    {
        if (ps != null)
        {
            StartCoroutine(Apagar());
        }
    }

    IEnumerator Apagar()
    {
        // Espera el tiempo activo del fuego
        yield return new WaitForSeconds(tiempoAntesDeApagar);

        // Apaga la emisión (deja que las partículas mueran solas)
        var emission = ps.emission;
        emission.enabled = false;

        // Espera a que desaparezcan las partículas vivas
        yield return new WaitForSeconds(ps.main.startLifetime.constantMax);

        // Destruye el objeto completo
        Destroy(gameObject);
    }
}
