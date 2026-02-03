using UnityEngine;
using System.Collections;

public class TriggerFadeIn : MonoBehaviour
{
    [Header("Configuración Fade")]
    [SerializeField] private float duracionFade = 0.5f;

    [Header("Configuración Movimiento")]
    [SerializeField] private bool forzarPaso = true;
    [SerializeField] private Transform puntoDeSalida;
    [SerializeField] private float velocidadEmpuje = 3.0f;

    // Ya no necesitamos la casilla en el Inspector para el script, lo busca solo
    private MonoBehaviour scriptMovimientoEncontrado;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Buscamos el script de movimiento en el clon que entró
            // IMPORTANTE: Cambia "MonoBehaviour" por el nombre de tu script (ej: PlayerController)
            scriptMovimientoEncontrado = other.GetComponent<PlayerController>();

            StartCoroutine(ScreenFader.Instance.FadeIn(duracionFade));

            if (forzarPaso && puntoDeSalida != null)
            {
                StartCoroutine(MoverYBloquear(other.transform));
            }
        }
    }

    private IEnumerator MoverYBloquear(Transform player)
    {
        // Desactivamos el script si lo encontramos
        if (scriptMovimientoEncontrado != null) scriptMovimientoEncontrado.enabled = false;

        float timeout = 0f;
        while (Vector3.Distance(player.position, puntoDeSalida.position) > 0.1f && timeout < 1.2f)
        {
            player.position = Vector3.MoveTowards(player.position, puntoDeSalida.position, velocidadEmpuje * Time.deltaTime);
            timeout += Time.deltaTime;
            yield return null;
        }

        player.position = puntoDeSalida.position;

        // Lo reactivamos
        if (scriptMovimientoEncontrado != null) scriptMovimientoEncontrado.enabled = true;
    }
}