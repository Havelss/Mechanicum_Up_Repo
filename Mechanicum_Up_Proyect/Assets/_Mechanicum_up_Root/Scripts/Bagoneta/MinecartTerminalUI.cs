using UnityEngine;
using UnityEngine.UI;

public class MinecartTerminalUI : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button noButton;
    public Button rightButton;

    [Header("Referencia al carrito")]
    public MinecartController cart;

    private void Awake()
    {
        if (noButton != null)
            noButton.onClick.AddListener(() => OnNoPressed());

        if (rightButton != null)
            rightButton.onClick.AddListener(() => OnRightPressed());
    }

    private void OnNoPressed()
    {
        if (cart != null)
        {
            cart.MoveLeft();
            Debug.Log("[MinecartTerminal] Botón NO pulsado → MoveLeft");
        }
    }

    private void OnRightPressed()
    {
        if (cart != null)
        {
            cart.MoveRight();
            Debug.Log("[MinecartTerminal] Botón RIGHT pulsado → MoveRight");
        }
    }
}
