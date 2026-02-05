using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerFrontalCrash : MonoBehaviour
{
    [Header("Detección")]
    public string vagonetaTag = "Vagoneta";
    public bool triggerOnlyOnce = true;

    [Header("Configuración del Choque")]
    public List<Transform> waypoints; // El último waypoint debería ser el punto del impacto
    public float moveSpeed = 15f;

    [Header("Efecto Volcado Frontal")]
    [Tooltip("Grados de rotación en X (90 = morro clavado, 180 = techo al suelo)")]
    public float crashTiltX = 180f;
    public float tiltSpeed = 5f;

    [Header("Efecto Salida Disparada")]
    [Tooltip("Fuerza de eyección. Al ser impacto frontal, la fuerza suele ir hacia adelante (X) y arriba (Y)")]
    public Vector3 ejectionForce = new Vector3(10f, 12f, 0f);

    [Header("Respawn")]
    public Transform respawnPoint;
    public float slowMotionFactor = 0.5f; // Para darle drama
    public float delayBeforeRespawn = 2f;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated && triggerOnlyOnce) return;

        MinecartController cart = other.GetComponentInParent<MinecartController>();
        if (cart == null) return;

        activated = true;
        StartCoroutine(FrontalCrashSequence(cart));
    }

    private IEnumerator FrontalCrashSequence(MinecartController cart)
    {
        Debug.Log("[Crash] 💥 ¡Impacto frontal detectado!");

        PlayerController pc = null;
        if (cart.isPlayerInside) pc = cart.player.GetComponent<PlayerController>();

        // 1. Bloqueo y preparación
        if (pc != null) pc.enabled = false;
        if (cart.rb != null) cart.rb.isKinematic = true;
        cart.enabled = false;

        // 2. Movimiento hacia el punto de impacto (Waypoints)
        foreach (Transform point in waypoints)
        {
            while (Vector3.Distance(cart.transform.position, point.position) > 0.2f)
            {
                cart.transform.position = Vector3.MoveTowards(cart.transform.position, point.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // --- MOMENTO DEL IMPACTO ---
        Time.timeScale = slowMotionFactor; // Efecto cámara lenta para el golpe
        float currentX = 0;

        // 3. Rotación de "pino" (el techo hacia adelante)
        while (currentX < crashTiltX)
        {
            currentX += tiltSpeed * Time.unscaledDeltaTime * 100f;

            // Rotamos en X para que la trasera suba
            cart.transform.localRotation = Quaternion.Euler(currentX, cart.transform.localRotation.eulerAngles.y, 0);

            // 4. A mitad del vuelco, lanzamos al jugador
            if (currentX > 45f && pc != null && cart.isPlayerInside)
            {
                LaunchPlayer(cart, pc);
            }

            yield return null;
        }

        Time.timeScale = 1f; // Restaurar tiempo

        yield return new WaitForSeconds(delayBeforeRespawn);

        // 5. Respawn
        if (pc != null && respawnPoint != null)
        {
            pc.transform.position = respawnPoint.position;
            pc.transform.rotation = respawnPoint.rotation;
            Rigidbody pRb = pc.GetComponent<Rigidbody>();
            if (pRb) pRb.linearVelocity = Vector3.zero;
            pc.enabled = true;
        }

        Destroy(cart.gameObject);
    }

    private void LaunchPlayer(MinecartController cart, PlayerController pc)
    {
        cart.ExitCart(); // Devuelve control físico y libera parentesco

        Rigidbody playerRb = pc.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
            // Lanzamos según la dirección del impacto
            playerRb.AddForce(ejectionForce, ForceMode.Impulse);
            // Añadimos una rotación loca (ragdoll manual)
            playerRb.AddTorque(new Vector3(Random.Range(-10, 10), 0, 500f), ForceMode.Impulse);
        }
        Debug.Log("[Crash] 📢 ¡Pasajero despedido por el parabrisas!");
    }
}