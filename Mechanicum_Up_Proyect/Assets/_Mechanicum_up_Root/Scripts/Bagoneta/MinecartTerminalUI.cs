//using UnityEngine;
//using UnityEngine.UI;

//public class MinecartTerminalUI : MonoBehaviour
//{
//    [Header("Referencias")]
//    public MinecartController cart;       // La bagoneta controlada
//    public Button noButton;               // Botón para mover a la izquierda (-X)
//    public Button rightButton;            // Botón para mover a la derecha (+X)

//    private bool noPressed = false;
//    private bool rightPressed = false;

//    private void Start()
//    {
//        if (noButton != null)
//            noButton.onClick.AddListener(OnNoButtonPressed);

//        if (rightButton != null)
//            rightButton.onClick.AddListener(OnRightButtonPressed);
//    }

//    private void OnNoButtonPressed()
//    {
//        if (noPressed) return; // ya está activo
//        noPressed = true;
//        rightPressed = false;

//        // Mover la bagoneta a la izquierda
//        if (cart != null)
//            cart.MoveLeft();
//    }

//    private void OnRightButtonPressed()
//    {
//        if (rightPressed) return; // ya está activo
//        rightPressed = true;
//        noPressed = false;

//        // Mover la bagoneta a la derecha
//        if (cart != null)
//            cart.MoveRight();
//    }

//    // Método opcional para detener movimiento lateral
//    public void StopCartLateral()
//    {
//        noPressed = false;
//        rightPressed = false;
//        if (cart != null)
//            cart.StopLateral();
//    }
//}



/////////9/12/2025///////

//using UnityEngine;
//using UnityEngine.UI;

//public class MinecartTerminalUI : MonoBehaviour
//{
//    [Header("Referencias")]
//    public Button noButton;
//    public Button rightButton;

//    private MinecartController cart;       // Ahora privado, se asigna con SetCart

//    private bool noPressed = false;
//    private bool rightPressed = false;

//    private void Start()
//    {
//        if (noButton != null)
//            noButton.onClick.AddListener(OnNoButtonPressed);

//        if (rightButton != null)
//            rightButton.onClick.AddListener(OnRightButtonPressed);
//    }

//    public void SetCart(MinecartController newCart)
//    {
//        cart = newCart;
//    }

//    private void OnNoButtonPressed()
//    {
//        if (noPressed) return;
//        noPressed = true;
//        rightPressed = false;

//        if (cart != null)
//            cart.MoveLeft();
//    }

//    private void OnRightButtonPressed()
//    {
//        if (rightPressed) return;
//        rightPressed = true;
//        noPressed = false;

//        if (cart != null)
//            cart.MoveRight();
//    }

//    public void StopCartLateral()
//    {
//        noPressed = false;
//        rightPressed = false;
//        if (cart != null)
//            cart.StopLateral();
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
