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
            Destroy(currentPlayer);

        Vector3 spawnPos = currentCheckpoint != null ? currentCheckpoint.position : Vector3.zero;
        currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // Reasignar cámara si existe
        CameraFollow camFollow = Camera.main?.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.SetTarget(currentPlayer.transform);
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
        Debug.Log($"🟢 Checkpoint actualizado: {checkpoint.name}");
    }

    public void OnPlayerDeath()
    {
        SpawnPlayer();
    }
}