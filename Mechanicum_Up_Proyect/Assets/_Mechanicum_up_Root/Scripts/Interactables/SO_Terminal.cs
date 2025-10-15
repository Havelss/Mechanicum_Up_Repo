using UnityEngine;

public class SO_Terminal : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Abre Terminal");
        return true;
    }
}
