using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;

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
    public Button help;

    int player = 1;
    internal string[] titles = { "Bot1", "Bot2" };

    // Start is called before the first frame update
    void Start()
    {
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);
        save.onClick.AddListener(Save);
        load.onClick.AddListener(Load);
        help.onClick.AddListener(ShowHelp);
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
            args = obj.GetChild(0).Children().Where(x => x.name.Contains("Arg")).Select(x => x.GetComponent<InputField>().text).ToList();
            if(obj.Next() != null) child = new Command(obj.Next());
        }

        public void ToObject(Transform parent)
        {
            Transform original = GameObject.Find(name.Substring(0, name.Length - 7)).transform;
            Transform clone = Instantiate(original, parent);
            clone.localPosition = new Vector3(x, y);
            clone.GetChild(0).Children().Where(x => x.name.Contains("Arg")).ToList().ForEach(x => x.GetComponent<InputField>().text = args[Convert.ToInt32("" + x.name.Reverse().ToArray()[1])]);
            clone.gameObject.Shape();
            if(child != null) child.ToObject(clone);
        }
    }

    private void Save()
    {
        string path = EditorUtility.SaveFilePanel("Сохранить как...", "", "script", "json");
        if (path.Length != 0)
        {
            string res = Serialize(player == 1 ? content : content2);
            File.WriteAllText(path, res);
        }
    }

    private string Serialize(Transform content)
    {
        List<Command> lst = new List<Command>();
        foreach(Transform tr in content)
        {            
            lst.Add(new Command(tr));
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
        string path = EditorUtility.OpenFilePanel("Открыть...", "", "json");
        if(path.Length != 0)
        {
            Deserialize(File.ReadAllText(path), player == 1 ? content : content2);
        }
    }

    private void ShowHelp()
    {

    }
}