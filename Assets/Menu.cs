using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button exit;
    public Button next;
    public Scene setup;

    // Start is called before the first frame update
    void Start()
    {
        exit.onClick.AddListener(Close);
        next.onClick.AddListener(Next);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Close()
    {
        Application.Quit();
    }

    void Next()
    {
        SceneManager.LoadScene("Setup", LoadSceneMode.Single);
    }
}
