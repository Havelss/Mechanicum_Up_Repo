using UnityEngine;
using System.Collections.Generic;

public class CintaTransportadora : MonoBehaviour
{
    [Header("Dirección")]
    [Tooltip("1 = derecha | -1 = izquierda")]
    public float direccionX = 1f;

    [Header("Fuerza")]
    public float fuerzaObjetos = 25f;
    public float fuerzaPlayer = 6f;

    [Header("Tags afectados")]
    public List<string> tagsObjetos = new List<string>() { "Box", "Crate" };
    public string playerTag = "Player";

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;

        Vector3 force = Vector3.right * direccionX;

        // 🧍 PLAYER → resistencia
        if (other.CompareTag(playerTag))
        {
            rb.AddForce(force * fuerzaPlayer, ForceMode.Force);
            return;
        }

        // 📦 OBJETOS → empuje fuerte
        for (int i = 0; i < tagsObjetos.Count; i++)
        {
            if (other.CompareTag(tagsObjetos[i]))
            {
                rb.AddForce(force * fuerzaObjetos, ForceMode.Force);
                return;
            }
        }

        Debug.Log($"[Cinta] Empujando {other.name}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.right * direccionX);
    }
}
