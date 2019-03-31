using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject element;
    Color original;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.gameObject.name.Contains("Clone")) element = this.gameObject;
        else
        {
            element = Instantiate(this.gameObject, this.transform.root);
            element.transform.GetComponentInChildren<Image>().SetNativeSize();
            element.transform.localScale = new Vector3(element.transform.localScale.x * 0.75f, element.transform.localScale.y * 0.75f);
        }
        element.transform.SetParent(element.transform.root);
        element.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        element.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        element.GetComponent<CanvasGroup>().blocksRaycasts = true;
        element.transform.SetParent(eventData.pointerEnter.transform);
        element.transform.localPosition.Set(0, 10, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        //original = this.gameObject.GetComponentInChildren<Image>().color;
        this.gameObject.GetComponentInChildren<Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        this.gameObject.GetComponentInChildren<Image>().color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (this.name.Contains("Placeholder")) this.GetComponent<Image>().color = Color.clear;
        eventData.pointerDrag.transform.SetParent(this.transform);
    }
}
