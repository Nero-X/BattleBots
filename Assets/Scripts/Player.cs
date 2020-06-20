using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    public float rotationSpeed;
    public float bulletSpeed;
    public float reloadTime;
    public int bulletDamage;
    public Text HPText;
    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (value != hp)
            {
                hp = value;
                HPText.text = botName + " HP: " + hp;
                OnChangeHP?.Invoke();
            }
        }
    }

    public Thread currentThread;

    string botName;
    Transform canvas;
    internal int secondsAlive = 0;

    public delegate void EventHandler();
    public event EventHandler OnCollisionWithPlayer;
    public event EventHandler OnCollisionWithBullet;
    public event EventHandler OnTimer; // Вызывается каждую секунду
    public event EventHandler OnChangeHP; // Вызывается при любом изменении кол-ва HP

    void Start()
    {
        canvas = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform;
        botName = canvas.GetComponent<Setup>().titles[this.name == "Player" ? 0 : 1];
        Arena arena = SceneManager.GetSceneAt(1).GetRootGameObjects().Where(x => x.name == "GameLogic").ToArray()[0].GetComponent<Arena>();

        // На старте игры присваиваем значения по умолчанию
        bulletDamage = arena.bulletDamage;
        playerSpeed = arena.playerSpeed;
        rotationSpeed = arena.rotationSpeed;
        bulletSpeed = arena.bulletSpeed;
        reloadTime = arena.reloadTime;
        HP = arena.HP;

        if(!currentThread.IsRunning) currentThread.Run(); // процесс OnChangeHP(Clone) запускается 2 раза. Возможное решение: при запуске проверять isRunning

        // Вариант реализации с событием OnStart (currentThread.Run() не нужен, сразу запускается процесс OnStart(Clone), который, по завершению, запустит процесс default)
        //OnStart?.Invoke();

        InvokeRepeating("Timer", 0, 1);
    }

    // Вызов события таймера
    void Timer()
    {
        secondsAlive++;
        OnTimer?.Invoke();
    }

    void Update()
    {
        if (HP <= 0)
        {
            currentThread.Stop(true);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            HP -= collision.gameObject.GetComponent<Bullet>().Damage;
            OnCollisionWithBullet?.Invoke();
        }
        if (collision.gameObject.tag == "Player")
        {
            OnCollisionWithPlayer?.Invoke();
        }
    }
}
