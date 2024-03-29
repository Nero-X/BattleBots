﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject element; // Перетаскиваемый объект
    public Type type; // Тип команды

    public enum Type { Movement, Event, Action}

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.hovered.Clear();
        if (this.gameObject.name.Contains("Clone")) element = this.gameObject; // если перетаскиваем команду из списка команд - создаём копию, иначе перетаскиваем сам объект
        else
        {
            element = Instantiate(this.gameObject, this.transform.root);
            element.Shape();
            element.GetComponent<Draggable>().type = (Type)Convert.ToInt16(this.transform.parent.parent.parent.parent.name.Reverse().ToArray()[1].ToString());
        }
        element.transform.SetParent(element.transform.root);
        element.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        element.transform.position = eventData.position; // следование за курсором
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.hovered.Count == 0) eventData.hovered.Add(eventData.pointerEnter); // фикс пустого списка hovered
        try
        {
            if (eventData.hovered.Exists(x => x.name.Contains("DZ") || x.name.Contains("Clone"))) // перетаскивать можно только на content или другую команду
            {
                element.GetComponent<CanvasGroup>().blocksRaycasts = true;

                // Проверки
                if (eventData.pointerEnter.name == "Text") element.transform.SetParent(eventData.pointerEnter.transform.parent.parent); // если перетащили на текст - задаем родителем Image
                else element.transform.SetParent(eventData.pointerEnter.transform);
                if (element.transform.parent.name == "Viewport") throw new UnityException();
                if(element.transform.parent.name == "Image" && element.transform.parent.Children().Count(x => x.name.Contains("Clone")) == 2
                    || element.GetComponent<Draggable>().type == Type.Event) // событие или если родительская команда не последняя в списке, не присоединять
                {
                    element.transform.SetParent(element.transform.root.Find("ScriptPanel/Viewport").Children().Where(x => x.gameObject.activeInHierarchy).First());
                    return;
                }

                element.transform.SetAsFirstSibling(); // дочерняя команда - первая в списке
                RectTransform parentRectTransform = element.transform.parent.GetComponent<RectTransform>();
                RectTransform elementRectTransform = element.GetComponentInChildren<Image>().rectTransform;
                if (eventData.pointerEnter.name.Contains("Content") == false) // если перетащили на команду, выравниваем по левому краю
                {
                    element.transform.position = new Vector3(eventData.pointerEnter.transform.position.x + (elementRectTransform.sizeDelta.x - parentRectTransform.sizeDelta.x) * (Screen.height / 900f),
                        eventData.pointerEnter.transform.position.y - (int)Math.Round(Screen.height * 0.055));
                }
            }
            else Destroy(element);
        }
        catch (UnityException) { Destroy(element); }
        ClearHighlited();
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

    // Cнимает выделение со всех команд
    public void ClearHighlited()
    {
        foreach(GameObject gameObject in FindObjectsOfType<GameObject>())
        {
            if (gameObject.name.Contains("Clone")) gameObject.GetComponentInChildren<Image>().color = Color.white;
        }
    }
}
