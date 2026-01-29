using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [Header("Configuración del Objeto")]
    [Tooltip("El prefab que quieres instanciar dentro del jugador")]
    [SerializeField] private GameObject prefabToPlace;

    [Tooltip("Nombre exacto del objeto hijo del Player donde se colocará el prefab")]
    [SerializeField] private string anchorPointName = "PowerUpHolder";

    private void OnTriggerEnter(Collider other)
    {
        // 1. Intentamos obtener el componente de electricidad como ya hacías
        var electricity = other.GetComponent<P_ElectricityArea>();

        if (electricity != null)
        {
            // 2. Buscamos el punto de anclaje dentro del Player
            // Buscamos en los hijos del objeto que colisionó (other)
            Transform anchor = FindChildRecursive(other.transform, anchorPointName);

            if (anchor != null && prefabToPlace != null)
            {
                // 3. Instanciamos el prefab y lo emparentamos al anchor
                GameObject newObject = Instantiate(prefabToPlace, anchor.position, anchor.rotation);
                newObject.transform.SetParent(anchor);
            }
            else if (anchor == null)
            {
                Debug.LogWarning($"No se encontró el punto de anclaje '{anchorPointName}' en {other.name}");
            }

            // 4. Lógica original
            electricity.ActivatePower();
            Destroy(gameObject);
        }
    }

    // Función auxiliar para encontrar un hijo por nombre incluso si está profundo en la jerarquía
    private Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }
}