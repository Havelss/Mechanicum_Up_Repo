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
        
        var electricity = other.GetComponent<P_ElectricityArea>();

        if (electricity != null)
        {
            if (electricity != null)
            {
                PlayerInventory.Instance.hasElectricPower = true;

                electricity.ActivatePower();
                Destroy(gameObject);
            }

            if (other.CompareTag("Player"))
            {
                PlayerInventory.Instance.hasElectricPower = true;
                Destroy(gameObject);
            }

            Transform anchor = FindChildRecursive(other.transform, anchorPointName);

            if (anchor != null && prefabToPlace != null)
            {
                
                GameObject newObject = Instantiate(prefabToPlace, anchor.position, anchor.rotation);
                newObject.transform.SetParent(anchor);
            }
            else if (anchor == null)
            {
                
            }

            
            electricity.ActivatePower();
            Destroy(gameObject);
        }
    }



    
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