using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    public float delay = 2f; // задержка появления в секундах
    public float duration = 5f; // длительность показа, 0 = неограничено

    public void OnPointerEnter(PointerEventData e)
    {
        Tooltip.text = text;
        Invoke("Show", delay);
    }

    public void OnPointerExit(PointerEventData e) // не всегда срабатывает
    {
        CancelInvoke("Show");
        Tooltip.show = false;
    }

    void Show()
    {
        Tooltip.show = true;
        if (duration > 0) Invoke("Hide", duration);
    }

    void Hide()
    {
        Tooltip.show = false;
    }
}
