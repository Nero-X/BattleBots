using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Arena : MonoBehaviour
{
    public Button back;
    public GameObject player;
    public GameObject enemy;

    List<Transform> events = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(Back);
        GameObject content = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform.Find("Content").gameObject;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).GetComponent<Draggable>().type == Draggable.Type.Event) events.Add(content.transform.GetChild(i));
        }
        InvokeRepeating("MyUpdate", 1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MyUpdate()
    {
        foreach (Transform x in events) Execute(x);
    }

    void Back()
    {
        SceneManager.LoadScene("Setup", LoadSceneMode.Single);
    }

    void Execute(Transform init)
    {
        Transform instruction = init;
        /*switch (instruction.name)
        {
            case "Move(Clone)": 
        }*/
    }

    void Move()
    {

    }
}
