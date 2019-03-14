using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject element;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.gameObject.name.Contains("Clone")) element = this.gameObject;
        else
        {
            element = Instantiate(this.gameObject, this.gameObject.transform.root);
            element.transform.GetComponentInChildren<Image>().SetNativeSize();
            element.transform.localScale = new Vector3(element.transform.localScale.x * 0.75f, element.transform.localScale.y * 0.75f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        element.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
