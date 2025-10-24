using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
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
*/


/*
public class SymbolTerminalController : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text displayText;
    public Button executeButton;

    public List<string> currentSequence ;

    private void Start()
    {
        currentSequence = new List<string>();
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
        {
            // Pasamos el objeto que controla esta terminal
            //TerminalCommandHandler.Instance.ProcessCommand(command, this);
        }
        else
        {
            Debug.LogError("No hay TerminalCommandHandler en la escena.");
        }

        ClearSequence();
    }
}
*/  // tocado por profe 2


public class SymbolTerminalController : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text displayText;
    public Button executeButton;

    private List<string> currentSequence;

    private void Start()
    {
        currentSequence = new List<string>();

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
        {
            // Pasa esta terminal al handler para que sepa qué objeto controla
            TerminalCommandHandler.Instance.ProcessCommand(command, this);
        }
        else
        {
            Debug.LogError("No hay TerminalCommandHandler en la escena.");
        }

        ClearSequence();
    }
}

