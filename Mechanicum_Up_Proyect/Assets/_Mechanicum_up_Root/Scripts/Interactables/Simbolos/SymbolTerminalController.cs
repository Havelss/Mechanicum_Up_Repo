using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
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


*/


public class SymbolTerminalController : MonoBehaviour
{
    [Header("UI")]
    public Text displayText;
    public Button executeButton;
    public Button exitButton;
    public List<Button> symbolButtons = new List<Button>();

    [HideInInspector] public MonoBehaviour ControlledObject;

    private List<string> currentSequence;

    private void Awake()
    {
        currentSequence = new List<string>();

        if (executeButton != null)
            executeButton.onClick.AddListener(() => ExecuteSequence(ControlledObject));

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
        if (currentSequence.Count == 0 || target == null)
        {
            Debug.LogWarning("No hay comando o el objeto controlado es nulo.");
            return;
        }

        string command = string.Join(",", currentSequence).Trim().ToLower();
        Debug.Log($"[Terminal] Ejecutando comando: {command}");

        bool executedSuccessfully = false;

        // Comprueba si el objeto es un ascensor
        if (target is Elevator elevator)
        {
            if (command == "up")
            {
                elevator.MoveUp();
                executedSuccessfully = true;
            }
            else if (command == "no,up")
            {
                elevator.MoveDown();
                executedSuccessfully = true;
            }
            else
            {
                Debug.LogWarning($"[Terminal] Comando desconocido: {command}");
            }
        }

        // 🔹 Si la secuencia fue válida → cerrar terminal
        if (executedSuccessfully)
        {
            var terminal = GetComponentInParent<SO_Terminal>();
            if (terminal != null)
                terminal.CloseTerminal();
        }

        ClearSequence();
    }
}
