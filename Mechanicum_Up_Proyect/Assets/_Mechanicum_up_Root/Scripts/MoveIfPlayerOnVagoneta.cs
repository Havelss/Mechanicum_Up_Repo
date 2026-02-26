using UnityEngine;

public class MoveIfPlayerOnVagoneta : MonoBehaviour
{
    [Header("Configuración Vagoneta")]
    [SerializeField] private LayerMask vagonetaLayer;

    [Header("Movimiento")]
    [SerializeField] private float heightOffset = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool playerOnVagoneta = false;

    private Transform playerTransform;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * heightOffset;
    }

    private void Update()
    {
        EnsurePlayerReference();
        CheckIfPlayerOnVagoneta();
        HandleMovement();
    }

    // 🔎 Si el player fue destruido, volver a buscarlo
    private void EnsurePlayerReference()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
    }

    private void CheckIfPlayerOnVagoneta()
    {
        if (playerTransform == null)
        {
            playerOnVagoneta = false;
            return;
        }

        Collider[] hits = Physics.OverlapSphere(
            playerTransform.position,
            0.2f,
            vagonetaLayer
        );

        playerOnVagoneta = hits.Length > 0;
    }

    private void HandleMovement()
    {
        Vector3 target = playerOnVagoneta ? targetPosition : startPosition;

        transform.position = Vector3.Lerp(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );
    }
}