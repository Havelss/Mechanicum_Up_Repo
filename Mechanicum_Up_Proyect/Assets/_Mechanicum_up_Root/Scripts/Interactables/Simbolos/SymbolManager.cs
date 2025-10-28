using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SymbolData
{
    public string id;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public bool isUnlocked = false;
}

public class SymbolManager : MonoBehaviour
{
    public static SymbolManager Instance { get; private set; }

    [Header("Símbolos globales")]
    public List<SymbolData> symbols = new List<SymbolData>();
    public List<Button> symbolButtons = new List<Button>(); // Prefabs o referencias a botones

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Configura los botones de UNA terminal específica
    public void SetupTerminalButtons(SymbolTerminalController terminal)
    {
        foreach (Button button in terminal.symbolButtons)
        {
            if (button == null) continue;

            string symbolId = button.name.ToLower();
            SymbolData data = symbols.Find(s => s.id.ToLower() == symbolId);
            bool unlocked = data != null && data.isUnlocked;

            button.interactable = unlocked;

            // Cambiar imagen según desbloqueo
            Image img = button.GetComponent<Image>();
            if (img != null)
                img.sprite = unlocked ? data.unlockedSprite : data.lockedSprite;

            button.onClick.RemoveAllListeners();
            if (unlocked)
            {
                button.onClick.AddListener(() => terminal.AddSymbol(symbolId));
            }
        }

        Debug.Log($"Configurando botones terminal: {terminal.name}");
    }

    // Desbloquea un símbolo global
    public void UnlockSymbol(string id)
    {
        SymbolData symbol = symbols.Find(s => s.id.ToLower() == id.ToLower());
        if (symbol != null)
        {
            symbol.isUnlocked = true;
            
        }
    }
}
