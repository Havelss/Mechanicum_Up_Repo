using UnityEngine;

public class SierraAnimador : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Animaciones")]
    public string animacion1 = "Animacion1";
    public string animacion2 = "Animacion2";

    [Header("Tiempo entre animaciones")]
    public float tiempoAnimacion1 = 2f;
    public float tiempoAnimacion2 = 3f;

    private float timer;
    private bool enAnimacion1 = true;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Iniciar con la primera animación
        animator.Play(animacion1);
        timer = tiempoAnimacion1;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Cambiar de animación
            if (enAnimacion1)
            {
                animator.Play(animacion2);
                timer = tiempoAnimacion2;
            }
            else
            {
                animator.Play(animacion1);
                timer = tiempoAnimacion1;
            }

            enAnimacion1 = !enAnimacion1;
        }
    }
}
