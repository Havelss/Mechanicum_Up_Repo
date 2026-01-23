using UnityEngine;

public class SymbolPickup : MonoBehaviour
{
    [SerializeField] private string symbolID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SymbolManager.Instance != null)
        {
            SymbolManager.Instance.UnlockSymbol(symbolID);
            Debug.Log($"Símbolo recogido: {symbolID}");
            Destroy(gameObject);
        }
    }
}

