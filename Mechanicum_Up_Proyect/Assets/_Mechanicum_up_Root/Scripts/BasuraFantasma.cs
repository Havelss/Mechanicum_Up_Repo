using UnityEngine;

public class BasuraFantasma : MonoBehaviour
{
    // Tags que este trigger reconocerá
    private readonly string[] tagsAceptados = { "Vagoneta", "Player" };

    private void OnTriggerEnter(Collider other)
    {
        // Solo detecta que el objeto entró, pero no lo bloquea ni destruye
        foreach (string tag in tagsAceptados)
        {
            if (other.CompareTag(tag))
            {
                Debug.Log("Trigger atravesado por: " + other.name);
                // No hacer nada más, el objeto sigue su camino
            }
        }
    }

}
