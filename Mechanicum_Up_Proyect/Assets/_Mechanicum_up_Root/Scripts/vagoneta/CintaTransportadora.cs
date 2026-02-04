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

        // Si no tiene Rigidbody o es cinemático, no podemos aplicar velocidad física
        if (rb == null || rb.isKinematic) return;

        // Comprobamos si el objeto tiene un tag válido
        if (EsObjetoValido(other))
        {
            float fuerzaAplicar = other.CompareTag(playerTag) ? fuerzaPlayer : fuerzaObjetos;

            // MÉTODO DE VELOCIDAD DIRECTA:
            // Forzamos la velocidad en el eje X del mundo (Vector3.right * direccionX)
            // Mantenemos rb.linearVelocity.y para que la gravedad siga funcionando
            // Mantenemos rb.linearVelocity.z para que no se frene si se mueve de frente/atrás

            float velocidadX = direccionX * fuerzaAplicar;

            rb.linearVelocity = new Vector3(velocidadX, rb.linearVelocity.y, rb.linearVelocity.z);

            // Nota: Si usas una versión de Unity antigua (anterior a 2023), 
            // cambia 'linearVelocity' por 'velocity'.
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
        // Esto dibujará una línea amarilla en el editor indicando la dirección real del empuje
        Gizmos.color = Color.yellow;
        Vector3 inicio = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawRay(inicio, Vector3.right * direccionX * 2);
    }
}