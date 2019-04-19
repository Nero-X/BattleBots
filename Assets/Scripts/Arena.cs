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

    List<Transform> playerEvents = new List<Transform>();
    List<Transform> enemyEvents = new List<Transform>();
    const float playerSpeed = 1f;
    const float rotationSpeed = 1.2f;
    const float bulletSpeed = 300f;
    const float reload = 1.5f;
    Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(Back);
        canvas = SceneManager.GetSceneAt(0).GetRootGameObjects().Where(x => x.name == "Canvas").ToArray()[0].transform;
        GameObject content = canvas.Find("ScriptPanel").Find("Viewport").Find("Content").gameObject;
        GameObject content2 = canvas.Find("ScriptPanel").Find("Viewport").Find("Content2").gameObject;
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
                case "Move(Clone)": yield return StartCoroutine(Move(player, Convert.ToInt32(instruction.GetChild(0).Find("InputField").GetComponentInChildren<Text>().text))); break;
                case "TurnR(Clone)": yield return StartCoroutine(Turn(player, -Convert.ToInt32(instruction.GetChild(0).Find("InputField").GetComponentInChildren<Text>().text))); break;
                case "TurnL(Clone)": yield return StartCoroutine(Turn(player, Convert.ToInt32(instruction.GetChild(0).Find("InputField").GetComponentInChildren<Text>().text))); break;
                case "Shoot(Clone)": yield return StartCoroutine(Shoot(player)); break;
            }
            Debug.Log("Executed " + instruction.name);
            if (instruction.GetChild(0).GetChild(0).name.Contains("Clone")) instruction = instruction.GetChild(0).GetChild(0);
            else instruction = init;
        }
    }

    IEnumerator<WaitForSeconds> Move(GameObject player, int arg) // заменить тип arg на object или gameobject
    {
        Debug.Log("Move call");
        Vector2 target = (Vector2)player.transform.position + (Vector2)player.transform.up * arg;
        while(Vector2.Distance(player.transform.position, target) >= playerSpeed)
        {
            //Debug.Log("Moving " + player.transform.position + " to " + target);
            player.transform.position = Vector2.MoveTowards(player.transform.position, target, playerSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        Debug.Log("Move exit");
    }

    IEnumerator<WaitForSeconds> Turn(GameObject player, int arg)
    {
        Debug.Log("Turn call");
        Quaternion target = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z + arg);
        Debug.Log($"Turning {player} to {arg} deg ({target.eulerAngles})");
        while (Quaternion.Angle(player.transform.rotation, target) >= rotationSpeed)
        {
            //Debug.Log("Moving " + player.transform.position + " to " + target);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, target, rotationSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        Debug.Log("Turn exit");
    }

    IEnumerator<WaitForSeconds> Shoot(GameObject player)
    {
        Debug.Log("Shoot call");
        yield return new WaitForSeconds(reload);
        Transform bullet = Instantiate(bulletPrefab);
        bullet.position = player.transform.position + player.transform.up * 15;
        bullet.rotation = player.transform.rotation;
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
