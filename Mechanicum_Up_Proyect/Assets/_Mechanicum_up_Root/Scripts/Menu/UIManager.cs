using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private bool anyMenuOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMenuState(bool isOpen)
    {
        anyMenuOpen = isOpen;
        UpdateCursorState();
    }

    public bool IsAnyMenuOpen()
    {
        return anyMenuOpen;
    }

    private void UpdateCursorState()
    {
        if (anyMenuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
