using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning($"[{name}] No se encontró Animator en el GameObject.");
    }

    /// <summary>
    /// Reproduce una animación por nombre (trigger o clip) y opcionalmente vuelve a Idle.
    /// </summary>
    /// <param name="animName">Animación a reproducir.</param>
    /// <param name="idleName">Animación a reproducir al terminar o cuando se detenga.</param>
    /// <param name="duration">Opcional, tiempo de duración si no se usa trigger.</param>
    public void PlayAnimation(string animName, string idleName = "Idle", float duration = 0f)
    {
        if (animator == null) return;

        animator.Play(animName);

        // Si se especifica un duration, vuelve a idle después de ese tiempo
        if (duration > 0f)
        {
            StopAllCoroutines();
            StartCoroutine(ReturnToIdleAfter(duration, idleName));
        }
    }

    /// <summary>
    /// Activa un trigger del Animator y opcionalmente vuelve a Idle después de la duración.
    /// </summary>
    public void PlayTrigger(string triggerName, string idleName = "Idle", float duration = 0f)
    {
        if (animator == null) return;

        animator.ResetTrigger(triggerName);
        animator.SetTrigger(triggerName);

        if (duration > 0f)
        {
            StopAllCoroutines();
            StartCoroutine(ReturnToIdleAfter(duration, idleName));
        }
    }

    /// <summary>
    /// Corrutina para volver a Idle después de un tiempo
    /// </summary>
    private IEnumerator ReturnToIdleAfter(float duration, string idleName)
    {
        yield return new WaitForSeconds(duration);
        if (animator != null && !string.IsNullOrEmpty(idleName))
            animator.Play(idleName);
    }

    /// <summary>
    /// Cambia un parámetro booleano
    /// </summary>
    public void SetBool(string param, bool state)
    {
        if (animator == null) return;
        animator.SetBool(param, state);
    }
}
