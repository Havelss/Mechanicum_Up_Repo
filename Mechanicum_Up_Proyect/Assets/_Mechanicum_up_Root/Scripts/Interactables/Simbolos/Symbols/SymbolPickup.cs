using UnityEngine;


public class SymbolPickup : MonoBehaviour
{
    [SerializeField] private string symbolID;
    [SerializeField] private SymbolManager targetManager; // referencia expl�cita

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetManager != null)
            {
                targetManager.UnlockSymbol(symbolID);
                Debug.Log($"S�mbolo {symbolID} desbloqueado en {targetManager.name}");
            }
            else
            {
                Debug.LogWarning("No se asign� ning�n SymbolManager en el SymbolPickup.");
            }
        }
    }
}

