using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Configuración del ascensor")]
    [SerializeField] private Transform upperPoint;     // Punto superior
    [SerializeField] private Transform lowerPoint;     // Punto inferior
    [SerializeField] private float speed = 2f;         // Velocidad de movimiento

    private bool movingUp = false;
    private bool movingDown = false;

    private void Update()
    {
        if (movingUp)
        {
            MoveTowards(upperPoint);
        }
        else if (movingDown)
        {
            MoveTowards(lowerPoint);
        }
    }

    private void MoveTowards(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("No se ha asignado un punto de destino al ascensor.");
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Si llega al punto destino, detenemos el movimiento
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            movingUp = false;
            movingDown = false;
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

    // Puedes llamar a este método desde el terminal global
    public void StopElevator()
    {
        movingUp = false;
        movingDown = false;
        Debug.Log("Ascensor detenido manualmente.");
    }
}

