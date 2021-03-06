﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;
using SimpleFileBrowser;

public class Setup : MonoBehaviour
{
    public Button next;
    public Button back;
    public Image header;
    public Text title;
    public Transform content;
    public Transform content2;
    public Transform scriptPanel;
    public Button save;
    public Button load;

    int player = 1; // Номер игрока, который сейчас создает скрипт. 0 - главное меню
    internal string[] titles = { "Bot1", "Bot2" }; // Названия ботов

    void Start()
    {
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);
        save.onClick.AddListener(Save);
        load.onClick.AddListener(Load);
        FileBrowser.SetFilters(false, new FileBrowser.Filter("JSON", ".json"));
    }

    private void Next()
    {
        player++;
        if (player == 2)
        {
            header.GetComponent<Image>().sprite = Resources.Load<Sprite>("Head_setup_2");
            title.text = titles[1];
            content.gameObject.SetActive(false);
            content2.gameObject.SetActive(true);
            scriptPanel.GetComponent<ScrollRect>().content = content2.GetComponent<RectTransform>();
        }
        else
        {
            player--;
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("Arena", LoadSceneMode.Additive);
        }
    }

    private void Back()
    {
        player--;
        if (player == 0)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else
        {
            header.GetComponent<Image>().sprite = Resources.Load<Sprite>("Head_setup_1");
            title.text = titles[0];
            content2.gameObject.SetActive(false);
            content.gameObject.SetActive(true);
            scriptPanel.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
        }
    }

    // Класс для сериализации команд
    [Serializable]
    public class Command
    {
        public float x;
        public float y;
        public string name;
        public List<string> args;
        public Command child;

        [JsonConstructor]
        public Command(float x, float y, string name, List<string> args, Command child)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.args = args;
            this.child = child;
        }

        public Command(Transform obj)
        {
            x = obj.localPosition.x;
            y = obj.localPosition.y;
            name = obj.name;
            args = obj.GetArgs();
            if(obj.Next() != null) child = new Command(obj.Next()); // если есть следующая команда - создаем для неё экземпляр класса храним ссылку на него в родительской команде
        }

        // Преобразует экземпляр класса в объект на сцене
        public void ToObject(Transform parent)
        {
            Transform original = GameObject.Find(name.Substring(0, name.Length - 7)).transform; // поиск исходного объекта по имени
            if (parent.name.Contains("Content") == false) parent = parent.GetChild(0); // если родитель - команда, то присоединяем к Image
            Transform clone = Instantiate(original, parent);
            clone.localPosition = new Vector3(x, y);
            clone.SetArgs(args); // записываем аргументы
            clone.GetComponent<Draggable>().type = (Draggable.Type)Convert.ToInt16(original.transform.parent.parent.parent.parent.name.Reverse().ToArray()[1].ToString()); // выставляем тип команды
            clone.gameObject.Shape();
            clone.SetAsFirstSibling();
            if(child != null) child.ToObject(clone); // если есть дочерняя команда - создаем её
        }
    }

    private void Save()
    {
        FileBrowser.ShowSaveDialog((path) =>
        {
            string res = Serialize(player == 1 ? content : content2);
            File.WriteAllText(path, res);
        }, null);
    }

    private string Serialize(Transform content)
    {
        List<Command> lst = new List<Command>();
        foreach(Transform tr in content)
        {            
            lst.Add(new Command(tr)); // вызываем конструктор для всех первых команд
        }
        return JsonConvert.SerializeObject(lst, Formatting.Indented);
    }

    private void Deserialize(string json, Transform content)
    {
        List<Command> lst = JsonConvert.DeserializeObject<List<Command>>(json);
        foreach (Command cmd in lst)
        {
            cmd.ToObject(content);
        }
    }

    private void Load()
    {
        FileBrowser.ShowLoadDialog((path) =>
        {
            Deserialize(File.ReadAllText(path), player == 1 ? content : content2);
        }, null);
    }
}