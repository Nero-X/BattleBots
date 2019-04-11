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
    public GameObject ground;

    List<Transform> playerEvents = new List<Transform>();
    List<Transform> enemyEvents = new List<Transform>();
    const float playerSpeed = 1f;
    const float rotationSpeed = 1.2f;
    const float bulletSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(Back);
        GameObject content = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform.Find("ScriptPanel").Find("Viewport").Find("Content").gameObject;
        GameObject content2 = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform.Find("ScriptPanel").Find("Viewport").Find("Content2").gameObject;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).GetComponent<Draggable>().type == Draggable.Type.Movement) playerEvents.Add(content.transform.GetChild(i));
        }
        for (int i = 0; i < content2.transform.childCount; i++)
        {
            if (content2.transform.GetChild(i).GetComponent<Draggable>().type == Draggable.Type.Movement) enemyEvents.Add(content2.transform.GetChild(i));
        }
        foreach (Transform x in playerEvents) StartCoroutine(Execute(x, player));
        foreach (Transform x in enemyEvents) StartCoroutine(Execute(x, enemy));
        /*foreach (Transform x in playerEvents) new Process(x, player);
        foreach (Transform x in enemyEvents) new Process(x, enemy);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Back()
    {
        SceneManager.LoadScene("Setup", LoadSceneMode.Single);
    }

    IEnumerator<YieldInstruction> Execute(Transform init, GameObject player)
    {
        Transform instruction = init;
        while (instruction.name.Contains("Clone"))
        {
            switch (instruction.name)
            {
                case "Move(Clone)": yield return StartCoroutine(Move(player, Convert.ToInt32(instruction.GetComponentInChildren<Text>().text))); break; // Move((Vector2)player.transform.position + Vector2.up * 15, 10f)
                case "TurnR(Clone)": yield return StartCoroutine(Turn(player, -Convert.ToInt32(instruction.GetComponentInChildren<Text>().text))); break;
                case "TurnL(Clone)": yield return StartCoroutine(Turn(player, Convert.ToInt32(instruction.GetComponentInChildren<Text>().text))); break;
            }
            Debug.Log("Executed " + instruction.name);
            if (instruction.GetChild(0).childCount > 1) instruction = instruction.GetChild(0).GetChild(1);
            else instruction = init;
        }
    }

    /*public IEnumerator<float> MoveC(Vector3 targetPos, float time) // Move с использованием Lerp
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPos;

        for (float t = 0; t <= 1 * time; t += Time.deltaTime)
        {
            player.transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;
        }

        player.transform.position = endPos;
    }*/

    IEnumerator<WaitForSeconds> Move(GameObject player, int arg) // заменить тип arg на object или gameobject
    {
        //Debug.Log("Move call");
        Vector2 target = (Vector2)player.transform.position + (Vector2)player.transform.up * arg;
        while(Vector2.Distance(player.transform.position, target) >= playerSpeed)
        {
            //Debug.Log("Moving " + player.transform.position + " to " + target);
            player.transform.position = Vector2.MoveTowards(player.transform.position, target, playerSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        //Debug.Log("Move exit");
    }

    IEnumerator<WaitForSeconds> Turn(GameObject player, int arg)
    {
        //Debug.Log("Turn call");
        Quaternion target = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z + arg);
        while (Quaternion.Angle(player.transform.rotation, target) >= rotationSpeed)
        {
            //Debug.Log("Moving " + player.transform.position + " to " + target);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, target, rotationSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        //Debug.Log("Turn exit");
    }

    // Возможна подобная реализация процессов в будущем
    /*class Process : MonoBehaviour
    {
        Transform root;
        GameObject player;
        bool waiting = false;

        public Process(Transform root, GameObject player)
        {
            this.root = root;
            this.player = player;
            Execute();
        }

        void Execute()
        {
            Transform instruction = root;
            while (instruction.name.Contains("Clone"))
            {
                switch (instruction.name)
                {
                    case "Move(Clone)": { Debug.Log("Switch starts Move"); StartCoroutine(Move(player, Convert.ToInt32(instruction.GetComponentInChildren<Text>().text))); Debug.Log("Switch breaks"); break; } // Move((Vector2)player.transform.position + Vector2.up * 15, 10f)
                    case "TurnR(Clone)": StartCoroutine(Turn(player, -Convert.ToInt32(instruction.GetComponentInChildren<Text>().text))); break;
                    case "TurnL(Clone)": StartCoroutine(Turn(player, Convert.ToInt32(instruction.GetComponentInChildren<Text>().text))); break;
                }
                //Debug.Log("Executed " + instruction.name);
                if (instruction.childCount > 1) instruction = instruction.GetChild(0).GetChild(1);
                else break;
            }
            Debug.Log("Executed all instructions. Repeating...");
            Execute();
        }

        IEnumerator<WaitForSeconds> Move(GameObject player, int arg) // заменить тип arg на object или gameobject
        {
            Debug.Log("Move started");
            Vector2 target = (Vector2)player.transform.position + (Vector2)player.transform.up * arg;
            while (Vector2.Distance(player.transform.position, target) >= playerSpeed)
            {
                //Debug.Log("Moving " + player.transform.position + " to " + target);
                player.transform.position = Vector2.MoveTowards(player.transform.position, target, playerSpeed);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            Debug.Log("Move ended");
            yield break;
        }

        IEnumerator<WaitForSeconds> Turn(GameObject player, int arg)
        {
            Quaternion target = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z + arg);
            while (Mathf.Approximately(player.transform.rotation.x, target.x))
            {
                //Debug.Log("Moving " + player.transform.position + " to " + target);
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, target, playerSpeed);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            yield break;
        }
    }*/
}
