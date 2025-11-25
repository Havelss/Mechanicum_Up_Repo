using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolTerminalController : MonoBehaviour
{
    [Header("UI")]
    public Text displayText;                       // muestra la secuencia (modo fallback)
    public Button executeButton;
    public Button exitButton;

    [Header("Modo botones (legacy)")]
    public List<Button> symbolButtons = new List<Button>(); // botones Up/No/Left para compatibilidad

    [Header("Modo drag & drop (slots)")]
    public List<SymbolSlot> symbolSlots = new List<SymbolSlot>(); // si usas slots, se rellenan primero

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
                CloseTerminal();
            });
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void Update()
    {
        // Detectar si se presiona la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseTerminal();
        }
    }

    private void CloseTerminal()
    {
        var terminal = GetComponentInParent<SO_Terminal>();
        if (terminal != null)
        {
            terminal.CloseTerminal();
        }
    }

    public void AddSymbol(string symbolID)
    {
        if (string.IsNullOrEmpty(symbolID)) return;

        // Verificar si el símbolo ya está en uso en algún slot
        if (symbolSlots != null && symbolSlots.Count > 0)
        {
            foreach (var slot in symbolSlots)
            {
                if (slot != null && slot.currentSymbol == symbolID)
                {
                    Debug.LogWarning($"El símbolo '{symbolID}' ya está asignado a un slot.");
                    return; // No permitir duplicados
                }
            }

            // Rellenar el primer slot vacío
            foreach (var slot in symbolSlots)
            {
                if (slot != null && string.IsNullOrEmpty(slot.currentSymbol))
                {
                    // Asignar el sprite desde SymbolManager si existe
                    var img = slot.iconImage;
                    var dataSprite = SymbolManager.Instance?.GetSpriteFor(symbolID);
                    if (img != null && dataSprite != null)
                    {
                        img.sprite = dataSprite;
                        img.enabled = true;
                    }

                    slot.currentSymbol = symbolID;
                    return;
                }
            }
        }

        // Fallback: añadir a la secuencia textual si no hay slots disponibles
        currentSequence.Add(symbolID);
        UpdateDisplay();
    }

    public void ClearSequence()
    {
        currentSequence.Clear();
        UpdateDisplay();

        // limpiar slots también
        if (symbolSlots != null)
        {
            foreach (var s in symbolSlots)
                s?.ClearSlot();
        }

        // si tienes symbolButtons en modo legacy, nada que limpiar aquí (botones permanecen)
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = string.Join(",", currentSequence);
    }

    public void ExecuteSequence(MonoBehaviour target)
    {
        if (target == null)
        {
            Debug.LogWarning("No hay objeto controlado asignado a esta terminal.");
            return;
        }

        List<string> sequence = new List<string>();

        if (symbolSlots != null && symbolSlots.Count > 0)
        {
            foreach (var slot in symbolSlots)
            {
                if (slot != null && !string.IsNullOrEmpty(slot.currentSymbol))
                    sequence.Add(slot.currentSymbol.ToLower());
            }
        }
        else
        {
            sequence.AddRange(currentSequence.ConvertAll(s => s.ToLower()));
        }

        string command = string.Join(",", sequence);
        Debug.Log($"[Terminal] Ejecutando comando: {command}");

        if (target is GatoAnimationController gato) // 🔹 Caso del gato mecánico
        {
            gato.ExecuteTerminalCommand(command);
        }
        else if (target is Elevator elevator)
        {
            if (command == "up")
                elevator.MoveUp();
            else if (command == "no,up")
                elevator.MoveDown();
            else
                Debug.LogWarning($"Comando desconocido: {command}");
        }

        ClearSequence();
        CloseTerminal();
    }
}