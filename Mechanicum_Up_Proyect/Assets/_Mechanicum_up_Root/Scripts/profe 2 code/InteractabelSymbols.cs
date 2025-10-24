using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/*
public class InteractabelSymbols : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text displayText;
    public Button executeButton;

    public string IDsymbol;

    public GameObject[] buttons;

    public List<string> currentSequence;

    public GameObject terminalCommandHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Active(string IDsymbol_)
    {
        IDsymbol = IDsymbol_;
       // buttons = GameObject.FindGameObjectsWithTag(IDsymbol);

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().onClick.AddListener(ExecuteSequence);
        }
    }

    public void ExecuteSequence()
    {
        if (currentSequence.Count == 0) return;

        string command = string.Join(",", currentSequence).Trim().ToLower();
        Debug.Log($"Ejecutando comando: {command}");

        if (TerminalCommandHandler.Instance != null)
        {
            // Pasamos el objeto que controla esta terminal
            TerminalCommandHandler.Instance.ProcessCommand(command, this);
        }
        else
        {
            Debug.LogError("No hay TerminalCommandHandler en la escena.");
        }

        ClearSequence();
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
}

*/ // tocado por profe 2

