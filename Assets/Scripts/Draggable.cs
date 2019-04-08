using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject element;
    //internal Type type;
    public Type type;

    public enum Type { Movement, Event, Control, Sensor, Operator}

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
        if (!(eventData.pointerEnter.name.Contains("Content") || eventData.pointerEnter.name.Contains("Image") || eventData.pointerEnter.name.Contains("Text"))) Destroy(element);
        element.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(eventData.pointerEnter.name == "Text") element.transform.SetParent(eventData.pointerEnter.transform.parent.parent);
        else element.transform.SetParent(eventData.pointerEnter.transform);
        RectTransform parentRectTransform = element.transform.parent.GetComponent<RectTransform>();
        RectTransform elementRectTransform = element.GetComponentInChildren<Image>().rectTransform;
        //int dy = (int)Math.Round(Screen.height * (0.0559 + (Screen.height - 912) * (-0.00004763))); :D
        if (eventData.pointerEnter.name.Contains("Content") == false)
        {
            element.transform.position = new Vector3(eventData.pointerEnter.transform.position.x + (elementRectTransform.sizeDelta.x - parentRectTransform.sizeDelta.x) * (Screen.height / 1200f),
                eventData.pointerEnter.transform.position.y - (int)Math.Round(Screen.height * 0.055));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        eventData.pointerEnter.GetComponentInChildren<Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        eventData.pointerEnter.GetComponentInChildren<Image>().color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerEnter.GetComponentInChildren<Image>().color = Color.white;
        ClearHighlited();
    }

    public void ClearHighlited()
    {
        GameObject nextParent = this.gameObject;
        while (!nextParent.name.Contains("Content"))
        {
            nextParent.GetComponentInChildren<Image>().color = Color.white;
            nextParent = nextParent.transform.parent.gameObject;
        }
    }
}
