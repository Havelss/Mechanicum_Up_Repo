using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerFrontalCrash : MonoBehaviour
{
    // ... (Tus variables se mantienen igual)
    [Header("Detección")]
    public string vagonetaTag = "Vagoneta";
    public bool triggerOnlyOnce = true;

    [Header("Configuración del Choque")]
    public List<Transform> waypoints;
    public float moveSpeed = 15f;

    [Header("Efecto Volcado Frontal")]
    public float crashTiltX = 180f;
    public float tiltSpeed = 5f;

    [Header("Efecto Salida Disparada")]
    public Vector3 ejectionForce = new Vector3(10f, 12f, 0f);

    [Header("Respawn")]
    public Transform respawnPoint;
    public float slowMotionFactor = 0.5f;
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
        Transform playerTransform = null;

        if (cart.isPlayerInside)
        {
            pc = cart.player.GetComponent<PlayerController>();
            playerTransform = cart.player;
        }

        // 1. Bloqueo
        if (pc != null) pc.enabled = false;
        if (cart.rb != null) cart.rb.isKinematic = true;
        cart.enabled = false; // Aquí se detiene el pegado automático del player

        // 2. Movimiento hacia el punto de impacto
        foreach (Transform point in waypoints)
        {
            while (Vector3.Distance(cart.transform.position, point.position) > 0.1f)
            {
                cart.transform.position = Vector3.MoveTowards(cart.transform.position, point.position, moveSpeed * Time.deltaTime);

                // 🔥 NOVEDAD: Arrastramos manualmente al player al asiento
                if (cart.isPlayerInside && playerTransform != null && cart.playerSeat != null)
                {
                    playerTransform.position = cart.playerSeat.position;
                    // Mantenemos escala por si acaso
                    playerTransform.localScale = Vector3.one;
                }

                yield return null;
            }
        }

        // --- MOMENTO DEL IMPACTO ---
        Time.timeScale = slowMotionFactor;
        float currentX = 0;
        bool playerLaunched = false;

        // 3. Rotación de "pino"
        while (currentX < crashTiltX)
        {
            currentX += tiltSpeed * Time.unscaledDeltaTime * 100f;
            cart.transform.localRotation = Quaternion.Euler(currentX, cart.transform.localRotation.eulerAngles.y, 0);

            // 🔥 NOVEDAD: El player debe rotar y posicionarse con el asiento mientras vuelca
            if (cart.isPlayerInside && !playerLaunched && playerTransform != null)
            {
                playerTransform.position = cart.playerSeat.position;
                playerTransform.rotation = cart.playerSeat.rotation;
            }

            // 4. Lanzamiento
            if (currentX > 45f && !playerLaunched && cart.isPlayerInside)
            {
                playerLaunched = true;
                LaunchPlayer(cart, pc);
            }

            yield return null;
        }

        Time.timeScale = 1f;

        yield return new WaitForSeconds(delayBeforeRespawn);

        // 5. Respawn
        if (pc != null && respawnPoint != null)
        {
            // Antes de moverlo, nos aseguramos de que no tenga padres
            pc.transform.SetParent(null);
            pc.transform.position = respawnPoint.position;
            pc.transform.rotation = respawnPoint.rotation;
            pc.transform.localScale = Vector3.one;

            Rigidbody pRb = pc.GetComponent<Rigidbody>();
            if (pRb)
            {
                pRb.linearVelocity = Vector3.zero;
                pRb.angularVelocity = Vector3.zero;
            }
            pc.enabled = true;
        }

        Destroy(cart.gameObject);
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
}