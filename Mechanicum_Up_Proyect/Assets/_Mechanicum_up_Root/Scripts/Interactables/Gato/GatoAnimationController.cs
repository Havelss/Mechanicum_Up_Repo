using UnityEngine;

/// <summary>
/// Controlador de animaciones específico para el Gato Mecánico.
/// Funciona con la terminal para ejecutar animaciones según comandos.
/// </summary>
public class GatoAnimationController : MonoBehaviour
{
    [Header("Animator del Gato")]
    public Animator gatoAnimator; // Arrastra aquí PF_Gato_Rig_Final

    [Header("Comandos de la terminal")]
    public string commandUp = "up";       // Comando que hace que suba
    public string commandDown = "no,up";  // Comando que hace que baje

    [Header("Nombres de animaciones")]
    public string upAnimation = "Gato_Subida";
    public string downAnimation = "Gato_bajada";
    public string idleUpAnimation = "Gato_idle_arriba";
    public string idleDownAnimation = "Gato_Idle_Abajo";

    public void ExecuteTerminalCommand(string command)
    {
        if (gatoAnimator == null || string.IsNullOrEmpty(command)) return;

        command = command.Trim().ToLower();

        if (command == commandUp.ToLower())
        {
            gatoAnimator.Play(upAnimation);
            StartCoroutine(ReturnToIdle(idleUpAnimation, gatoAnimator.GetCurrentAnimatorStateInfo(0).length));
        }
        else if (command == commandDown.ToLower())
        {
            gatoAnimator.Play(downAnimation);
            StartCoroutine(ReturnToIdle(idleDownAnimation, gatoAnimator.GetCurrentAnimatorStateInfo(0).length));
        }
        else
        {
            Debug.LogWarning($"[GatoAnimationController] Comando desconocido: {command}");
        }
    }

    private System.Collections.IEnumerator ReturnToIdle(string idleAnim, float delay)
    {
        yield return new WaitForSeconds(delay);
        gatoAnimator.Play(idleAnim);
    }
}
