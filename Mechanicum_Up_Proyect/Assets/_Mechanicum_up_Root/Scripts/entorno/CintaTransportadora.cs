using UnityEngine;
using System.Collections.Generic;

public class CintaTransportadora : MonoBehaviour
{
    [Header("Dirección Global")]
    [Tooltip("1 = Derecha (X+) | -1 = Izquierda (X-)")]
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
        if (rb == null) return;

        if (EsObjetoValido(other))
        {
            float fuerzaAplicar = other.CompareTag(playerTag) ? fuerzaPlayer : fuerzaObjetos;

            // Aplicar fuerza adicional en el eje X
            Vector3 fuerza = new Vector3(direccionX * fuerzaAplicar, 0, 0);
            rb.AddForce(fuerza, ForceMode.Acceleration);
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
