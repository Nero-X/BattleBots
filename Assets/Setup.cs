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
        if (player == 1)
        {
            header.color = Color.red;
            // очистить скрипт
            player++;
        }
        else
        {

        }
    }

    private void Back()
    {
        if (player == 1)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else
        {
            header.color = Color.green;
            // загрузить скрипт
            player--;
        }
    }

}
