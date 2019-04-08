using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Arena : MonoBehaviour
{
    public Button back;
    public GameObject player;
    public GameObject enemy;

    List<Transform> playerEvents = new List<Transform>();
    List<Transform> enemyEvents = new List<Transform>();
    const float playerSpeed = 1f;
    const float bulletSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(Back);
        GameObject content = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform.Find("ScriptPanel").Find("Viewport").Find("Content").gameObject;
        GameObject content2 = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform.Find("ScriptPanel").Find("Viewport").Find("Content2").gameObject;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).GetComponent<Draggable>().type == Draggable.Type.Event) playerEvents.Add(content.transform.GetChild(i));
        }
        for (int i = 0; i < content2.transform.childCount; i++)
        {
            if (content2.transform.GetChild(i).GetComponent<Draggable>().type == Draggable.Type.Event) enemyEvents.Add(content2.transform.GetChild(i));
        }
        //InvokeRepeating("MyUpdate", 1f, 0.1f);
        MyUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MyUpdate()
    {
        foreach (Transform x in playerEvents) Execute(x, player);
        foreach (Transform x in enemyEvents) Execute(x, enemy);
    }

    void Back()
    {
        SceneManager.LoadScene("Setup", LoadSceneMode.Single);
    }

    void Execute(Transform init, GameObject player)
    {
        Transform instruction = init;              // 
        while (instruction.name.Contains("Clone")) // Move((Vector2)player.transform.position + Vector2.up * 15, 10f)
        {
            switch (instruction.name)
            {
                case "Move(Clone)": { Debug.Log("Switch move"); StartCoroutine(Move(player, Convert.ToInt32(instruction.GetComponentInChildren<Text>().text))); Debug.Log("Switch move end"); break; }
            }
            Debug.Log("Executed " + instruction.name);
            if (instruction.childCount > 1) instruction = instruction.GetChild(0).GetChild(1);
            else break;
        }
    }

    public IEnumerator<float> MoveC(Vector3 targetPos, float time)
    {
        Debug.Log("Move call");
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPos;

        for (float t = 0; t <= 1 * time; t += Time.deltaTime)
        {
            player.transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;
        }

        player.transform.position = endPos;
    }

    IEnumerator<WaitForSeconds> Move(GameObject player, int arg) // заменить тип arg на object или gameobject
    {
        Debug.Log("Move call");
        Vector2 target = (Vector2)player.transform.position + Vector2.up * arg;
        while(Vector3.Distance(player.transform.position, target) >= playerSpeed)
        {
            Debug.Log("Moving " + player.transform.position + " to " + target);
            player.transform.position = Vector2.MoveTowards(player.transform.position, target, playerSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        yield break;
    }
}
