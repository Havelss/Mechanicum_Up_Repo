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

    private Rigidbody rb;

    private void Start()
    {
        PlayIdleClosed();

        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

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
        CheckElevatorSound();
    }

    private void FixedUpdate()
    {
        if (movingUp)
            MoveTowardsPhysics(upperPoint, MOVE_UP);
        else if (movingDown)
            MoveTowardsPhysics(lowerPoint, MOVE_DOWN);
    }

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

    private void CheckElevatorSound()
    {
        if (doorAnimator == null || audioSource == null) return;

        AnimatorStateInfo state = doorAnimator.GetCurrentAnimatorStateInfo(0);

        bool isMovingAnim =
            state.IsName(MOVE_UP) ||
            state.IsName(MOVE_DOWN);

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
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    private void MoveTowardsPhysics(Transform target, string moveAnim)
    {
        Vector3 newPos = Vector3.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        PlayDoors(moveAnim);

        if (Vector3.Distance(rb.position, target.position) < 0.05f)
        {
            movingUp = false;
            movingDown = false;

            if (playerNearby)
                PlayIdleOpen();
            else
                PlayIdleClosed();
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

    public bool IsMoving() => movingUp || movingDown;

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
