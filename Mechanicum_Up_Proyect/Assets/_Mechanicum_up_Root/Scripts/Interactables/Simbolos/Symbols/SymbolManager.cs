using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
[System.Serializable]


public class SymbolButton
{
    public string id;
    public Button button;
    public Image symbolImage;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
}

public class SymbolManager : MonoBehaviour
{
    public static SymbolManager Instance { get; private set; }

    public List<SymbolButton> symbols = new List<SymbolButton>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var s in symbols)
            LockSymbol(s);
    }

    public void SetupTerminalButtons(SymbolTerminalController terminal)
    {
        if (terminal == null)
        {
            Debug.LogError("TerminalController es null al configurar botones");
            return;
        }

        foreach (var s in symbols)
        {
            if (s.button == null)
            {
                Debug.LogWarning($"Símbolo {s.id} no tiene botón asignado");
                continue;
            }

            s.button.onClick.RemoveAllListeners();
            s.button.interactable = true;
            s.button.onClick.AddListener(() => terminal.AddSymbol(s.id));

            if (s.symbolImage != null && s.unlockedSprite != null)
                s.symbolImage.sprite = s.unlockedSprite;
        }
    }

    private void LockSymbol(SymbolButton s)
    {
        s.button.interactable = false;
        if (s.symbolImage && s.lockedSprite)
            s.symbolImage.sprite = s.lockedSprite;
    }

    public void UnlockSymbol(string id)
{
    var symbol = symbols.Find(s => s.id == id);
    if (symbol != null)
    {
        symbol.button.interactable = true;
        if (symbol.symbolImage && symbol.unlockedSprite)
            symbol.symbolImage.sprite = symbol.unlockedSprite;
        
        Debug.Log($"Símbolo desbloqueado: {id}");
    }
}
}

*/

/*
[System.Serializable]
public class SymbolButton
{
    public string id;
    public Button button;
    public Image symbolImage;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    [HideInInspector] public bool isUnlocked = false; // controla si el símbolo está desbloqueado
}

public class SymbolManager : MonoBehaviour
{
    public static SymbolManager Instance { get; private set; }

    public List<SymbolButton> symbols = new List<SymbolButton>();

    private void Awake()
    {
        Instance = this;

        // Inicialmente todos bloqueados
        foreach (var s in symbols)
        {
            LockSymbol(s);
        }
    }

    public void UnlockSymbol(string id)
    {
        var symbol = symbols.Find(s => s.id == id);
        if (symbol != null)
        {
            symbol.isUnlocked = true;
            if (symbol.symbolImage && symbol.unlockedSprite != null)
                symbol.symbolImage.sprite = symbol.unlockedSprite;

            Debug.Log($"Símbolo desbloqueado: {id}");
        }
    }

    private void LockSymbol(SymbolButton s)
    {
        s.isUnlocked = false;
        if (s.symbolImage && s.lockedSprite != null)
            s.symbolImage.sprite = s.lockedSprite;
    }

    public void SetupTerminalButtons(SymbolTerminalController terminal)
    {
        if (terminal == null)
        {
            Debug.LogError("TerminalController es null al configurar botones");
            return;
        }

        foreach (var s in symbols)
        {
            if (s.button == null)
            {
                Debug.LogWarning($"Símbolo {s.id} no tiene botón asignado");
                continue;
            }

            s.button.onClick.RemoveAllListeners();
            Debug.Log("Removealllisteners funcianó");
            s.button.interactable = s.isUnlocked;
            Debug.Log("interactable funcianó");

            s.button.onClick.AddListener(() =>
            {
                terminal.AddSymbol(s.id);
                Debug.Log("add.listener funcianó");
            });

            if (s.symbolImage != null)
            {
                s.symbolImage.sprite = s.isUnlocked && s.unlockedSprite != null
                    ? s.unlockedSprite
                    : s.lockedSprite;
            }
        }
    }
}

*/ // tocado por profe 2


[System.Serializable]
public class SymbolButton
{
    public string id;
    public Button button;
    public Image symbolImage;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    [HideInInspector] public bool isUnlocked = false; // indica si el símbolo está desbloqueado
}

public class SymbolManager : MonoBehaviour
{
    public static SymbolManager Instance { get; private set; }

    [Header("Símbolos disponibles")]
    public List<SymbolButton> symbols = new List<SymbolButton>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Inicialmente bloqueamos todos los símbolos
        foreach (var s in symbols)
            LockSymbol(s);
    }

    public void UnlockSymbol(string id)
    {
        var symbol = symbols.Find(s => s.id == id);
        if (symbol != null)
        {
            symbol.isUnlocked = true;
            if (symbol.symbolImage != null && symbol.unlockedSprite != null)
                symbol.symbolImage.sprite = symbol.unlockedSprite;

            if (symbol.button != null)
                symbol.button.interactable = true;

            Debug.Log($"Símbolo desbloqueado: {id}");
        }
    }

    private void LockSymbol(SymbolButton s)
    {
        s.isUnlocked = false;
        if (s.symbolImage != null && s.lockedSprite != null)
            s.symbolImage.sprite = s.lockedSprite;

        if (s.button != null)
            s.button.interactable = false;
    }

    // Configura los botones de la terminal (terminal global)
    public void SetupTerminalButtons(SymbolTerminalController terminal)
    {
        if (terminal == null)
        {
            Debug.LogError("TerminalController es null al configurar botones");
            return;
        }

        foreach (var s in symbols)
        {
            if (s.button == null)
            {
                Debug.LogWarning($"Símbolo {s.id} no tiene botón asignado");
                continue;
            }

            s.button.onClick.RemoveAllListeners();
            s.button.interactable = s.isUnlocked;

            s.button.onClick.AddListener(() =>
            {
                terminal.AddSymbol(s.id);
            });

            if (s.symbolImage != null)
                s.symbolImage.sprite = s.isUnlocked && s.unlockedSprite != null ? s.unlockedSprite : s.lockedSprite;
        }
    }
}



