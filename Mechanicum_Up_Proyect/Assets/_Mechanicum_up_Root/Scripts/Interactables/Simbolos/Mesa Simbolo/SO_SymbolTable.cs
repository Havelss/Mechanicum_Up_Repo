using UnityEngine;

public class SO_SymbolTable : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Examinar mesa";
    [SerializeField] private string symbolID; 
    private bool collected = false;

    public string InteractionPrompt => prompt;

    public bool Interact(Interactor interactor)
    {
        if (collected) return false;

        // Desbloquear símbolo
        if (SymbolManager.Instance != null)
        {
            SymbolManager.Instance.UnlockSymbol(symbolID);
            collected = true; // evita desbloquear varias veces
            Debug.Log($"Símbolo obtenido: {symbolID}");
        }

        return true;
    }
}

