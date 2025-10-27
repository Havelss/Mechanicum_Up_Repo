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
            return;

        foreach (var s in symbols)
        {
            if (s.button == null) continue;

            s.button.onClick.RemoveAllListeners();

            // Desbloquea según el estado global
            s.button.interactable = s.isUnlocked;

            // Añade listener para **esta terminal**
            s.button.onClick.AddListener(() => terminal.AddSymbol(s.id));

            // Cambia sprite según desbloqueo
            if (s.symbolImage != null)
                s.symbolImage.sprite = s.isUnlocked && s.unlockedSprite != null
                    ? s.unlockedSprite
                    : s.lockedSprite;
        }
    }
}



