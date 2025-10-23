using UnityEngine;
/*
public class Elevator : MonoBehaviour
{
    public static Elevator Instance;
    [SerializeField] private Transform topPosition;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private float speed = 2f;

    private Transform target;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    #region Limites arriba/abajo

    public void MoveUp()
    {
        Debug.Log("Ascensor subiendo...");
        target = topPosition;
    }

    public void MoveDown()
    {
        Debug.Log("Ascensor bajando...");
        target = groundPosition;
    }

    #endregion
}

*/


public class Elevator : MonoBehaviour
{
    [Header("Posiciones del ascensor")]
    [SerializeField] private Transform bottomPoint;
    [SerializeField] private Transform topPoint;

    [Header("Configuración")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool startAtTop = false;

    private bool isMoving = false;
    private bool goingUp = false;

    private Transform target;

    private void Start()
    {
        // Al iniciar, define la posición inicial del ascensor
        if (startAtTop && topPoint != null)
            transform.position = topPoint.position;
        else if (!startAtTop && bottomPoint != null)
            transform.position = bottomPoint.position;
    }

    private void Update()
    {
        if (isMoving && target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.unscaledDeltaTime // usa unscaled para funcionar aunque el tiempo esté pausado
            );

            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                isMoving = false;
                Debug.Log($"Ascensor '{name}' llegó a destino.");
            }
        }
    }

    public void MoveUp()
    {
        if (isMoving || topPoint == null) return;

        target = topPoint;
        isMoving = true;
        goingUp = true;
        Debug.Log($"Ascensor '{name}' subiendo.");
    }

    public void MoveDown()
    {
        if (isMoving || bottomPoint == null) return;

        target = bottomPoint;
        isMoving = true;
        goingUp = false;
        Debug.Log($"Ascensor '{name}' bajando.");
    }

    public bool IsAtTop()
    {
        return Vector3.Distance(transform.position, topPoint.position) < 0.05f;
    }

    public bool IsAtBottom()
    {
        return Vector3.Distance(transform.position, bottomPoint.position) < 0.05f;
    }
}

