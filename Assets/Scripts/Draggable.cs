using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject element;
    //internal Type type;
    public Type type;

    public enum Type { Movement, Event, Action}

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.hovered.Clear();
        if (this.gameObject.name.Contains("Clone")) element = this.gameObject;
        else
        {
            element = Instantiate(this.gameObject, this.transform.root);
            element.GetComponentInChildren<Image>().SetNativeSize();
            RectTransform rectTransform = element.transform.GetChild(0).GetComponent<RectTransform>();
            float ratio = rectTransform.rect.width / rectTransform.rect.height;
            rectTransform.sizeDelta = new Vector2(30 * ratio, 30);
            element.GetComponent<Draggable>().type = (Type)Convert.ToInt16(this.transform.parent.parent.parent.parent.name.Reverse().ToArray()[1].ToString());
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
        if (eventData.hovered.Exists(x => x.name.Contains("DZ")) || eventData.hovered.Count == 0)
        {
            element.GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (eventData.pointerEnter.name == "Text") element.transform.SetParent(eventData.pointerEnter.transform.parent.parent);
            else element.transform.SetParent(eventData.pointerEnter.transform);
            element.transform.SetAsFirstSibling();
            RectTransform parentRectTransform = element.transform.parent.GetComponent<RectTransform>();
            RectTransform elementRectTransform = element.GetComponentInChildren<Image>().rectTransform;
            if (eventData.pointerEnter.name.Contains("Content") == false)
            {
                element.transform.position = new Vector3(eventData.pointerEnter.transform.position.x + (elementRectTransform.sizeDelta.x - parentRectTransform.sizeDelta.x) * (Screen.height / 900f),
                    eventData.pointerEnter.transform.position.y - (int)Math.Round(Screen.height * 0.055));
            }
        }
        else Destroy(element);
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
