using UnityEngine;
using System;

public class TriggerHook : MonoBehaviour
{
    public Action onEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onEnter?.Invoke();
        }
    }
}
