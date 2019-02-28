using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button exit; 

    // Start is called before the first frame update
    void Start()
    {
        exit.onClick.AddListener(Close);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Close()
    {
        Application.Quit();
    }
}
