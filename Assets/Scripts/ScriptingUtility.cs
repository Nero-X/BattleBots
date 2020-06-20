using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class ScriptingUtility
{
    // Возвращает все дочерние элементы объекта в виде перечисления
    public static IEnumerable<Transform> Children(this Transform parent)
    {
        foreach (Transform tr in parent)
        {
            yield return tr;
        }
    }

    // Возвращает объект следующей команды в списке или null, если команда последняя
    public static Transform Next(this Transform parent)
    {
        if (parent.GetChild(0).childCount > 0)
        {
            if (parent.GetChild(0).GetChild(0).name.Contains("Clone")) return parent.GetChild(0).GetChild(0);
            else return null;
        }
        else return null;
    }

    // Придать объекту команды правильный размер
    public static void Shape(this GameObject obj)
    {
        obj.GetComponentInChildren<Image>().SetNativeSize();
        RectTransform rectTransform = obj.transform.GetChild(0).GetComponent<RectTransform>();
        float ratio = rectTransform.rect.width / rectTransform.rect.height;
        rectTransform.sizeDelta = new Vector2(30 * ratio, 30);
    }

    // Возвращает список аргументов объекта команды
    public static List<string> GetArgs(this Transform command)
    {
        List<Transform> argObjects = command.GetChild(0).Children().Where(x => x.name.Contains("Arg")).ToList();
        List<string> args = new List<string>();
        foreach(Transform transform in argObjects)
        {
            switch (transform.name.Split(' ')[0])
            {
                case "InputField": args.Add(transform.GetComponent<InputField>().text); break;
                case "Slider": args.Add(transform.GetComponent<Slider>().value.ToString()); break;
            }
        }
        return args;
    }

    // Задаёт значения аргументов объекта команды
    public static void SetArgs(this Transform command, List<string> argsList)
    {
        argsList.Reverse();
        Stack<string> args = new Stack<string>(argsList);

        // Сортировка на всякий случай
        List<Transform> argObjects = command.GetChild(0).Children().Where(x => x.name.Contains("Arg")).OrderBy(x => x.name.Reverse().ToArray()[1]).ToList();

        foreach (Transform transform in argObjects)
        {
            switch (transform.name.Split(' ')[0])
            {
                case "InputField": transform.GetComponent<InputField>().text = args.Pop(); break;
                case "Slider": transform.GetComponent<Slider>().value = float.Parse(args.Pop()); break;
            }
        }
    }
}