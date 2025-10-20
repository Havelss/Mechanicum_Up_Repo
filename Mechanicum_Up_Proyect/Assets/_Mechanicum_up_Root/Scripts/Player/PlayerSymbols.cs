using System.Collections.Generic;
using UnityEngine;

public class PlayerSymbols : MonoBehaviour
{
    public static PlayerSymbols Instance { get; private set; }
    private HashSet<string> collectedSymbols = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CollectSymbol(string symbolID)
    {
        collectedSymbols.Add(symbolID);
        Debug.Log($"Símbolo añadido a PlayerSymbols: {symbolID}");
    }

    public bool HasSymbol(string symbolID)
    {
        return collectedSymbols.Contains(symbolID);
    }
}
