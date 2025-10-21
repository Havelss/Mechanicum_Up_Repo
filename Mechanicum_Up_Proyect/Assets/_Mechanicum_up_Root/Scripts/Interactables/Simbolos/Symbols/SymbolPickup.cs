using UnityEngine;

public class SymbolPickup : MonoBehaviour
{
    [SerializeField] private string symbolID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SymbolManager.Instance != null)
            {
                SymbolManager.Instance.UnlockSymbol(symbolID);
                Debug.Log($"Símbolo recogido por trigger: {symbolID}");
            }

            
        }
    }
}
