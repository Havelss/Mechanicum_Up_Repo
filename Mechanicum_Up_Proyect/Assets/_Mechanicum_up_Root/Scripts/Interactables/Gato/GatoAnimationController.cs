using UnityEngine;

/// <summary>
/// Controlador de animaciones específico para el Gato Mecánico.
/// Funciona con la terminal para ejecutar animaciones según comandos.
/// </summary>
public class GatoAnimationController : MonoBehaviour
{
    [Header("Animator del Gato")]
    public Animator gatoAnimator; // Aquí arrastras el Animator de PF_Gato_Rig_Final

    [Header("Comandos de la terminal")]
    public string commandUp = "up";       // Comando que hace que suba
    public string commandDown = "no,up";  // Comando que hace que baje

    [Header("Nombres de animaciones")]
    public string upAnimation = "MoveUp";
    public string downAnimation = "MoveDown";
    public string idleUpAnimation = "IdleUp";
    public string idleDownAnimation = "IdleDown";

    /// <summary>
    /// Llamar desde SymbolTerminalController.ExecuteSequence
    /// Ejecuta la animación correspondiente según el comando recibido
    /// </summary>
    /// <param name="command">Comando recibido desde la terminal</param>
    public void ExecuteTerminalCommand(string command)
    {
        if (gatoAnimator == null || string.IsNullOrEmpty(command)) return;

        command = command.Trim().ToLower();

        if (command == commandUp.ToLower())
        {
            // Ejecuta animación de subir y vuelve a IdleUp
            gatoAnimator.Play(upAnimation);
            StartCoroutine(ReturnToIdle(idleUpAnimation, gatoAnimator.GetCurrentAnimatorStateInfo(0).length));
        }
        else if (command == commandDown.ToLower())
        {
            // Ejecuta animación de bajar y vuelve a IdleDown
            gatoAnimator.Play(downAnimation);
            StartCoroutine(ReturnToIdle(idleDownAnimation, gatoAnimator.GetCurrentAnimatorStateInfo(0).length));
        }
        else
        {
            Debug.LogWarning($"[GatoAnimationController] Comando desconocido: {command}");
        }
    }

    /// <summary>
    /// Vuelve a la animación Idle después de la duración de la animación actual
    /// </summary>
    private System.Collections.IEnumerator ReturnToIdle(string idleAnim, float delay)
    {
        yield return new WaitForSeconds(delay);
        gatoAnimator.Play(idleAnim);
    }
}
