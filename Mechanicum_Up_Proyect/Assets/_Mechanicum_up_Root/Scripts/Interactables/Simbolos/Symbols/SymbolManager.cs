using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
