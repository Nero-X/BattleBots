using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    public Button next;
    public Button back;
    public Image header;
    public Text title;

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
            header.color = Color.green;
            title.text = title1;
            // загрузить скрипт
        }
    }

}
