using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolManager : MonoBehaviour
{
    public static SymbolManager Instance;

    [System.Serializable]
    public class SymbolButton
    {
        public string id;
        public Button button;
        public Image symbolImage;   // opcional, por si quieres cambiar el sprite
        public Sprite lockedSprite;
        public Sprite unlockedSprite;
    }

    [Header("Lista de s�mbolos en la terminal")]
    public List<SymbolButton> symbols = new List<SymbolButton>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
            symbol.button.interactable = true;
            if (symbol.symbolImage && symbol.unlockedSprite)
                symbol.symbolImage.sprite = symbol.unlockedSprite;

            Debug.Log($"S�mbolo desbloqueado: {id}");
        }
    }

    private void LockSymbol(SymbolButton s)
    {
        s.button.interactable = false;
        if (s.symbolImage && s.lockedSprite)
            s.symbolImage.sprite = s.lockedSprite;
    }
}
