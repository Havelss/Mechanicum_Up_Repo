//using UnityEngine;
//using System.Collections;

//public class PlayerManager : MonoBehaviour
//{
//    public static PlayerManager Instance;

//    [Header("Player")]
//    public GameObject playerPrefab;
//    [HideInInspector] public GameObject currentPlayer;

//    [Header("Estado")]
//    public Transform currentCheckpoint;

//    [Header("Respawn")]
//    public float respawnDelay = 3f; // Tiempo configurable antes de reaparecer

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//        DontDestroyOnLoad(gameObject);

//        SpawnPlayerInstant();
//    }

//    // 🔹 Spawn inicial sin delay
//    private void SpawnPlayerInstant()
//    {
//        Vector3 spawnPos = currentCheckpoint != null ? currentCheckpoint.position : Vector3.zero;
//        currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
//        Debug.Log($"[PlayerManager] Player instanciado en: {spawnPos} | Checkpoint: {(currentCheckpoint != null ? currentCheckpoint.name : "Ninguno")}");

//        // Reasignar cámara si existe
//        CameraFollow camFollow = Camera.main?.GetComponent<CameraFollow>();
//        if (camFollow != null)
//        {
//            camFollow.SetTarget(currentPlayer.transform);
//            Debug.Log("[PlayerManager] Cámara reasignada al nuevo player");
//        }
//    }

//    // 🔹 Spawn con delay (desde PlayerRespawn)
//    public void RespawnPlayer()
//    {
//        StartCoroutine(RespawnCoroutine());
//    }

//    private IEnumerator RespawnCoroutine()
//    {
//        if (currentPlayer != null)
//        {
//            Destroy(currentPlayer);
//            Debug.Log("[PlayerManager] Player destruido antes de respawn");
//        }

//        Debug.Log($"[PlayerManager] Respawn en {respawnDelay} segundos...");
//        yield return new WaitForSeconds(respawnDelay);

//        Vector3 spawnPos = currentCheckpoint != null ? currentCheckpoint.position : Vector3.zero;
//        currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
//        Debug.Log($"[PlayerManager] Player respawneado en: {spawnPos}");

//        // Reasignar cámara si existe
//        CameraFollow camFollow = Camera.main?.GetComponent<CameraFollow>();
//        if (camFollow != null)
//            camFollow.SetTarget(currentPlayer.transform);

//        // Activar visuales y VFX si el prefab tiene PlayerController
//        PlayerController pc = currentPlayer.GetComponent<PlayerController>();
//        if (pc != null)
//        {
//            pc.isRespawning = true;
//            pc.ApplyPowerUpVisuals();
//            pc.PlayRespawnVFX();
//            pc.isRespawning = false;
//        }
//    }

//    public void SetCheckpoint(Transform checkpoint)
//    {
//        if (checkpoint == null)
//        {
//            Debug.LogWarning("[PlayerManager] Intentaste asignar un checkpoint nulo.");
//            return;
//        }

//        currentCheckpoint = checkpoint;
//        Debug.Log($"🟢 Checkpoint actualizado: {checkpoint.name} | Posición: {checkpoint.position}");
//    }

//    public void OnPlayerDeath()
//    {
//        Debug.Log("[PlayerManager] Player ha muerto, iniciando respawn...");
//        RespawnPlayer();
//    }
//}

using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player")]
    public GameObject playerPrefab;
    [HideInInspector] public GameObject currentPlayer;

    [Header("Estado")]
    public Transform currentCheckpoint;

    [Header("Respawn")]
    public float respawnDelay = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SpawnPlayerInstant();
    }

    // 🔹 Spawn inicial sin delay
    private void SpawnPlayerInstant()
    {
        Vector3 spawnPos = currentCheckpoint != null ? currentCheckpoint.position : Vector3.zero;
        Quaternion spawnRot = playerPrefab.transform.rotation; // ✅ RESPETA ROTACIÓN ORIGINAL

        currentPlayer = Instantiate(playerPrefab, spawnPos, spawnRot);

        Debug.Log($"[PlayerManager] Player instanciado en: {spawnPos}");

        // Reasignar cámara
        CameraFollow camFollow = Camera.main?.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.SetTarget(currentPlayer.transform);
    }

    // 🔹 Spawn con delay
    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
            Debug.Log("[PlayerManager] Player destruido antes de respawn");
        }

        Debug.Log($"[PlayerManager] Respawn en {respawnDelay} segundos...");
        yield return new WaitForSeconds(respawnDelay);

        Vector3 spawnPos = currentCheckpoint != null ? currentCheckpoint.position : Vector3.zero;
        Quaternion spawnRot = playerPrefab.transform.rotation; // ✅ RESPETA ROTACIÓN ORIGINAL

        currentPlayer = Instantiate(playerPrefab, spawnPos, spawnRot);

        Debug.Log($"[PlayerManager] Player respawneado en: {spawnPos}");

        // Reasignar cámara
        CameraFollow camFollow = Camera.main?.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.SetTarget(currentPlayer.transform);

        // Restaurar estado visual
        PlayerController pc = currentPlayer.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.isRespawning = true;
            pc.ApplyPowerUpVisuals();
            pc.PlayRespawnVFX();
            pc.isRespawning = false;
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint == null)
        {
            Debug.LogWarning("[PlayerManager] Intentaste asignar un checkpoint nulo.");
            return;
        }

        currentCheckpoint = checkpoint;
        Debug.Log($"🟢 Checkpoint actualizado: {checkpoint.name}");
    }

    public void OnPlayerDeath()
    {
        Debug.Log("[PlayerManager] Player ha muerto, iniciando respawn...");
        RespawnPlayer();
    }
}