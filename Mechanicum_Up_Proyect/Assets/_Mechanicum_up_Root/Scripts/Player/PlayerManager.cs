using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player")]
    public GameObject playerPrefab;
    [HideInInspector] public GameObject currentPlayer;

    [Header("Estado")]
    public Transform currentCheckpoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // Destruye el player antiguo si existe
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
            Debug.Log("[PlayerManager] Player anterior destruido");
        }

        if (currentCheckpoint == null)
        {
            Debug.LogWarning("[PlayerManager] Spawn sin checkpoint, usando Vector3.zero");
        }

        Vector3 spawnPos = currentCheckpoint != null ? currentCheckpoint.position : Vector3.zero;
        currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"[PlayerManager] Player instanciado en: {spawnPos} | Checkpoint: {(currentCheckpoint != null ? currentCheckpoint.name : "Ninguno")}");

        // Reasignar cámara si existe
        CameraFollow camFollow = Camera.main?.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.SetTarget(currentPlayer.transform);
            Debug.Log("[PlayerManager] Cámara reasignada al nuevo player");
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
        Debug.Log($"🟢 Checkpoint actualizado: {checkpoint.name} | Posición: {checkpoint.position}");
    }

    public void OnPlayerDeath()
    {
        Debug.Log("[PlayerManager] Player ha muerto, respawneando...");
        SpawnPlayer();
    }
}
