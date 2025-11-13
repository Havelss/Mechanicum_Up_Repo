using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Puntos de movimiento")]
    [SerializeField] private Transform upperPoint;
    [SerializeField] private Transform lowerPoint;

    [Header("Ajustes del ascensor")]
    [SerializeField] private float speed = 2f;

    [Header("Animator de las puertas")]
    [SerializeField] private Animator doorAnimator;  // Un solo animator para las dos puertas

    [Header("Detección del jugador")]
    [SerializeField] private float playerDetectDistance = 3f;
    [SerializeField] private Transform player;

    // Nombres de animaciones
    private const string IDLE_OPEN = "Ascensor_Idle_Abierto";
    private const string IDLE_CLOSED = "Ascensor_Idle_Cerrado";
    private const string CLOSE = "Ascensor_Cerrar";

    private const string MOVE_UP = "Ascensor_Subir";
    private const string MOVE_DOWN = "Ascensor_Bajar";

    private bool movingUp = false;
    private bool movingDown = false;
    private bool playerNearby = false;

    private void Start()
    {
        PlayIdleClosed();
    }

    private void Update()
    {
        DetectPlayer();

        if (movingUp)
            MoveTowards(upperPoint, MOVE_UP);

        if (movingDown)
            MoveTowards(lowerPoint, MOVE_DOWN);
    }

    // ------------------------------------------
    // DETECCIÓN DE JUGADOR
    // ------------------------------------------
    private void DetectPlayer()
    {
        if (player == null || IsMoving()) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= playerDetectDistance)
        {
            if (!playerNearby)
            {
                playerNearby = true;
                PlayIdleOpen();
            }
        }
        else
        {
            if (playerNearby)
            {
                playerNearby = false;
                PlayIdleClosed();
            }
        }
    }

    // ------------------------------------------
    // MOVIMIENTO DEL ASCENSOR
    // ------------------------------------------
    private void MoveTowards(Transform target, string moveAnim)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Puertas cerradas + animación de movimiento
        PlayDoors(moveAnim);

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            movingUp = false;
            movingDown = false;

            if (playerNearby)
                PlayIdleOpen();
            else
                PlayIdleClosed();

            Debug.Log("Ascensor detenido.");
        }
    }

    public void MoveUp()
    {
        if (upperPoint == null) return;

        movingDown = false;
        movingUp = true;

        PlayClosing();
    }

    public void MoveDown()
    {
        if (lowerPoint == null) return;

        movingUp = false;
        movingDown = true;

        PlayClosing();
    }

    public bool IsMoving()
    {
        return movingUp || movingDown;
    }

    // ------------------------------------------
    // ANIMACIONES
    // ------------------------------------------
    private void Play(string anim)
    {
        if (doorAnimator != null)
            doorAnimator.Play(anim);
    }

    private void PlayIdleOpen() => Play(IDLE_OPEN);
    private void PlayIdleClosed() => Play(IDLE_CLOSED);
    private void PlayClosing() => Play(CLOSE);
    private void PlayDoors(string anim) => Play(anim);
}
