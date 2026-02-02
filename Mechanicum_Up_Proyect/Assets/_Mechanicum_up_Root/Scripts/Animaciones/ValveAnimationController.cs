using UnityEngine;
using System.Collections;

public class ValveAnimationController : MonoBehaviour
{
    [Header("Animator de la válvula")]
    public Animator valveAnimator;

    [Header("Nombres de animaciones")]
    public string idleStart = "Palanca_Arriba_Idle"; // idle arriba
    public string downAnimation = "Palanca_Down";      // bajar palanca
    public string idleEnd = "Palanca_Down_Idle";     // idle abajo
    public string upAnimation = "Palanca_Arriba";     // subir palanca

    private Coroutine returnCoroutine;
    private bool lastRotationWasRight = false; // controla dirección anterior

    // Llamar cuando se interactúa con la palanca para mover el ascensor
    public void PlayValveAnimationForElevator(Elevator elevator)
    {
        if (valveAnimator == null || elevator == null)
        {
            Debug.LogWarning("[ValveAnimationController] Animator o Elevator no asignados");
            return;
        }

        // Alternar dirección: sube o baja
        lastRotationWasRight = !lastRotationWasRight;

        string moveAnim = lastRotationWasRight ? upAnimation : downAnimation;
        string idleAnim = lastRotationWasRight ? idleEnd : idleStart;

        // Reproducir animación de mover palanca
        valveAnimator.Play(moveAnim, 0, 0);

        // Cancelar coroutine anterior si existe
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        // Esperar a que el ascensor termine para poner idle
        returnCoroutine = StartCoroutine(WaitForElevatorAndSetIdle(elevator, idleAnim));
    }

    private IEnumerator WaitForElevatorAndSetIdle(Elevator elevator, string idleAnim)
    {
        while (elevator.IsMoving())
            yield return null;

        valveAnimator.Play(idleAnim, 0, 0);
        returnCoroutine = null;
    }

    // Función auxiliar para obtener duración de clips (opcional, si quieres medir tiempos)
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
