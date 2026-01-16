using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Configuration")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnFallLimit = -10f;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Muerte por caída
        if (transform.position.y <= respawnFallLimit)
        {
            if (!playerController.IsDead())
                playerController.Die("fall");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Muerte por enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!playerController.IsDead())
                playerController.Die("enemy");
        }
    }

    // -------------------------------
    //  CHECKPOINT SYSTEM (como antes)
    // -------------------------------

    public void SetRespawnPoint(Transform newRespawn)
    {
        if (newRespawn == null) return;

        respawnPoint = newRespawn;
        PlayerManager.Instance.SetCheckpoint(newRespawn);

        Debug.Log($"🟢 Nuevo punto de respawn establecido: {newRespawn.name}");
    }

    public void UpdateRespawn(Transform newRespawnPoint)
    {
        if (newRespawnPoint == null) return;

        respawnPoint = newRespawnPoint;
        PlayerManager.Instance.SetCheckpoint(newRespawnPoint);

        Debug.Log($"Respawn actualizado a: {newRespawnPoint.position}");
    }


}
