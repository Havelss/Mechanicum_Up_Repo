using UnityEngine;

public class EndZone : MonoBehaviour
{
    [Header("UI del final del juego")]
    [SerializeField] private GameObject endGameCanvas; // Asigna aquí el Canvas de "Fin del juego"

    private bool hasEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que el jugador haya entrado
        if (!hasEnded && other.CompareTag("Player"))
        {
            hasEnded = true;
            ShowEndScreen();
        }
    }

    private void ShowEndScreen()
    {
        Debug.Log(" Fin del juego alcanzado.");

        if (endGameCanvas != null)
            endGameCanvas.SetActive(true);
        else
            Debug.LogWarning("No se asignó el EndGameCanvas en el inspector.");

        // Pausar el juego
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
