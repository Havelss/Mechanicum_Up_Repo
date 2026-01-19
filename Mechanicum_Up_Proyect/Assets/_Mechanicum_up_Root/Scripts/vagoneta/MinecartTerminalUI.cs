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

    public void SetCart(MinecartController newCart) => cart = newCart;

    public void StopCartLateral()
    {
        noPressed = false;
        rightPressed = false;
        cart?.StopLateral();
    }
}
