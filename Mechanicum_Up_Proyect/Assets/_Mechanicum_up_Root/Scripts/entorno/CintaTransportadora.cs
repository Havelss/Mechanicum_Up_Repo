using UnityEngine;
using System.Collections.Generic;

public class CintaTransportadora : MonoBehaviour
{
    [Header("Dirección Global")]
    [Tooltip("1 = Derecha (X+) | -1 = Izquierda (X-)")]
    public float direccionX = 1f;

    [Header("Velocidad de la cinta")]
    public float velocidadObjetos = 3f;
    public float velocidadPlayer = 2f;

    [Header("Tags afectados")]
    public List<string> tagsObjetos = new List<string>() { "Box", "Crate" };
    public string playerTag = "Player";

    private HashSet<Rigidbody> objetosEnCinta = new HashSet<Rigidbody>();
    private HashSet<Transform> playersEnCinta = new HashSet<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !rb.isKinematic && EsObjetoValido(other))
        {
            if (other.CompareTag(playerTag))
            {
                playersEnCinta.Add(other.transform);
            }
            else
            {
                objetosEnCinta.Add(rb);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            objetosEnCinta.Remove(rb);
        }

        if (other.CompareTag(playerTag))
        {
            playersEnCinta.Remove(other.transform);
        }
    }

    private void FixedUpdate()
    {
        // Mover objetos con Rigidbody
        foreach (var rb in objetosEnCinta)
        {
            if (rb == null) continue;
            Vector3 fuerza = new Vector3(direccionX * velocidadObjetos, 0, 0);
            rb.AddForce(fuerza, ForceMode.VelocityChange);
        }

        // Mover players directamente transform
        foreach (var player in playersEnCinta)
        {
            if (player == null) continue;
            Vector3 movimiento = Vector3.right * direccionX * velocidadPlayer * Time.fixedDeltaTime;
            player.position += movimiento;
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
