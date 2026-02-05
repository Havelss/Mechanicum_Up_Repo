using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrashAndGameOver : MonoBehaviour
{
    [Header("Detección")]
    public string vagonetaTag = "Vagoneta";
    public bool triggerOnlyOnce = true;

    [Header("Configuración del Choque")]
    public List<Transform> waypoints;
    public float moveSpeed = 15f;

    [Header("Efecto Volcado")]
    public float crashTiltX = 180f;
    public float tiltSpeed = 5f;

    [Header("Efecto Eyección")]
    public Vector3 ejectionForce = new Vector3(10f, 12f, 0f);

    [Header("Interfaz de Fin de Juego")]
    [Tooltip("El Canvas que se activará al final")]
    public GameObject gameOverCanvas;
    public bool pauseGameAtEnd = true;
    public bool showMouseCursor = true;

    private bool activated = false;

    private void Start()
    {
        if (gameOverCanvas != null) gameOverCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated && triggerOnlyOnce) return;

        // Buscamos si es vagoneta o el player a pie
        MinecartController cart = other.GetComponentInParent<MinecartController>();
        PlayerController pc = other.GetComponentInParent<PlayerController>();

        // Si es el player a pie, vamos directo al Game Over
        if (pc != null && cart == null)
        {
            activated = true;
            ShowGameOverUI();
            return;
        }

        // Si es vagoneta, hacemos la secuencia de choque
        if (cart != null)
        {
            activated = true;
            StartCoroutine(FrontalCrashSequence(cart));
        }
    }

    private IEnumerator FrontalCrashSequence(MinecartController cart)
    {
        PlayerController pc = null;
        if (cart.isPlayerInside) pc = cart.player.GetComponent<PlayerController>();

        // 1. Bloqueo
        if (pc != null) pc.enabled = false;
        if (cart.rb != null) cart.rb.isKinematic = true;
        cart.enabled = false;

        // 2. Movimiento a los Waypoints
        foreach (Transform point in waypoints)
        {
            while (Vector3.Distance(cart.transform.position, point.position) > 0.2f)
            {
                cart.transform.position = Vector3.MoveTowards(cart.transform.position, point.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // 3. Impacto y Volcado
        float currentX = 0;
        while (currentX < crashTiltX)
        {
            currentX += tiltSpeed * Time.unscaledDeltaTime * 100f;
            cart.transform.localRotation = Quaternion.Euler(currentX, cart.transform.localRotation.eulerAngles.y, 0);

            // Eyección del jugador
            if (currentX > 45f && pc != null && cart.isPlayerInside)
            {
                LaunchPlayer(cart, pc);
            }
            yield return null;
        }

        // 4. Esperar un segundo para ver el desastre y mostrar UI
        yield return new WaitForSeconds(1f);
        ShowGameOverUI();
    }

    private void LaunchPlayer(MinecartController cart, PlayerController pc)
    {
        cart.ExitCart();
        Rigidbody playerRb = pc.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
            playerRb.AddForce(ejectionForce, ForceMode.Impulse);
            playerRb.AddTorque(new Vector3(Random.Range(-10, 10), 0, 500f), ForceMode.Impulse);
        }
    }

    public void ShowGameOverUI()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);

            if (pauseGameAtEnd) Time.timeScale = 0f;

            if (showMouseCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}