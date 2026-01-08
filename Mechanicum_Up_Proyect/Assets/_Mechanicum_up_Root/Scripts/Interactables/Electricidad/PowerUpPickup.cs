using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"El objeto {other.name} ha tocado el power-up.");

        var powerUp = other.GetComponent<P_ElectricityPowerUp>();
        if (powerUp != null)
        {
            powerUp.ActivatePower();
            Debug.Log("⚡ Power-up de electricidad obtenido!");
            Destroy(gameObject);
        }
    }
}

//using UnityEngine;

//public class PowerUpPickup : MonoBehaviour
//{
//    private void OnTriggerEnter(Collider other)
//    {
//        var electricity = other.GetComponent<P_ElectricityArea>();
//        if (electricity != null)
//        {
//            electricity.ActivatePower();
//            Debug.Log("⚡ Power-up de electricidad obtenido!");
//            Destroy(gameObject);
//        }
//    }
//}
