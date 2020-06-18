using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameMenu : MonoBehaviour
{
    public GameObject menu;
    public bool isOpened = false;
    public Button Continue;
    public Button back;

    void Start()
    {
        Continue.onClick.AddListener(ShowHideMenu);
        back.onClick.AddListener(Back);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ShowHideMenu();
        }
    }

    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        menu.SetActive(isOpened);
        Time.timeScale = isOpened ? 0 : 1;
    }

    void Back()
    {
        SceneManager.UnloadSceneAsync("Arena");
        GameObject canvas = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0];
        canvas.SetActive(true);
        Time.timeScale = 1;
    }
}
