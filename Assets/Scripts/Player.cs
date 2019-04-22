using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public Text HPText;

    const int damage = 10;
    string botName;
    Transform canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform;
        botName = canvas.GetComponent<Setup>().titles[this.transform.GetSiblingIndex() - 1];
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
