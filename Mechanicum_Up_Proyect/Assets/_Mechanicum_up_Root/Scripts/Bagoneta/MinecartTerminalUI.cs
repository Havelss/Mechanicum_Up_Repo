using UnityEngine;
using UnityEngine.UI;

public class MinecartTerminalUI : MonoBehaviour
{
    [Header("Botones")]
    public Button noButton;
    public Button rightButton;

    private MinecartController cart;

    private bool noPressed = false;
    private bool rightPressed = false;

    private void Start()
    {
        if (noButton != null) noButton.onClick.AddListener(OnNoButtonPressed);
        if (rightButton != null) rightButton.onClick.AddListener(OnRightButtonPressed);
    }

    public void SetCart(MinecartController newCart) => cart = newCart;

    private void OnNoButtonPressed()
    {
        if (noPressed) return;
        noPressed = true;
        rightPressed = false;

        cart?.MoveLeft();
    }

    private void OnRightButtonPressed()
    {
        if (rightPressed) return;
        rightPressed = true;
        noPressed = false;

        cart?.MoveRight();
    }

    public void StopCartLateral()
    {
        noPressed = false;
        rightPressed = false;
        cart?.StopLateral();
    }
}
