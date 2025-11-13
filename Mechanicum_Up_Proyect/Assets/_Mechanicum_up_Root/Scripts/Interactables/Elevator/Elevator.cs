using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Configuración del ascensor")]
    [SerializeField] private Transform upperPoint;
    [SerializeField] private Transform lowerPoint;
    [SerializeField] private float speed = 2f;
    [SerializeField] private AnimationController elevatorAnim; // Controlador de animación del ascensor

    private bool movingUp = false;
    private bool movingDown = false;

    private void Update()
    {
        if (movingUp)
        {
            MoveTowards(upperPoint, "MoveUp");
        }
        else if (movingDown)
        {
            MoveTowards(lowerPoint, "MoveDown");
        }
    }

    private void MoveTowards(Transform target, string animName)
    {
        if (target == null)
        {
            Debug.LogWarning("No se ha asignado un punto de destino al ascensor.");
            return;
        }

        // Reproduce animación mientras se mueve, vuelve a Idle al detenerse
        if (elevatorAnim != null)
            elevatorAnim.PlayAnimation(animName, "Idle");

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Si llega al destino, detén el movimiento
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            movingUp = false;
            movingDown = false;
            if (elevatorAnim != null)
                elevatorAnim.PlayAnimation("Idle"); // Asegura Idle
            Debug.Log($"Ascensor detenido en {target.name}");
        }
    }

    public void MoveUp()
    {
        if (upperPoint == null)
        {
            Debug.LogWarning("El ascensor no tiene asignado un punto superior.");
            return;
        }

        movingDown = false;
        movingUp = true;
        Debug.Log("Ascensor subiendo...");
    }

    public void MoveDown()
    {
        if (lowerPoint == null)
        {
            Debug.LogWarning("El ascensor no tiene asignado un punto inferior.");
            return;
        }

        movingUp = false;
        movingDown = true;
        Debug.Log("Ascensor bajando...");
    }

    public void StopElevator()
    {
        movingUp = false;
        movingDown = false;

        if (elevatorAnim != null)
            elevatorAnim.PlayAnimation("Idle");

        Debug.Log("Ascensor detenido manualmente.");
    }

    public bool IsMoving()
    {
        return movingUp || movingDown;
    }
}
