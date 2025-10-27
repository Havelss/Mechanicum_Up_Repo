using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolTerminalController : MonoBehaviour
{
    [Header("UI References")]
    public Text displayText;              // Texto que muestra los símbolos
    public Button executeButton;          // Botón Enter
    public Button exitButton;             // Botón Exit

    private List<string> currentSequence;
    public MonoBehaviour controlledObject; // Asignado desde SO_Terminal

    private void Awake()
    {
        currentSequence = new List<string>();
        UpdateDisplay();

        if (executeButton != null)
            executeButton.onClick.AddListener(() => ExecuteSequence(controlledObject));

        if (exitButton != null)
            exitButton.onClick.AddListener(() =>
            {
                // Cerrar la terminal que contiene este controller
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

        // Ejemplo con Elevator, puedes añadir más tipos si quieres
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
