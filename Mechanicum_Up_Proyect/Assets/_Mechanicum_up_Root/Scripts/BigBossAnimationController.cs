using UnityEngine;
using System.Collections;

public class BigBossAnimationController : MonoBehaviour, I_Electrifiable
{
    [Header("Animator")]
    public Animator bossAnimator;

    [Header("Configuración")]
    public float crossFadeDuration = 0.25f;

    [Header("Estados de Animación")]
    public string idleInitial = "Bigboss_idle";      // El estado al empezar el juego
    public string idleBase = "Bigboss_Idle_Down";    // El estado base tras activarse
    public string idlePowered = "Bigboss_Idle_UP";   // El estado con energía

    [Header("Acciones")]
    public string upAction = "Bigboss_subir_sala";
    public string downAction = "Bigboss_bajar_sala";

    private Coroutine currentRoutine;
    private bool isCurrentlyPowered = false;
    private bool hasMovedOnce = false; // Controla si ya cambió su estado base

    private float lastPowerTime = 0f;
    private float powerTimeout = 0.2f;

    private void Start()
    {
        if (bossAnimator != null)
        {
            // Empezamos en el idle de reposo absoluto
            bossAnimator.Play(idleInitial);
        }
    }

    private void Update()
    {
        if (isCurrentlyPowered && Time.time > lastPowerTime + powerTimeout)
        {
            PowerOff();
        }
    }

    // ==========================================
    // INTERFAZ I_Electrifiable
    // ==========================================
    public void PowerOn()
    {
        lastPowerTime = Time.time;
        if (!isCurrentlyPowered)
        {
            isCurrentlyPowered = true;
            PlaySmooth(idlePowered);
        }
    }

    public void PowerOff()
    {
        if (!isCurrentlyPowered) return;
        isCurrentlyPowered = false;

        // Si ya se ha movido alguna vez, vuelve a Idle_Down. 
        // Si no, vuelve al inicial (aunque lo lógico es que ya se haya movido)
        PlaySmooth(hasMovedOnce ? idleBase : idleInitial);
    }

    // ==========================================
    // COMANDOS DE TERMINAL
    // ==========================================
    public void ExecuteTerminalCommand(string command)
    {
        if (isCurrentlyPowered) return;

        command = command.Trim().ToLower();

        if (command == "up")
        {
            hasMovedOnce = true; // A partir de aquí, su "casa" es Idle_Down
            PlayActionThenReturn(upAction, idleBase);
        }
        else if (command == "down")
        {
            PlayActionThenReturn(downAction, idleBase);
        }
    }

    // ==========================================
    // MOTORES DE ANIMACIÓN
    // ==========================================

    public void PlaySmooth(string animName)
    {
        if (bossAnimator == null) return;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        bossAnimator.CrossFadeInFixedTime(animName, crossFadeDuration);
    }

    private void PlayActionThenReturn(string actionAnim, string returnIdle)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        bossAnimator.CrossFadeInFixedTime(actionAnim, crossFadeDuration);

        float length = GetClipLength(actionAnim);
        currentRoutine = StartCoroutine(WaitAndTransition(returnIdle, length));
    }

    private float GetClipLength(string clipName)
    {
        if (bossAnimator.runtimeAnimatorController == null) return 1f;
        foreach (var clip in bossAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName) return clip.length;
        }
        return 1.0f;
    }

    private IEnumerator WaitAndTransition(string targetAnim, float delay)
    {
        yield return new WaitForSeconds(delay - crossFadeDuration);
        if (!isCurrentlyPowered)
        {
            bossAnimator.CrossFadeInFixedTime(targetAnim, crossFadeDuration);
        }
    }
}