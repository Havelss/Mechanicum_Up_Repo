using UnityEngine;

public class P_Energy : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float CurrentEnergy { get; private set; }

    private void Awake()
    {
        CurrentEnergy = maxEnergy;
    }

    public void ConsumeEnergy(float amount)
    {
        CurrentEnergy -= amount;
        if (CurrentEnergy < 0f)
            CurrentEnergy = 0f;
    }

    public void RechargeEnergy(float amount)
    {
        CurrentEnergy += amount;
        if (CurrentEnergy > maxEnergy)
            CurrentEnergy = maxEnergy;
    }
}
