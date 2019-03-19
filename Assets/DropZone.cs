using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        this.gameObject.GetComponentInChildren<Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        this.gameObject.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.4f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        this.GetComponent<Image>().color = Color.clear;
        eventData.pointerDrag.transform.SetParent(this.transform);
        /*Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }*/

    }
}
