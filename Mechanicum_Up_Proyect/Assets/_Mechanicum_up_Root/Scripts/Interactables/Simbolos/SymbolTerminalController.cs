using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolTerminalController : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text displayText;
    public Button executeButton;

    private List<string> currentSequence = new List<string>();

    private void Start()
    {
        if (executeButton != null)
            executeButton.onClick.AddListener(ExecuteSequence);

        UpdateDisplay();
    }

    public void AddSymbol(string symbolID)
    {
        currentSequence.Add(symbolID);
        UpdateDisplay();
    }

    public void ClearSequence()
    {
        currentSequence.Clear();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = string.Join(",", currentSequence);
    }

    public void ExecuteSequence()
    {
        if (currentSequence.Count == 0) return;

        string command = string.Join(",", currentSequence).Trim().ToLower();
        Debug.Log($"Ejecutando comando: {command}");

        if (TerminalCommandHandler.Instance != null)
            TerminalCommandHandler.Instance.ProcessCommand(command);
        else
            Debug.LogError("No hay TerminalCommandHandler en la escena.");

        ClearSequence();
    }
}
