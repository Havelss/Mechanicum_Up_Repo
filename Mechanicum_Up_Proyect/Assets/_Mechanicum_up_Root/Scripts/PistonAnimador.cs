using UnityEngine;
using System.Collections;

public class PistonLoopSeguro : MonoBehaviour
{
    public Animator animator;

    [Header("Animaciones")]
    public string animIdle = "Armature_Piston_Idle";
    public string animDown = "Armature_Piston_Down";
    public string animUp = "Armature_Piston_Up";

    [Header("Duración de cada animación")]
    public float tiempoIdle = 1f;
    public float tiempoDown = 1f;
    public float tiempoUp = 1f;

    private bool primerIdle = true; // Solo se reproduce Idle una vez
    private bool siguienteUp = false; // Alterna Down / Up

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Inicia Idle solo una vez
        if (animator.HasState(0, Animator.StringToHash(animIdle)))
        {
            animator.Play(animIdle);
            StartCoroutine(EsperarIdle());
        }
        else
        {
            // Si no existe Idle, comienza directamente con Down
            StartCoroutine(CicloDownUp());
        }
    }

    private IEnumerator EsperarIdle()
    {
        yield return new WaitForSeconds(tiempoIdle);
        primerIdle = false;
        StartCoroutine(CicloDownUp());
    }

    private IEnumerator CicloDownUp()
    {
        while (true)
        {
            if (siguienteUp)
            {
                if (animator.HasState(0, Animator.StringToHash(animUp)))
                    animator.Play(animUp);
                yield return new WaitForSeconds(tiempoUp);
            }
            else
            {
                if (animator.HasState(0, Animator.StringToHash(animDown)))
                    animator.Play(animDown);
                yield return new WaitForSeconds(tiempoDown);
            }

            siguienteUp = !siguienteUp; // Alterna Down / Up
        }
    }
}
