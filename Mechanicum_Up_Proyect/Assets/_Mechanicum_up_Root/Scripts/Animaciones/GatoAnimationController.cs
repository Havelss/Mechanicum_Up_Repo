using UnityEngine;
using System.Collections;

public class GatoAnimationController : MonoBehaviour
{
    [Header("Animator del Gato")]
    public Animator gatoAnimator;

    [Header("Comandos de la terminal")]
    public string commandUp = "up";
    public string commandDown = "no,up";

    [Header("Nombres de animaciones")]
    public string upAnimation = "Gato_Subida";
    public string downAnimation = "Gato_bajada";
    public string idleUpAnimation = "Gato_idle_arriba";
    public string idleDownAnimation = "Gato_Idle_Abajo";

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (gatoAnimator == null)
        {
            gatoAnimator = GetComponent<Animator>();
            if (gatoAnimator == null)
                Debug.LogWarning("[GatoAnimationController] No se encontró Animator en este objeto.");
        }
    }

    public void ExecuteTerminalCommand(string command)
    {
        if (gatoAnimator == null) return;
        if (string.IsNullOrEmpty(command)) return;

        command = command.Trim().ToLower();
        Debug.Log($"[Gato] Comando recibido: {command}");

        if (command == commandUp.ToLower())
        {
            PlayWithReturn(upAnimation, idleUpAnimation);
        }
        else if (command == commandDown.ToLower())
        {
            PlayWithReturn(downAnimation, idleDownAnimation);
        }
        else
        {
            Debug.LogWarning($"[Gato] Comando desconocido: {command}");
        }
    }

    private void PlayWithReturn(string anim, string idle)
    {
        // Cancelar una animación anterior que aún no ha terminado
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        gatoAnimator.Play(anim);

        float clipLength = GetClipLength(anim);

        currentRoutine = StartCoroutine(ReturnToIdle(idle, clipLength));
    }

    private float GetClipLength(string clipName)
    {
        foreach (var clip in gatoAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }

        Debug.LogWarning($"[Gato] No se encontró el clip {clipName}");
        return 0.1f;
    }

    private IEnumerator ReturnToIdle(string idleAnim, float delay)
    {
        yield return new WaitForSeconds(delay);
        gatoAnimator.Play(idleAnim);
    }
}
