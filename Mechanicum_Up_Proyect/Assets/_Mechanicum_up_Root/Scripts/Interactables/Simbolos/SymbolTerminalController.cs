using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolTerminalController : MonoBehaviour
{
    [Header("UI")]
    public Text displayText;
    public Button executeButton;
    public Button exitButton;
    public List<Button> symbolButtons = new List<Button>(); // Solo botones de esta terminal

    [HideInInspector] private MonoBehaviour controlledObject;

    public MonoBehaviour ControlledObject
    {
        get => controlledObject;
        set => controlledObject = value;
    }

    private List<string> currentSequence;

    private void Awake()
    {
        currentSequence = new List<string>();

        if (executeButton != null)
            executeButton.onClick.AddListener(() => ExecuteSequence(controlledObject));

        if (exitButton != null)
            exitButton.onClick.AddListener(() =>
            {
                var terminal = GetComponentInParent<SO_Terminal>();
                if (terminal != null)
                    terminal.CloseTerminal();
            });
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

    public void ExecuteSequence(MonoBehaviour target)
    {
        if (currentSequence.Count == 0 || target == null) return;

        string command = string.Join(",", currentSequence).Trim().ToLower();
        Debug.Log($"Ejecutando comando: {command}");

        if (target is Elevator elevator)
        {
            if (command == "up")
            {
                elevator.MoveUp();
            }
            else if (command == "no,up")
            {
                elevator.MoveDown();
            }
            else
            {
                Debug.LogWarning($"Comando desconocido: {command}");
            }
        }

        ClearSequence();
    }

}
