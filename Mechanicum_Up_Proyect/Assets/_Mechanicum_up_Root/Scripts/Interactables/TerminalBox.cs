using UnityEngine;

public class TerminalBox : MonoBehaviour
{
    [SerializeField] private MonoBehaviour controlledObject; // el ascensor u otro objeto

    public MonoBehaviour GetControlledObject() => controlledObject;

    public string GetPrompt() => "E";
}
