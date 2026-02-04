using UnityEngine;
using System.Collections;

public class BigBossAnimationController : MonoBehaviour
{
    [Header("Animator del BigBoss")]
    public Animator bossAnimator;

    [Header("Comandos de la terminal")]
    public string commandUp = "up";
    public string commandDown = "down";

    [Header("Nombres de animaciones")]
    public string upAnimation = "Bigboss_subir_sala";
    public string downAnimation = "Bigboss_bajar_sala"; // Animación de bajar
    public string idleUpAnimation = "Bigboss_Idle_UP";
    public string idleDownAnimation = "Bigboss_Idle_Down";
    public string idleDefault = "Bigboss_idle";

    private Coroutine currentRoutine;

    // 🔌 Electricidad
    private bool receivingElectricity = false;

    private void Awake()
    {
        if (bossAnimator == null)
        {
            bossAnimator = GetComponent<Animator>();
            if (bossAnimator == null)
                Debug.LogWarning("[BigBossAnimationController] No se encontró Animator.");
        }
    }

    private void Update()
    {
        // Si recibe electricidad, mostrar animación idle correspondiente
        if (receivingElectricity)
        {
            PlayInstant(idleUpAnimation); // puedes cambiar a idleDownAnimation según tu lógica
            receivingElectricity = false; // se resetea cada frame para evitar repetición
        }
    }

    public void ExecuteTerminalCommand(string command)
    {
        if (bossAnimator == null) return;
        if (string.IsNullOrEmpty(command)) return;

        command = command.Trim().ToLower();
        Debug.Log($"[BigBoss] Comando recibido: {command}");

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
            Debug.LogWarning($"[BigBoss] Comando desconocido: {command}");
        }
    }

    private void PlayWithReturn(string anim, string idle)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        bossAnimator.Play(anim);

        float clipLength = GetClipLength(anim);
        currentRoutine = StartCoroutine(ReturnToIdle(idle, clipLength));
    }

    private float GetClipLength(string clipName)
    {
        foreach (var clip in bossAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }

        Debug.LogWarning($"[BigBoss] No se encontró el clip {clipName}");
        return 0.1f;
    }

    private IEnumerator ReturnToIdle(string idleAnim, float delay)
    {
        yield return new WaitForSeconds(delay);
        bossAnimator.Play(idleAnim);
    }

    public void PlayInstant(string anim)
    {
        if (bossAnimator == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        bossAnimator.Play(anim);
    }

    // =========================
    // NUEVO: sistema de electricidad
    // =========================
    public void PowerOn(bool upIdle = true)
    {
        receivingElectricity = true;
        PlayInstant(upIdle ? idleUpAnimation : idleDownAnimation);
    }

    public void PowerOff()
    {
        // Opcional: podrías poner idleDefault al apagar
        PlayInstant(idleDefault);
    }
}
