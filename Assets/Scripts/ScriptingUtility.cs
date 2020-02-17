using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ScriptingUtility
{
    public static IEnumerable<Transform> Children(this Transform parent)
    {
        foreach (Transform tr in parent)
        {
            yield return tr;
        }
    }

    public static Transform Next(this Transform parent)
    {
        if (parent.GetChild(0).childCount > 0)
        {
            if (parent.GetChild(0).GetChild(0).name.Contains("Clone")) return parent.GetChild(0).GetChild(0);
            else return null;
        }
        else return null;
    }

    public static void Shape(this GameObject obj)
    {
        obj.GetComponentInChildren<Image>().SetNativeSize();
        RectTransform rectTransform = obj.transform.GetChild(0).GetComponent<RectTransform>();
        float ratio = rectTransform.rect.width / rectTransform.rect.height;
        rectTransform.sizeDelta = new Vector2(30 * ratio, 30);
    }
}
