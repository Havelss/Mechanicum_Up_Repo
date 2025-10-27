using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolTerminalController : MonoBehaviour
{
    [Header("UI References")]
    public Text displayText;              // Texto que muestra los símbolos
    public Button executeButton;          // Botón Enter
    public Button exitButton;             // Botón Exit

    [HideInInspector] public MonoBehaviour controlledObject; // Asignado desde SO_Terminal

    private List<string> currentSequence = new List<string>();

    private void Awake()
    {
        UpdateDisplay();

        if (executeButton != null)
            executeButton.onClick.AddListener(OnExecutePressed);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitPressed);
    }

    private void OnExecutePressed()
    {
        ExecuteSequence(controlledObject);

        // Al ejecutar correctamente, cerramos la terminal
        var terminal = GetComponentInParent<SO_Terminal>();
        if (terminal != null)
            terminal.CloseTerminal();
    }

    private void OnExitPressed()
    {
        // Solo cerrar la terminal sin ejecutar nada
        var terminal = GetComponentInParent<SO_Terminal>();
        if (terminal != null)
            terminal.CloseTerminal();
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
        Debug.Log($"Ejecutando comando: {command}");

        if (target is Elevator elevator)
        {
            switch (command)
            {
                case "up":
                    elevator.MoveUp();
                    break;
                case "down":
                case "no,up":
                    elevator.MoveDown();
                    break;
                default:
                    Debug.LogWarning($"Comando desconocido: {command}");
                    break;
            }
        }

        // Limpiar secuencia tras ejecutar
        ClearSequence();
    }
}
