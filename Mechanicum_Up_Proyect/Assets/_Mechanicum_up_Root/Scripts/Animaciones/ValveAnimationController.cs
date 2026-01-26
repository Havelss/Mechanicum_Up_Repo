using UnityEngine;
using System.Collections;

public class ValveAnimationController : MonoBehaviour
{
    [Header("Animator de la válvula")]
    public Animator valveAnimator;

    [Header("Nombres de animaciones")]
    public string idleStart = "Palanca_Arriba_Idle";
    public string rotateRight = "Palanca_Down";
    public string idleEnd = "Palanca_Down_Idle";
    public string rotateLeft = "Palanca_Arriba";

    private Coroutine returnCoroutine;
    private bool lastRotationWasRight = false; // 👉 Controla la dirección anterior

    public void PlayValveRotation()
    {
        if (valveAnimator == null)
        {
            Debug.LogWarning("[ValveAnimationController] No hay Animator asignado.");
            return;
        }

        // Alternar la dirección
        lastRotationWasRight = !lastRotationWasRight;

        string playAnim = lastRotationWasRight ? rotateRight : rotateLeft;
        string idleToPlay = lastRotationWasRight ? idleEnd : idleStart;

        // Buscar duración del clip
        float clipLength = GetClipLengthByName(playAnim);
        if (clipLength <= 0f)
        {
            clipLength = 1f;
            Debug.LogWarning("[ValveAnimationController] No se encontró duración del clip: " + playAnim);
        }

        // Reproducir animación
        valveAnimator.Play(playAnim, 0, 0);

        // Cancelar coroutine anterior si existía
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        // Esperar hasta que termine para volver al idle
        returnCoroutine = StartCoroutine(ReturnToIdle(idleToPlay, clipLength + 0.05f));
    }

    private IEnumerator ReturnToIdle(string idleAnim, float delay)
    {
        yield return new WaitForSeconds(delay);
        valveAnimator.Play(idleAnim);
        returnCoroutine = null;
    }

    private float GetClipLengthByName(string clipName)
    {
        if (valveAnimator == null || valveAnimator.runtimeAnimatorController == null)
            return 0f;

        foreach (var clip in valveAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length / valveAnimator.speed;
        }

        return 0f;
    }
}
