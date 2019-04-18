using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    public Button next;
    public Button back;
    public Image header;
    public Text title;
    public Transform content;
    public Transform content2;
    public Transform scriptPanel;

    int player = 1;
    string title1 = "Bot1";
    string title2 = "Bot2";

    // Start is called before the first frame update
    void Start()
    {
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);
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
            header.GetComponent<Image>().sprite = Resources.Load<Sprite>("Head_setup_2");
            title.text = title2;
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
            title.text = title1;
            content2.gameObject.SetActive(false);
            content.gameObject.SetActive(true);
            scriptPanel.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
        }
    }
}
