using UnityEngine;



/*
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

*/ // tocado por profe 2

public class SO_SymbolTable : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Examinar mesa"; // Texto que aparece al mirar el objeto
    [SerializeField] private string symbolID; // ID del símbolo que desbloquea este objeto
    private bool collected = false; // Para evitar desbloquear varias veces

    public string InteractionPrompt => prompt;

    public bool Interact(Interactor interactor)
    {
        if (collected) return false; // Ya recogido, no hace nada

        // Desbloquear símbolo en el SymbolManager global
        if (SymbolManager.Instance != null)
        {
            SymbolManager.Instance.UnlockSymbol(symbolID);
            collected = true; // Marcamos como recogido
            Debug.Log($"Símbolo desbloqueado desde SO_SymbolTable: {symbolID}");
        }
        else
        {
            Debug.LogWarning("No hay SymbolManager en la escena.");
        }

        return true;
    }
}
