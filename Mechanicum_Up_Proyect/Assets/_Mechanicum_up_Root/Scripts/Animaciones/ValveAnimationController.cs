using UnityEngine;

public class ValveAnimationController : MonoBehaviour
{
    [Header("Referencias")]
    public Animator valveAnimator;
    public Elevator controlledElevator;

    [Header("Configuración de Luces (Renderers)")]
    public Renderer luzRojaPalanca;
    public Renderer luzVerdePalanca;
    [ColorUsage(true, true)] public Color colorRojoEmisivo = Color.red;
    [ColorUsage(true, true)] public Color colorVerdeEmisivo = Color.green;

    [Header("Nombres de animaciones")]
    public string idleStart = "Palanca_Arriba_Idle";
    public string downAnimation = "Palanca_Down";
    public string idleEnd = "Palanca_Down_Idle";
    public string upAnimation = "Palanca_Arriba";

    private bool lastRotationWasRight = false;
    private bool wasMovingLastFrame = false;

    private void Start()
    {
        // Estado inicial forzado
        ActualizarVisualesLuces(false);
    }

    private void Update()
    {
        if (controlledElevator == null) return;

        bool isMovingNow = controlledElevator.IsMoving();

        // Si el estado de movimiento ha cambiado desde el último frame...
        if (isMovingNow != wasMovingLastFrame)
        {
            ActualizarVisualesLuces(isMovingNow);

            // Si se acaba de detener, ponemos la animación de IDLE correspondiente
            if (!isMovingNow)
            {
                string idleAnim = lastRotationWasRight ? idleEnd : idleStart;
                if (valveAnimator != null) valveAnimator.Play(idleAnim, 0, 0);
            }

            wasMovingLastFrame = isMovingNow;
        }
    }

    // Se llama desde ElevatorCall
    public void PlayValveAnimationForElevator()
    {
        if (valveAnimator == null) return;

        lastRotationWasRight = !lastRotationWasRight;
        string moveAnim = lastRotationWasRight ? upAnimation : downAnimation;

        valveAnimator.Play(moveAnim, 0, 0);
    }

    private void ActualizarVisualesLuces(bool estaMoviendose)
    {
        // Si se mueve: Rojo ON (true), Verde OFF (false)
        // Si está quieto: Rojo OFF (false), Verde ON (true)
        SetEmission(luzRojaPalanca, colorRojoEmisivo, estaMoviendose);
        SetEmission(luzVerdePalanca, colorVerdeEmisivo, !estaMoviendose);
    }

    private void SetEmission(Renderer targetRenderer, Color color, bool state)
    {
        if (targetRenderer == null) return;

        // .material crea una instancia única para que no cambien todas las palancas del juego a la vez
        Material mat = targetRenderer.material;

        if (state)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", color);
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black);
            mat.DisableKeyword("_EMISSION");
        }
    }

    public float GetClipLengthByName(string clipName)
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