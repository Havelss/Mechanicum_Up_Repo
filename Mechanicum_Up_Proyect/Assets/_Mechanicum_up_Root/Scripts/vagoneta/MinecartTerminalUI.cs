using UnityEngine;
using UnityEngine.UI;

public class MinecartTerminalUI : MonoBehaviour
{
    [Header("Botones")]
    public Button noButton;
    public Button rightButton;

    [Header("Cursor")]
    [Tooltip("Sprite del cursor en estado normal")]
    public Texture2D cursorNormal;
    [Tooltip("Sprite del cursor cuando mantienes presionado el clic")]
    public Texture2D cursorClick;
    [Tooltip("Punto de activación del clic (0,0 es arriba-izquierda)")]
    public Vector2 hotspot = Vector2.zero;

    private MinecartController cart;

    private bool noPressed = false;
    private bool rightPressed = false;

    // estado local del cursor
    private bool cursorActive = false;

    private void Start()
    {
        if (noButton != null)
        {
            var holdNo = noButton.gameObject.AddComponent<ButtonHold>();
            holdNo.Setup(
                () => { noPressed = true; rightPressed = false; cart?.MoveLeft(); },
                () => { noPressed = false; cart?.StopLateral(); }
            );
        }

        if (rightButton != null)
        {
            var holdRight = rightButton.gameObject.AddComponent<ButtonHold>();
            holdRight.Setup(
                () => { rightPressed = true; noPressed = false; cart?.MoveRight(); },
                () => { rightPressed = false; cart?.StopLateral(); }
            );
        }
    }

    private void OnEnable()
    {
        ActivateCursor();
    }

    private void OnDisable()
    {
        DeactivateCursor();
    }

    private void Update()
    {
        // Cambiar sprite mientras se mantiene pulsado el botón izquierdo
        if (!cursorActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            SetCursorTexture(cursorClick);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetCursorTexture(cursorNormal);
        }
    }

    private void ActivateCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetCursorTexture(cursorNormal);
        cursorActive = true;
    }

    private void DeactivateCursor()
    {
        // Restaurar cursor del sistema
        SetCursorTexture(null);
        cursorActive = false;
        // No forzamos ocultar/lockear; si lo deseas, descomenta:
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetCursorTexture(Texture2D tex)
    {
        if (tex != null)
            Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void SetCart(MinecartController newCart) => cart = newCart;

    public void StopCartLateral()
    {
        noPressed = false;
        rightPressed = false;
        cart?.StopLateral();
    }
}
