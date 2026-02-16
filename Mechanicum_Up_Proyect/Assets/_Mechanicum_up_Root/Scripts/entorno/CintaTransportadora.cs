using UnityEngine;
using System.Collections.Generic;

public class CintaTransportadora : MonoBehaviour
{
    [Header("Dirección Global")]
    [Tooltip("1 = Derecha (X+) | -1 = Izquierda (X-)")]
    public float direccionX = 1f;

    [Header("Fuerza / Velocidad")]
    public float velocidadObjetos = 5f; // velocidad de las cajas
    public float velocidadPlayer = 3f;  // velocidad del jugador

    [Header("Tags afectados")]
    public List<string> tagsObjetos = new List<string>() { "Box", "Crate" };
    public string playerTag = "Player";

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;

        if (EsObjetoValido(other))
        {
            if (other.CompareTag(playerTag))
            {
                // Jugador: movemos con velocidad directa para mayor control
                Vector3 vel = rb.velocity;
                vel.x = direccionX * velocidadPlayer;
                rb.velocity = vel;
            }
            else
            {
                // Objetos: empuje físico confiable
                Vector3 fuerza = new Vector3(direccionX * velocidadObjetos, 0, 0);
                rb.AddForce(fuerza, ForceMode.VelocityChange);
            }
        }
    }

    private bool EsObjetoValido(Collider other)
    {
        if (other.CompareTag(playerTag)) return true;

        foreach (string t in tagsObjetos)
        {
            if (other.CompareTag(t)) return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 inicio = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawRay(inicio, Vector3.right * direccionX * 2);
    }
}
