using UnityEngine;

public class SierraAnimador : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Animaciones")]
    public string animacion1 = "Animacion1";
    public string animacion2 = "Animacion2";
    public string animacion3 = "Animacion3";

    [Header("Tiempo entre animaciones")]
    public float tiempoAnimacion1 = 2f;
    public float tiempoAnimacion2 = 3f;
    public float tiempoAnimacion3 = 3f;

    private float timer;
    private int indiceAnim = 0;

    [Header("Collider")]
    public BoxCollider boxCollider;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Iniciar con la primera animación
        indiceAnim = 0;
        PlayActual();

        if (boxCollider == null) 
            boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Avanzar al siguiente índice (0 -> 1 -> 2 -> 0 ...)
            indiceAnim = (indiceAnim + 1) % 3;
            PlayActual();
        }
    }

    private void PlayActual()
    {
        switch (indiceAnim)
        {
            case 0:
                animator.Play(animacion1);
                timer = tiempoAnimacion1;
                boxCollider.enabled = true;
                break;
            case 1:
                animator.Play(animacion2);
                timer = tiempoAnimacion2;
                boxCollider.enabled = true;
                break;
            case 2:
                animator.Play(animacion3);
                timer = tiempoAnimacion3;
                boxCollider.enabled = false;
                break;
        }
    }
}
