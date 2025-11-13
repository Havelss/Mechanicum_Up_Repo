using UnityEngine;

public class GatoAnimationController : MonoBehaviour
{
    [Header("Animator del Gato")]
    public Animator gatoAnimator; // Se puede arrastrar, pero también se autoasigna

    [Header("Comandos de la terminal")]
    public string commandUp = "up";
    public string commandDown = "no,up";

    [Header("Nombres de animaciones")]
    public string upAnimation = "Gato_Subida";
    public string downAnimation = "Gato_bajada";
    public string idleUpAnimation = "Gato_idle_arriba";
    public string idleDownAnimation = "Gato_Idle_Abajo";

    private void Awake()
    {
        // 🔹 Si no lo arrastras manualmente, busca el Animator en este objeto
        if (gatoAnimator == null)
        {
            gatoAnimator = GetComponent<Animator>();
            if (gatoAnimator == null)
                Debug.LogWarning("[GatoAnimationController] No se encontró Animator en este objeto.");
        }
    }

    public void ExecuteTerminalCommand(string command)
    {
        if (gatoAnimator == null)
        {
            Debug.LogWarning("[Gato] Animator no asignado!");
            return;
        }

        if (string.IsNullOrEmpty(command)) return;

        command = command.Trim().ToLower();
        Debug.Log($"[Gato] Comando recibido: {command}");

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
            Debug.LogWarning($"[Gato] Comando desconocido: {command}");
        }
    }

    private System.Collections.IEnumerator ReturnToIdle(string idleAnim, float delay)
    {
        yield return new WaitForSeconds(delay);
        gatoAnimator.Play(idleAnim);
    }
}
