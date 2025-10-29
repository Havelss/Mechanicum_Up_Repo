using UnityEngine;

public interface I_Electrifiable
{
    bool IsPowered { get; }
    void PowerOn();
}
