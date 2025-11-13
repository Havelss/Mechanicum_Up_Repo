using UnityEngine;
using System.Collections;

public class ValveAnimationController : MonoBehaviour
{
    [Header("Animator de la válvula")]
    public Animator valveAnimator; // ← Aquí arrastras el componente Animator del objeto SK_Valvula_Rig_Final

    [Header("Nombres de animaciones")]
    public string idleStart = "Valvula_Idle_0G";              // Idle inicial
    public string rotateRight = "Valvula_Rotacion_Derecha";   // Girar a la derecha
    public string idleEnd = "Valvula_Idle_360G";              // Idle tras giro completo
    public string rotateLeft = "Valvula_Rotacion_Izquierda";  // Girar a la izquierda

    /// <summary>
    /// Ejecuta la animación de rotación según si el ascensor sube o baja.
    /// </summary>
    public void PlayValveRotation(bool goingUp)
    {
        if (valveAnimator == null)
        {
            Debug.LogWarning("[ValveAnimationController] No hay Animator asignado.");
            return;
        }

        if (goingUp)
        {
            valveAnimator.Play(rotateLeft);
            StartCoroutine(ReturnToIdle(idleStart, valveAnimator.GetCurrentAnimatorStateInfo(0).length));

            
        }
        else
        {
            valveAnimator.Play(rotateRight);
            StartCoroutine(ReturnToIdle(idleEnd, valveAnimator.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    private IEnumerator ReturnToIdle(string idleAnim, float delay)
    {
        yield return new WaitForSeconds(delay);
        valveAnimator.Play(idleAnim);
    }
}
