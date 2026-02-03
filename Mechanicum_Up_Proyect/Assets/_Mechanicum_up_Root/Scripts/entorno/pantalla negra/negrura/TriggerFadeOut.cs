using UnityEngine;
using System.Collections;

public class TriggerFadeOut : MonoBehaviour
{
    [Header("Configuración Fade")]
    [SerializeField] private float duracionFade = 0.5f;

    [Header("Configuración Movimiento")]
    [SerializeField] private bool forzarPaso = true;
    [SerializeField] private Transform puntoDeSalida;
    [SerializeField] private float velocidadEmpuje = 3.0f;

    private MonoBehaviour scriptMovimientoEncontrado;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scriptMovimientoEncontrado = other.GetComponent<PlayerController>();

            StartCoroutine(ScreenFader.Instance.FadeOut(duracionFade));

            if (forzarPaso && puntoDeSalida != null)
            {
                StartCoroutine(MoverYBloquear(other.transform));
            }
        }
    }

    private IEnumerator MoverYBloquear(Transform player)
    {
        if (scriptMovimientoEncontrado != null) scriptMovimientoEncontrado.enabled = false;

        float timeout = 0f;
        while (Vector3.Distance(player.position, puntoDeSalida.position) > 0.1f && timeout < 1.2f)
        {
            player.position = Vector3.MoveTowards(player.position, puntoDeSalida.position, velocidadEmpuje * Time.deltaTime);
            timeout += Time.deltaTime;
            yield return null;
        }

        player.position = puntoDeSalida.position;

        if (scriptMovimientoEncontrado != null) scriptMovimientoEncontrado.enabled = true;
    }
}