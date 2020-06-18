using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Slider slider;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData e)
    {
        slider = gameObject.GetComponent<Slider>();
        Tooltip.text = slider.value.ToString();
        slider.onValueChanged.AddListener((float x) => Tooltip.text = x.ToString());
        Tooltip.show = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData e)
    {
        Tooltip.show = false;
    }
}
