using UnityEngine;


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

