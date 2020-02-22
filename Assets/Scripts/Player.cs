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

    string botName;
    Transform canvas;
    
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
        }
    }
}
