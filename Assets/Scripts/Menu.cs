using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button newGame;
    public Button exit;

    // Start is called before the first frame update
    void Start()
    {
        newGame.onClick.AddListener(NewGame);
        exit.onClick.AddListener(Close);
    }

    // Update is called once per frame
    void Update()
    {
        
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
