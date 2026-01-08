using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var electricity = other.GetComponent<P_ElectricityArea>();
        if (electricity != null)
        {
            electricity.ActivatePower();
            Debug.Log("⚡ Power-up de electricidad obtenido!");
            Destroy(gameObject);
        }
    }
}
