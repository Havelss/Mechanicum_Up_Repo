using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    [Header("Puntos de movimiento")]
    [SerializeField] private Transform upperPoint;
    [SerializeField] private Transform lowerPoint;

    [Header("Ajustes del ascensor")]
    [SerializeField] private float speed = 2f;

    [Header("Animator de las puertas")]
    [SerializeField] private Animator doorAnimator;

    [Header("Detección del jugador")]
    [SerializeField] private float playerDetectDistance = 3f;
    [SerializeField] private Transform player;

    [Header("Audio del Ascensor")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ascensorClip;

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

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.LogWarning("[Elevator] No se encontró AudioSource.");
        }
    }

    private void Update()
    {
        DetectPlayer();

        if (movingUp)
            MoveTowards(upperPoint, MOVE_UP);

        if (movingDown)
            MoveTowards(lowerPoint, MOVE_DOWN);

        CheckElevatorSound();
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
    // CONTROL DE SONIDO
    // ------------------------------------------
    private void CheckElevatorSound()
    {
        if (doorAnimator == null || audioSource == null) return;

        AnimatorStateInfo state = doorAnimator.GetCurrentAnimatorStateInfo(0);

        bool isMovingAnim =
            state.IsName(MOVE_UP) ||
            state.IsName(MOVE_DOWN);

        // Si está en animación de subir/bajar → sonido ON
        if (isMovingAnim)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = ascensorClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // Cambia a otra animación → cortar sonido
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    // ------------------------------------------
    // MOVIMIENTO
    // ------------------------------------------
    private void MoveTowards(Transform target, string moveAnim)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

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
