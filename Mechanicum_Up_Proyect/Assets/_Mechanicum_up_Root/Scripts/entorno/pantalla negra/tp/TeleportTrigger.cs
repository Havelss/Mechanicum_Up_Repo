using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Configuración del TP")]
    [SerializeField] private Transform destinoTP;
    [SerializeField] private float duracionFade = 0.5f;

    private bool procesando = false;

    private void OnTriggerEnter(Collider other)
    {
        // Detectamos al jugador
        if (other.CompareTag("Player") && !procesando)
        {
            // Buscamos el script específicamente por su nombre de clase
            PlayerController scriptMov = other.GetComponent<PlayerController>();
            CharacterController cc = other.GetComponent<CharacterController>();

            StartCoroutine(SecuenciaTeleport(other.transform, scriptMov, cc));
        }
    }

    private IEnumerator SecuenciaTeleport(Transform player, PlayerController script, CharacterController cc)
    {
        procesando = true;

        // 1. BLOQUEO TOTAL
        // Desactivamos el script de control y el componente físico
        if (script != null) script.enabled = false;
        if (cc != null) cc.enabled = false;

        // 2. FUNDIDO A NEGRO
        // Usamos el FadeOut y esperamos a que termine
        yield return StartCoroutine(ScreenFader.Instance.FadeOut(duracionFade));

        // 3. TELETRANSPORTE
        if (destinoTP != null)
        {
            player.position = destinoTP.position;
            player.rotation = destinoTP.rotation;
        }

        // Breve pausa para que la cámara y las físicas se estabilicen en la nueva posición
        yield return new WaitForSeconds(0f);

        // 4. ACLARAR PANTALLA
        yield return StartCoroutine(ScreenFader.Instance.FadeIn(duracionFade));

        // 5. DEVOLVER EL CONTROL
        if (cc != null) cc.enabled = true;
        if (script != null) script.enabled = true;

        procesando = false;
    }
}