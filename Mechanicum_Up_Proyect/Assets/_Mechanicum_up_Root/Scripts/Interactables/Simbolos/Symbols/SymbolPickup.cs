using UnityEngine;

/*
public class SymbolPickup : MonoBehaviour
{
    [SerializeField] private string symbolID;
    [SerializeField] private SymbolManager targetManager; // referencia explícita

    

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetManager != null)
            {
                targetManager.UnlockSymbol(symbolID);
                Debug.Log($"Símbolo {symbolID} desbloqueado en {targetManager.name}");
                
            }
            else
            {
                Debug.LogWarning("No se asignó ningún SymbolManager en el SymbolPickup.");
            }
        }
    }
}
*/ // tocado por profe 2


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

