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

    // Потоки команд "по умолчанию". TODO(?): Занести хранение (и выполнение?) потоков в Player.cs для поддержки множества игроков
    Thread default1;
    Thread default2;

    Transform canvas;

    private int _counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(Back);
        canvas = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform;

        // Получаем скрипты всех игроков
        Transform content = canvas.Find("ScriptPanel").Find("Viewport").Find("ContentDZ");
        Transform content2 = canvas.Find("ScriptPanel").Find("Viewport").Find("Content2DZ");

        // Создаем и запускаем потоки по умолчанию
        default1 = player.GetComponent<Player>().currentThread = new Thread(this, BuildCommandLists(player, content), true, 0, "def");
        default2 = enemy.GetComponent<Player>().currentThread = new Thread(this, BuildCommandLists(enemy, content2), true, 0, "def");
        default1.Run();
        default2.Run();
    }

    // Update is called once per frame
    void Update()
    {
        // Конец игры
        if (player == null || enemy == null)
        {
            default1.Stop(false);
            default2.Stop(false);
        }
    }

    void Back()
    {
        SceneManager.UnloadSceneAsync("Arena");
        canvas.gameObject.SetActive(true);
    }

    List<Command> BuildCommandLists(GameObject player, Transform content)
    {
        Transform _enemy = player == this.player ? enemy.transform : this.player.transform;
        List<Command> list = new List<Command>();
        foreach (Transform headCmd in content)
        {
            List<Command> lst = null;
            Transform cmdObj = headCmd;
            Command cmdClass = null;
            while (true)
            {
                switch (cmdObj.name)
                {
                        // Movement
                    case "Move(Clone)": cmdClass = new MoveCommand(player, Convert.ToInt32(cmdObj.GetArgs()[0])); break;
                    case "TurnR(Clone)": cmdClass = new TurnCommand(player, -Convert.ToSingle(cmdObj.GetArgs()[0])); break;
                    case "TurnL(Clone)": cmdClass = new TurnCommand(player, Convert.ToSingle(cmdObj.GetArgs()[0])); break;
                        // Actions
                    case "Shoot(Clone)": cmdClass = new ShootCommand(player, bulletPrefab); break;
                    case "Look at enemy(Clone)": cmdClass = new TurnCommand(player, _enemy); break;
                        // Events
                    case "OnCollisionWithBullet(Clone)":
                        lst = new List<Command>();
                        player.GetComponent<Player>().OnCollisionWithBullet += () => HandleEvent(player, new Thread(this, lst, false, 1, _counter++.ToString())); break;
                    case "OnCollisionWithPlayer(Clone)": 
                        lst = new List<Command>(); 
                        player.GetComponent<Player>().OnCollisionWithPlayer += () => HandleEvent(player, new Thread(this, lst, false, 1, _counter++.ToString())); break;
                    /*case "OnSuccessfulHit(Clone)": 
                        lst = new List<Command>(); 
                        _enemy.GetComponent<Player>().OnCollisionWithBullet += () => HandleEvent(player, new Thread(this, lst, false, 1)); break;
                    case "OnTimer(Clone)": 
                        lst = new List<Command>();
                        player.GetComponent<Player>().OnTimer += () =>
                        {
                            if (player.GetComponent<Player>().secondsAlive % Convert.ToInt32(headCmd.GetArgs()[1]) == 0) HandleEvent(player, new Thread(this, lst, false, Convert.ToInt32(cmdObj.GetArgs()[0])));
                        }; break;
                    case "OnChangeHP(Clone)": 
                        lst = new List<Command>();
                        player.GetComponent<Player>().OnChangeHP += () =>
                        {
                            if (player.GetComponent<Player>().HP == Convert.ToInt32(headCmd.GetArgs()[1])) HandleEvent(player, new Thread(this, lst, false, Convert.ToInt32(cmdObj.GetArgs()[0])));
                        }; break;
                    case "OnCollisionWithBounds(Clone)": 
                        lst = new List<Command>(); 
                        player.GetComponent<Bounds>().OnCollisionWithBounds += () => HandleEvent(player, new Thread(this, lst, false, 1)); break;*/
                }
                if (lst == null) list.Add(cmdClass);
                else if (cmdClass != null) lst.Add(cmdClass);
                Transform next = cmdObj.Next();
                if (next == null) break;
                else cmdObj = next;
            }
        }
        return list;
    }

    void HandleEvent(GameObject player, Thread thread)
    {
        Thread current = player.GetComponent<Player>().currentThread;
        
        // Сравнить приоритеты 
        if(thread.Priority <= current.Priority) // проверить после текущего
        {
            //Debug.Log("Waiting...");
            current.OnFinish += () => HandleEvent(player, thread);
        }
        else if (thread.Priority > current.Priority) // вытеснить текущий
        {
            //Debug.Log("Executing!");
            //Debug.Log($"Thread '{thread.Name}' pushed current thread '{current.Name}'");
            current.Pause(true);
            player.GetComponent<Player>().currentThread = thread;
            thread.Run();
            thread.OnFinish += () =>
            {
                player.GetComponent<Player>().currentThread = current;
                current.Resume();
            };
        }
    }
}
