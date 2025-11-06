using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var powerUp = other.GetComponent<P_ElectricityPowerUp>();
        if (powerUp != null)
        {
            powerUp.ActivatePower();
            Destroy(gameObject);
        }
    }
}
