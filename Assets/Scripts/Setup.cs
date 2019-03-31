using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    public Button next;
    public Button back;
    public Image header;
    public Text title;
    public Button add;
    public GameObject panel;
    public Transform Content;

    int player = 1;
    string title1 = "Bot1";
    string title2 = "Bot2";

    // Start is called before the first frame update
    void Start()
    {
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);
        add.onClick.AddListener(Add);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Next()
    {
        player++;
        if (player == 2)
        {
            header.color = Color.red;
            title.text = title2;
            // очистить скрипт
        }
        else
        {

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
            header.color = new Color(0, 0.5647059f, 0, 1);
            title.text = title1;
            // загрузить скрипт
        }
    }

    private void Add()
    {
        if (Content.childCount < 25)
        {
            GameObject panelCopy = Instantiate(panel, Content);
            panelCopy.GetComponentInChildren<Text>().text = (Content.childCount - 1) + ":";
            panelCopy.transform.SetSiblingIndex(Content.childCount - 2);
        }
    }
}
