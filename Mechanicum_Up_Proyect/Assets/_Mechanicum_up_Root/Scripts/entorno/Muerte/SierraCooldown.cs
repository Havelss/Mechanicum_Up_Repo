using UnityEngine;

public class SierraCooldown : MonoBehaviour
{
    [Header("Animator de la Sierra")]
    public Animator sierraAnimator;

    [Header("Animación que activa la sierra")]
    public string animacionActiva = "Armature_Corte_Up";

    [Header("Tiempo de cooldown entre activaciones (segundos)")]
    public float cooldown = 2f;

    private bool puedeActivar = true;
    private float timer = 0f;

    void Update()
    {
        if (sierraAnimator == null)
            sierraAnimator = GetComponentInParent<Animator>();

        if (sierraAnimator == null) return;

        // Revisar si la animación actual es la que activa la sierra
        AnimatorStateInfo stateInfo = sierraAnimator.GetCurrentAnimatorStateInfo(0);
        bool enAnimacionCorrecta = stateInfo.IsName(animacionActiva);

        if (enAnimacionCorrecta)
        {
            // Si está en la animación y el cooldown terminó
            if (puedeActivar)
            {
                ActivarSierra();
                puedeActivar = false;
                timer = cooldown;
            }
        }
        else
        {
            // Si cambia de animación, la sierra puede activarse otra vez
            puedeActivar = true;
        }

        // Contar cooldown
        if (!puedeActivar)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                puedeActivar = true;
            }
        }
    }

    private void ActivarSierra()
    {
        // Aquí puedes poner efectos visuales, sonido o activar el InstantKillZone
        // Por ejemplo:
        Debug.Log("Sierra activada en animación: " + animacionActiva);

        // Si quieres, podrías habilitar/deshabilitar un collider
        // Collider c = GetComponent<Collider>();
        // if (c != null) c.enabled = true;
    }
}
