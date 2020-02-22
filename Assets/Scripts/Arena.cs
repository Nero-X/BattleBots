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
    public Transform bulletPrefab;

    // Значения по умолчанию
    public float playerSpeed = 1f;
    public float rotationSpeed = 2f;
    public float bulletSpeed = 300f;
    public int bulletDamage = 10;
    public float reloadTime = 0.75f;

    // TODO: массив игроков и массив списков команд
    List<Transform> playerEvents = new List<Transform>();
    List<Transform> enemyEvents = new List<Transform>();
    List<List<Command>> player1Commands;
    List<List<Command>> player2Commands;
    
    Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(Back);
        canvas = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform;
        Transform content = canvas.Find("ScriptPanel").Find("Viewport").Find("ContentDZ");
        Transform content2 = canvas.Find("ScriptPanel").Find("Viewport").Find("Content2DZ");
        player1Commands = BuildCommandLists(player, content);
        player2Commands = BuildCommandLists(enemy, content2);
        /*
        for (int i = 0; i < content.transform.childCount; i++)
        {
            //if (content.transform.GetChild(i).GetComponent<Draggable>().type == Draggable.Type.Movement) playerEvents.Add(content.transform.GetChild(i));
            playerEvents.Add(content.transform.GetChild(i));
        }
        for (int i = 0; i < content2.transform.childCount; i++)
        {
            //if (content2.transform.GetChild(i).GetComponent<Draggable>().type == Draggable.Type.Movement) enemyEvents.Add(content2.transform.GetChild(i));
            enemyEvents.Add(content2.transform.GetChild(i));
        }
        foreach (Transform x in playerEvents) StartCoroutine(Execute(x, player));
        foreach (Transform x in enemyEvents) StartCoroutine(Execute(x, enemy));
        */
        StartCoroutine(StartProcessor(player1Commands));
        StartCoroutine(StartProcessor(player2Commands));
    }

    IEnumerator<YieldInstruction> StartProcessor(List<List<Command>> commands)//////////////////////
    {
        bool needToStop = false;
        if (commands.Count == 0) needToStop = true;
        while(needToStop == false)
        {
            foreach(List<Command> lst in commands)
            {
                foreach(Command cmd in lst)
                {
                    yield return StartCoroutine(cmd.Execute());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || enemy == null) StopAllCoroutines();
    }

    void Back()
    {
        SceneManager.UnloadSceneAsync("Arena");
        canvas.gameObject.SetActive(true);
    }

    IEnumerator<YieldInstruction> Execute(Transform init, GameObject player)
    {
        Transform instruction = init;
        while (instruction.name.Contains("Clone"))
        {
            switch (instruction.name)
            {
                case "Move(Clone)": yield return StartCoroutine(Move(player, Convert.ToInt32(instruction.GetChild(0).Find("InputField (Arg0)").GetComponentInChildren<Text>().text))); break;
                case "TurnR(Clone)": yield return StartCoroutine(Turn(player, -Convert.ToInt32(instruction.GetChild(0).Find("InputField (Arg0)").GetComponentInChildren<Text>().text))); break;
                case "TurnL(Clone)": yield return StartCoroutine(Turn(player, Convert.ToInt32(instruction.GetChild(0).Find("InputField (Arg0)").GetComponentInChildren<Text>().text))); break;
                case "Shoot(Clone)": yield return StartCoroutine(Shoot(player)); break;
                case "Look at enemy(Clone)": yield return StartCoroutine(Turn(player, Vector2.SignedAngle(player.transform.up, ((player == this.player ? enemy.transform.position : this.player.transform.position) - player.transform.position).normalized))); break;
            }
            Transform next = instruction.Next();
            instruction = next == null ? init : next;
        }
    }

    List<List<Command>> BuildCommandLists(GameObject player, Transform content)
    {
        List<List<Command>> lists = new List<List<Command>>();
        foreach(Transform headCmd in content)
        {
            List<Command> lst = new List<Command>();
            Transform cmdObj = headCmd;
            Command cmdClass = null;
            while(true)
            {
                switch (cmdObj.name)
                {
                    case "Move(Clone)": cmdClass = new MoveCommand(player, Convert.ToInt32(cmdObj.GetArgs()[0])); break;
                    case "TurnR(Clone)": cmdClass = new TurnCommand(player, -Convert.ToInt32(cmdObj.GetArgs()[0])); break;
                    case "TurnL(Clone)": cmdClass = new TurnCommand(player, Convert.ToInt32(cmdObj.GetArgs()[0])); break;
                    case "Shoot(Clone)":  break;
                    case "Look at enemy(Clone)": break;
                }
                lst.Add(cmdClass);
                Transform next = cmdObj.Next();
                if (next == null) break;
                else cmdObj = next;
            }
            lists.Add(lst);
        }
        return lists;
    }

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

    IEnumerator<WaitForSeconds> Turn(GameObject player, float arg)
    {
        //Debug.Log("Turn call");
        Quaternion target = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z + arg);
        //Debug.Log($"Turning to {arg} deg ({target.eulerAngles})");
        while (Quaternion.Angle(player.transform.rotation, target) >= rotationSpeed)
        {
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, target, rotationSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        //Debug.Log("Turn exit");
    }

    IEnumerator<WaitForSeconds> Shoot(GameObject player)
    {
        //Debug.Log("Shoot call");
        yield return new WaitForSeconds(reloadTime);
        Transform bullet = Instantiate(bulletPrefab);
        bullet.position = player.transform.position + player.transform.up * 25;
        bullet.rotation = player.transform.rotation;
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
