using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject element;
    public Transform parentToReturnTo;
    Color original;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.gameObject.name.Contains("Clone")) element = this.gameObject;
        else
        {
            element = Instantiate(this.gameObject, this.gameObject.transform.root);
            element.transform.GetComponentInChildren<Image>().SetNativeSize();
            element.transform.localScale = new Vector3(element.transform.localScale.x * 0.75f, element.transform.localScale.y * 0.75f);
        }
        element.GetComponent<CanvasGroup>().blocksRaycasts = false;
        parentToReturnTo = this.transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        element.transform.position = eventData.position;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        element.GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.SetParent(parentToReturnTo);
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
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }
}
