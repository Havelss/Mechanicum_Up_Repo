using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
//public class SymbolData
//{
//    public string id;
//    public Sprite lockedSprite;
//    public Sprite unlockedSprite;
//    public bool isUnlocked = false;
//}

//public class SymbolManager : MonoBehaviour
//{
//    public static SymbolManager Instance { get; private set; }

//    [Header("Símbolos globales")]
//    public List<SymbolData> symbols = new List<SymbolData>();
//    public List<Button> symbolButtons = new List<Button>(); // Prefabs o referencias a botones

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//    }

//    // Configura los botones de UNA terminal específica
//    public void SetupTerminalButtons(SymbolTerminalController terminal)
//    {
//        foreach (Button button in terminal.symbolButtons)
//        {
//            if (button == null) continue;

//            string symbolId = button.name.ToLower();
//            SymbolData data = symbols.Find(s => s.id.ToLower() == symbolId);
//            bool unlocked = data != null && data.isUnlocked;

//            button.interactable = unlocked;

//            // Cambiar imagen según desbloqueo
//            Image img = button.GetComponent<Image>();
//            if (img != null)
//                img.sprite = unlocked ? data.unlockedSprite : data.lockedSprite;

//            button.onClick.RemoveAllListeners();
//            if (unlocked)
//            {
//                button.onClick.AddListener(() => terminal.AddSymbol(symbolId));
//            }
//        }

//        Debug.Log($"Configurando botones terminal: {terminal.name}");
//    }

//    // Desbloquea un símbolo global
//    public void UnlockSymbol(string id)
//    {
//        SymbolData symbol = symbols.Find(s => s.id.ToLower() == id.ToLower());
//        if (symbol != null)
//        {
//            symbol.isUnlocked = true;

//        }
//    }
//}


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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Devuelve el sprite apropiado (unlocked si desbloqueado, si no locked). Puede devolver null.
    public Sprite GetSpriteFor(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        var s = symbols.Find(x => x.id != null && x.id.ToLower() == id.ToLower());
        if (s == null) return null;
        return s.isUnlocked && s.unlockedSprite != null ? s.unlockedSprite : s.lockedSprite;
    }

    // Configura los botones de UNA terminal específica (modo legacy botón)
    public void SetupTerminalButtons(SymbolTerminalController terminal)
    {
        if (terminal == null)
        {
            Debug.LogWarning("[SymbolManager] Terminal nula en SetupTerminalButtons.");
            return;
        }

        foreach (Button button in terminal.symbolButtons)
        {
            if (button == null) continue;

            // Id a partir del nombre del botón (controla en inspector que sea "Up", "No", etc.)
            string symbolId = button.name;

            // normalizar
            SymbolData data = symbols.Find(s => s.id != null && s.id.ToLower() == symbolId.ToLower());
            bool unlocked = data != null && data.isUnlocked;

            // asignar sprite si hay imagen
            Image img = button.GetComponent<Image>();
            if (img != null && data != null)
                img.sprite = unlocked && data.unlockedSprite != null ? data.unlockedSprite : data.lockedSprite;

            button.onClick.RemoveAllListeners();
            button.interactable = unlocked;

            if (unlocked)
            {
                // captura local para evitar problema de cierre en el bucle
                string idCopy = symbolId;
                button.onClick.AddListener(() => terminal.AddSymbol(idCopy));
            }
        }

        Debug.Log($"[SymbolManager] Configurando botones terminal: {terminal.name}");
    }

    // Desbloquea un símbolo global
    public void UnlockSymbol(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        SymbolData symbol = symbols.Find(s => s.id != null && s.id.ToLower() == id.ToLower());
        if (symbol != null)
        {
            symbol.isUnlocked = true;
            Debug.Log($"[SymbolManager] 🔓 Símbolo '{id}' desbloqueado correctamente.");
        }
        else
        {
            Debug.LogWarning($"[SymbolManager] No se encontró el símbolo '{id}' para desbloquear.");
        }
    }
}

