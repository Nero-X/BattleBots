﻿using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    public float delay = 2f; //задержка появления в секундах

    void IPointerEnterHandler.OnPointerEnter(PointerEventData e)
    {
        Tooltip.text = text;
        Invoke("Show", delay);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData e)
    {
        CancelInvoke("Show");
        Tooltip.show = false; 
    }

    void Show()
    {
        Tooltip.show = true;
    }
}
