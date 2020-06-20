using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    public float delay = 2f; //задержка появления в секундах

    public void OnPointerEnter(PointerEventData e)
    {
        Tooltip.text = text;
        Invoke("Show", delay);
    }

    public void OnPointerExit(PointerEventData e)
    {
        CancelInvoke("Show");
        Tooltip.show = false;
    }

    void Show()
    {
        Tooltip.show = true;
    }
}
