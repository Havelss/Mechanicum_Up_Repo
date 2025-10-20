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

        if (SymbolManager.Instance != null)
        {
            SymbolManager.Instance.UnlockSymbol(symbolID);
            collected = true;
            Debug.Log($"Símbolo recogido: {symbolID}");
        }

        return true;
    }
}
