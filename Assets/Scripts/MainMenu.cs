using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button newGame;
    public Button exit;

    void Start()
    {
        newGame.onClick.AddListener(NewGame);
        exit.onClick.AddListener(Close);
    }

    void NewGame()
    {
        SceneManager.LoadScene("Setup", LoadSceneMode.Single);
    }

    void Close()
    {
        Application.Quit();
    }
}
