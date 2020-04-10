using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject menu;
    public bool isOpened = false;
    public Button Continue;

    void Start()
    {
        Continue.onClick.AddListener(ShowHideMenu);
    }

    void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ShowHideMenu();
            //Input.G
        }
    }

    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        //GetComponent<Canvas>().enabled = isOpened; //Включение или отключение Canvas
        menu.SetActive(isOpened);
    }

   
}
