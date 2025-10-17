using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolTerminalController : MonoBehaviour
{
    public static SymbolTerminalController Instance;

    [Header("Referencias")]
    public Text displayText; // Texto donde se muestra la secuencia escrita
    public Button executeButton; // Bot�n para ejecutar la combinaci�n

    private List<string> currentSequence = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (executeButton != null)
            executeButton.onClick.AddListener(ExecuteSequence);

        UpdateDisplay();
    }

    public void AddSymbol(string symbolID)
    {
        // A�adimos el s�mbolo a la secuencia
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

        string command = string.Join(",", currentSequence);

        Debug.Log($"Ejecutando comando: {command}");

        // Aqu� puedes manejar qu� pasa con cada combinaci�n
        TerminalCommandHandler.Instance.ProcessCommand(command);

        ClearSequence();
    }
}
