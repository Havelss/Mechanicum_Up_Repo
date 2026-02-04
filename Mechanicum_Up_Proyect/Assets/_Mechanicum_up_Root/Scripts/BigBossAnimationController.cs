using UnityEngine;
using System.Collections;

public class BigBossAnimationController : MonoBehaviour, I_Electrifiable
{
    [Header("Animator")]
    public Animator bossAnimator;

    [Header("Configuración de Tiempos")]
    public float crossFadeDuration = 0.25f;
    [Tooltip("Segundos que permanece en Idle_UP tras dejar de recibir electricidad")]
    public float powerOffDelay = 2.0f;

    [Header("Estados de Animación")]
    public string idleInitial = "Bigboss_idle";
    public string idleBase = "Bigboss_Idle_Down";
    public string idlePowered = "Bigboss_Idle_UP";

    [Header("Acciones")]
    public string upAction = "Bigboss_subir_sala";
    public string downAction = "Bigboss_bajar_sala";

    private Coroutine currentRoutine;
    private Coroutine powerOffRoutine; // Nueva rutina para el retardo

    private bool isCurrentlyPowered = false;
    private bool hasMovedOnce = false;

    private float lastPowerTime = 0f;
    private float powerCheckThreshold = 0.2f; // Margen para el PowerOn continuo

    private void Start()
    {
        if (bossAnimator != null) bossAnimator.Play(idleInitial);
    }

    private void Update()
    {
        // Detectamos si el aura ha dejado de tocar al Boss
        if (isCurrentlyPowered && Time.time > lastPowerTime + powerCheckThreshold)
        {
            // En lugar de apagar de golpe, iniciamos la cuenta atrás si no hay una ya
            if (powerOffRoutine == null)
            {
                powerOffRoutine = StartCoroutine(DelayedPowerOff());
            }
        }
    }

    // ==========================================
    // INTERFAZ I_Electrifiable
    // ==========================================
    public void PowerOn()
    {
        lastPowerTime = Time.time;

        // Si recibimos energía, cancelamos cualquier intento de apagado previo
        if (powerOffRoutine != null)
        {
            StopCoroutine(powerOffRoutine);
            powerOffRoutine = null;
        }

        if (!isCurrentlyPowered)
        {
            isCurrentlyPowered = true;
            PlaySmooth(idlePowered);
        }
    }

    public void PowerOff()
    {
        // Este método ahora lo llamamos internamente o por interfaz si fuera necesario
        if (!isCurrentlyPowered) return;

        isCurrentlyPowered = false;
        PlaySmooth(hasMovedOnce ? idleBase : idleInitial);
    }

    // Corrutina para el tiempo de espera extra
    private IEnumerator DelayedPowerOff()
    {
        yield return new WaitForSeconds(powerOffDelay);
        PowerOff();
        powerOffRoutine = null;
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
            hasMovedOnce = true;
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