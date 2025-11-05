using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Configuration")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnFallLimit = -10f;

    private Rigidbody playerRB;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y <= respawnFallLimit)
            Respawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        if (playerRB != null)
            playerRB.linearVelocity = Vector3.zero;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;
        else
            Debug.LogWarning("⚠️ No hay punto de respawn asignado.");
    }

    // 🔹 Llamar a este método cuando interactúe con una terminal
    public void SetRespawnPoint(Transform newRespawn)
    {
        if (newRespawn == null) return;

        respawnPoint = newRespawn;
        Debug.Log($"🟢 Nuevo punto de respawn establecido: {newRespawn.name}");
    }
}
