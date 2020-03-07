using System.Collections.Generic;
using UnityEngine;

public abstract class Command // MonoBehaviour для корутин
{
    protected GameObject player;

    public Command(GameObject player)
    {
        this.player = player;
    }

    public abstract IEnumerator<YieldInstruction> Execute();
}

public class MoveCommand : Command
{
    private int arg;

    public MoveCommand(GameObject player, int arg) : base(player)
    {
        this.arg = arg;
    }

    public override IEnumerator<YieldInstruction> Execute()
    {
        float playerSpeed = player.GetComponent<Player>().playerSpeed;
        Vector2 target = (Vector2)player.transform.position + (Vector2)player.transform.up * arg;
        while (Vector2.Distance(player.transform.position, target) >= playerSpeed)
        {
            //Debug.Log("Moving " + player.transform.position + " to " + target);
            player.transform.position = Vector2.MoveTowards(player.transform.position, target, playerSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}

public class TurnCommand : Command
{
    private float arg;
    private Transform obj;

    public TurnCommand(GameObject player, float arg) : base(player)
    {
        this.arg = arg;
    }

    public TurnCommand(GameObject player, Transform lookAt) : base(player)
    {
        obj = lookAt;
    }

    public override IEnumerator<YieldInstruction> Execute()
    {
        //Debug.Log("Turn call");
        float rotationSpeed = player.GetComponent<Player>().rotationSpeed;
        if (obj) arg = Vector2.SignedAngle(player.transform.up, (obj.position - player.transform.position).normalized);
        Quaternion target = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z + arg);
        //Debug.Log($"Turning to {arg} deg ({target.eulerAngles})");
        while (Quaternion.Angle(player.transform.rotation, target) >= rotationSpeed)
        {
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, target, rotationSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        //Debug.Log("Turn exit");
    }
}

public class ShootCommand : Command
{
    private Transform bulletPrefab;

    public ShootCommand(GameObject player, Transform bulletPrefab) : base(player) 
    {
        this.bulletPrefab = bulletPrefab;
    }

    public override IEnumerator<YieldInstruction> Execute()
    {
        //Debug.Log("Shoot call");
        float reloadTime = player.GetComponent<Player>().reloadTime;
        float bulletSpeed = player.GetComponent<Player>().bulletSpeed;
        yield return new WaitForSeconds(reloadTime);
        Transform bullet = Object.Instantiate(bulletPrefab);
        bullet.position = player.transform.position + player.transform.up * 25;
        bullet.rotation = player.transform.rotation;
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
