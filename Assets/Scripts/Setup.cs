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

    // Update is called once per frame
    void Update()
    {

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

    private void Deserialize(string json)
    {

    }

    private void Load()
    {
        
        
    }

    private void ShowHelp()
    {

    }
}

public static class Utility
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
}
