using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject element;

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
        RectTransform rectTransform = eventData.pointerEnter.GetComponent<RectTransform>();
        //int dy = (int)Math.Round(Screen.height * (0.0559 + (Screen.height - 912) * (-0.00004763))); :D
        if (eventData.pointerEnter.name.Contains("Content") == false)
        {
            element.transform.position = new Vector3(eventData.pointerEnter.transform.position.x, eventData.pointerEnter.transform.position.y - (int)Math.Round(Screen.height * 0.05));
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
    }
}
