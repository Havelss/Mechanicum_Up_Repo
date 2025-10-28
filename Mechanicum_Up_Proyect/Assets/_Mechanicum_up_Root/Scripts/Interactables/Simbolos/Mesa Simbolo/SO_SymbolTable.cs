using UnityEngine;

public class SO_SymbolTable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "E";
    [SerializeField] private string symbolID;

    public string InteractionPrompt => interactionPrompt;

    public void Interact(Interactor interactor)
    {
        if (SymbolManager.Instance != null)
        {
            SymbolManager.Instance.UnlockSymbol(symbolID);
            Debug.Log($"Símbolo recogido: {symbolID}");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SymbolManager.Instance no está asignado en la escena.");
        }
    }
}

