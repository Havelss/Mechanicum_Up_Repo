//using UnityEngine;
//using UnityEngine.UI;

//public class MinecartTerminalUI : MonoBehaviour
//{
//    [Header("Botones")]
//    public Button noButton;
//    public Button rightButton;

//    private MinecartController cart;

//    private bool noPressed = false;
//    private bool rightPressed = false;

//    private void Start()
//    {
//        if (noButton != null) noButton.onClick.AddListener(OnNoButtonPressed);
//        if (rightButton != null) rightButton.onClick.AddListener(OnRightButtonPressed);
//    }

//    public void SetCart(MinecartController newCart) => cart = newCart;

//    private void OnNoButtonPressed()
//    {
//        if (noPressed) return;
//        noPressed = true;
//        rightPressed = false;

//        cart?.MoveLeft();
//    }

//    private void OnRightButtonPressed()
//    {
//        if (rightPressed) return;
//        rightPressed = true;
//        noPressed = false;

//        cart?.MoveRight();
//    }

//    public void StopCartLateral()
//    {
//        noPressed = false;
//        rightPressed = false;
//        cart?.StopLateral();
//    }
//}

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
            noButton.onClick.AddListener(() => { noPressed = true; rightPressed = false; cart?.MoveLeft(); });
            noButton.gameObject.AddComponent<ButtonHold>().Setup(() => { noPressed = true; rightPressed = false; cart?.MoveLeft(); },
                                                                  () => { noPressed = false; cart?.StopLateral(); });
        }
        if (rightButton != null)
        {
            rightButton.onClick.AddListener(() => { rightPressed = true; noPressed = false; cart?.MoveRight(); });
            rightButton.gameObject.AddComponent<ButtonHold>().Setup(() => { rightPressed = true; noPressed = false; cart?.MoveRight(); },
                                                                     () => { rightPressed = false; cart?.StopLateral(); });
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

// Script auxiliar para hold
public class ButtonHold : MonoBehaviour
{
    private System.Action onHold;
    private System.Action onRelease;
    private bool isHolding = false;

    public void Setup(System.Action holdAction, System.Action releaseAction)
    {
        onHold = holdAction;
        onRelease = releaseAction;
    }

    private void Update()
    {
        if (isHolding && onHold != null)
            onHold();
    }

    public void OnPointerDown() { isHolding = true; }
    public void OnPointerUp() { isHolding = false; onRelease?.Invoke(); }
}