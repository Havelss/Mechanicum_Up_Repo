using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SymbolSlot : MonoBehaviour, IDropHandler
{
    public string currentSymbol = null;
    public Image iconImage;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableSymbol draggedSymbol = eventData.pointerDrag.GetComponent<DraggableSymbol>();

        if (draggedSymbol != null)
        {
            currentSymbol = draggedSymbol.symbolID;
            iconImage.sprite = draggedSymbol.GetComponent<Image>().sprite;
            iconImage.enabled = true;

            // Bloquear que se vuelva a arrastrar
            draggedSymbol.gameObject.SetActive(false);
        }
    }

    public void ClearSlot()
    {
        currentSymbol = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
    }
}
