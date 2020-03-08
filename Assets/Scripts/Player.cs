using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public Text HPText;
    public float playerSpeed;
    public float rotationSpeed;
    public float bulletSpeed;
    public float reloadTime;
    public int damage;

    public Thread defaultThread;

    string botName;
    Transform canvas;
    internal int secondsAlive = 0;

    public delegate void EventHandler();
    public event EventHandler OnCollisionWithPlayer;
    public event EventHandler OnCollisionWithBullet;
    public event EventHandler OnTimer; // Вызывается каждую секунду
    public event EventHandler OnChangeHP; // Вызывается при любом изменении кол-ва HP

    // Start is called before the first frame update
    void Start()
    {
        canvas = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform;
        botName = canvas.GetComponent<Setup>().titles[this.name == "Player" ? 0 : 1];
        Arena arena = SceneManager.GetSceneAt(1).GetRootGameObjects().Where(x => x.name == "GameLogic").ToArray()[0].GetComponent<Arena>();

        // На старте игры присваиваем значения по умолчанию
        damage = arena.bulletDamage;
        playerSpeed = arena.playerSpeed;
        rotationSpeed = arena.rotationSpeed;
        bulletSpeed = arena.bulletSpeed;
        reloadTime = arena.reloadTime;

        InvokeRepeating("Timer", 0, 1);
    }

    // Вызов события таймера
    void Timer()
    {
        secondsAlive++;
        OnTimer?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Bullet(Clone)")
        {
            Destroy(collision.gameObject);
            HP -= damage;
            HPText.text = botName + " HP: " + HP;
            OnCollisionWithBullet?.Invoke();
            OnChangeHP?.Invoke();
        }
        if (collision.gameObject.tag == "Player")
        {
            OnCollisionWithPlayer?.Invoke();
        }
    }
}
