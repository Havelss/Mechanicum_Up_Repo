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

    public List<SymbolData> symbols = new List<SymbolData>();
    public List<Button> symbolButtons = new List<Button>();


    private void Start()
    {
        foreach (Button button in symbolButtons)
        {
            if (button == null) continue;

            string symbolId = button.name.ToLower();
            SymbolData data = symbols.Find(s => s.id.ToLower() == symbolId);
            bool unlocked = data != null && data.isUnlocked;

            button.interactable = unlocked;

            Image img = button.GetComponent<Image>();
            if (img != null && data != null)
            {
                img.sprite = unlocked ? data.unlockedSprite : data.lockedSprite;
                img.color = unlocked ? Color.white : new Color(1, 1, 1, 0.3f);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetupTerminalButtons(SymbolTerminalController terminal)
    {
        foreach (Button button in symbolButtons)
        {
            if (button == null) continue;

            string symbolId = button.name.ToLower();
            SymbolData data = symbols.Find(s => s.id.ToLower() == symbolId);
            bool unlocked = data != null && data.isUnlocked;

            // 🔹 Cambiar sprite según estado
            Image img = button.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = unlocked ? data.unlockedSprite : data.lockedSprite;
                img.color = unlocked ? Color.white : new Color(1, 1, 1, 0.3f);
            }

            // 🔹 Desactivar si no está desbloqueado
            button.interactable = unlocked;
            button.onClick.RemoveAllListeners();

            // 🔹 Solo añadir evento si está desbloqueado
            if (unlocked)
            {
                button.onClick.AddListener(() =>
                {
                    terminal.AddSymbol(symbolId);
                });
            }
        }
    }

    public void UnlockSymbol(string id)
    {
        SymbolData symbol = symbols.Find(s => s.id.ToLower() == id.ToLower());
        if (symbol != null)
            symbol.isUnlocked = true;
    }
}
