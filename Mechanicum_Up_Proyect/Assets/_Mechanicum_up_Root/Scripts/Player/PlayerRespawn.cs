using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Configuration")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnFallLimit = -10f;

    [Header("Respawn Timing")]
    [SerializeField] private float respawnDelay = 3f;

    private PlayerController playerController;
    private bool isRespawning = false;

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
            {
                Debug.Log("[PlayerRespawn] Player cayó por debajo del límite, muriendo...");
                playerController.Die("fall");
                StartRespawnCoroutine();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Muerte por enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!playerController.IsDead())
            {
                Debug.Log("[PlayerRespawn] Player colisionó con enemigo, muriendo...");
                playerController.Die("enemy");
                StartRespawnCoroutine();
            }
        }
    }

    // -------------------------------
    // CHECKPOINT SYSTEM
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

        Debug.Log($"[PlayerRespawn] Respawn actualizado a: {newRespawnPoint.position}");
    }

    // -------------------------------
    // COROUTINE DE RESPAWN
    // -------------------------------
    private void StartRespawnCoroutine()
    {
        if (!isRespawning)
            StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        isRespawning = true;

        Debug.Log($"[PlayerRespawn] Respawn en {respawnDelay} segundos...");

        yield return new WaitForSeconds(respawnDelay);

        if (respawnPoint != null && playerController != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;

            playerController.isRespawning = true;
            playerController.SetInputEnabled(true);
            playerController.ApplyPowerUpVisuals();
            playerController.PlayRespawnVFX();

            Debug.Log("[PlayerRespawn] Player respawneado en el checkpoint");
        }
        else
        {
            Debug.LogWarning("[PlayerRespawn] RespawnPoint o PlayerController no asignados");
        }

        isRespawning = false;
    }
}
