using UnityEngine;
using System.Collections;

public class PistonLoop : MonoBehaviour
{
    [Header("Animator del Piston")]
    public Animator animator;

    [Header("Animaciones")]
    public string animIdle = "Armature_Piston_Idle";
    public string animDown = "Armature_Piston_Down";
    public string animUp = "Armature_Piston_Up";

    [Header("Duración de cada animación")]
    public float tiempoIdle = 1f;
    public float tiempoDown = 1f;
    public float tiempoUp = 1f;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        StartCoroutine(CicloAnimaciones());
    }

    private IEnumerator CicloAnimaciones()
    {
        while (true)
        {
            // 1️⃣ Idle
            animator.Play(animIdle);
            yield return new WaitForSeconds(tiempoIdle);

            // 2️⃣ Down
            animator.Play(animDown);
            yield return new WaitForSeconds(tiempoDown);

            // 3️⃣ Up
            animator.Play(animUp);
            yield return new WaitForSeconds(tiempoUp);
        }
    }
}
