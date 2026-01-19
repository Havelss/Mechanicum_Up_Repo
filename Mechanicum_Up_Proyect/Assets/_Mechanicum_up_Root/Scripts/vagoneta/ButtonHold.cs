using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            onHold.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        onRelease?.Invoke();
    }
}
