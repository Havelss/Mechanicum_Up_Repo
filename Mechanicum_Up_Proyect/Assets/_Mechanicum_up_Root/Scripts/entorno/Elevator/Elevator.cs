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
    [SerializeField] private LayerMask playerLayer;

    [Header("Audio del Ascensor")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ascensorClip;

    // Constantes de Animación
    private const string IDLE_OPEN = "Armature_Ascensor_Idle_Abierto";
    private const string IDLE_CLOSED = "Armature_Ascensor_Idle_Cerrado";
    private const string CLOSE = "Armature_Ascensor_Cierre";
    private const string OPEN = "Armature_Ascensor_Apertura";

    private const string MOVE_UP = "Armature_Ascensor_Idle_Subida";
    private const string MOVE_DOWN = "Armature_Ascensor_Idle_Bajada";

    // Nuevas constantes de animación
    private const string STOP_UP = "Armature_Ascensor_Parada_Arriba";
    private const string STOP_DOWN = "Armature_Ascensor_Parada_Abajo";

    private bool movingUp = false;
    private bool movingDown = false;
    private bool playerNearby = false;

    private Rigidbody rb;
    private Transform player;

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
        FindPlayer();
        DetectPlayer();
        CheckElevatorSound();
    }

    private void FixedUpdate()
    {
        if (movingUp)
            MoveTowardsPhysics(upperPoint, MOVE_UP, STOP_UP);
        else if (movingDown)
            MoveTowardsPhysics(lowerPoint, MOVE_DOWN, STOP_DOWN);
    }

    private void FindPlayer()
    {
        if (player != null) return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            playerDetectDistance,
            playerLayer
        );

        if (hits.Length > 0)
            player = hits[0].transform;
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
                PlayOpening();
            }
        }
        else
        {
            if (playerNearby)
            {
                playerNearby = false;
                PlayClosing();
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

    private void MoveTowardsPhysics(Transform target, string moveAnim, string stopAnim)
    {
        Vector3 newPos = Vector3.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        PlayDoors(moveAnim);

        if (Vector3.Distance(rb.position, target.position) < 0.05f)
        {
            movingUp = false;
            movingDown = false;

            Play(stopAnim); // Ejecuta la animación de parada

            if (playerNearby)
                PlayOpening();
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

    // Métodos de ayuda
    private void PlayOpening() => Play(OPEN);
    private void PlayIdleOpen() => Play(IDLE_OPEN);
    private void PlayIdleClosed() => Play(IDLE_CLOSED);
    private void PlayClosing() => Play(CLOSE);
    private void PlayDoors(string anim) => Play(anim);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, playerDetectDistance);
    }

    // Añade esto al final de tu script Elevator.cs
    public bool IsAtUpperPoint()
    {
        if (upperPoint == null) return false;
        // Comprobamos si la distancia al punto de arriba es muy pequeña
        return Vector3.Distance(transform.position, upperPoint.position) < 0.1f;
    }

    public bool IsAtLowerPoint()
    {
        if (lowerPoint == null) return false;
        // Comprobamos si la distancia al punto de abajo es muy pequeña
        return Vector3.Distance(transform.position, lowerPoint.position) < 0.1f;
    }
}