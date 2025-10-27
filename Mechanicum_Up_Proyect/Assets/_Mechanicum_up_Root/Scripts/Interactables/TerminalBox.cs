using UnityEngine;

[DisallowMultipleComponent]
public class TerminalBox : MonoBehaviour
{
    [Header("El objeto que controla esta caja")]
    [Tooltip("El objeto recibirá los comandos")]
    public MonoBehaviour controlledObject;

    [Header("Prompt (opcional override)")]
    public string prompt = "E";

    // Método auxiliar para obtener prompt desde Interactor
    public string GetPrompt() => prompt;

    // Devuelve el objeto asignado (puede ser null)
    public MonoBehaviour GetControlledObject() => controlledObject;
}
