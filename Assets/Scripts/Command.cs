using System.Collections.Generic;
using UnityEngine;

public abstract class Command
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
        Vector2 target = (Vector2)player.transform.position + (Vector2)player.transform.forward * arg;
        while (Vector2.Distance(player.transform.position, target) >= playerSpeed)
        {
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
        float rotationSpeed = player.GetComponent<Player>().rotationSpeed;
        if (obj) arg = Vector3.SignedAngle(player.transform.forward, (obj.position - player.transform.position).normalized, Vector3.forward);
        Quaternion startRotation = player.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(arg, Vector3.down);
        //Debug.Log($"Turn started. RotationSpeed = {rotationSpeed} Arg = {arg} StartRotation = {startRotation.eulerAngles} EndRotation = {endRotation.eulerAngles} EndRotationQ = {endRotation}");
        while (Quaternion.Angle(player.transform.rotation, endRotation) > 0)
        {
            //Debug.Log($"Current rotation: {player.transform.rotation.eulerAngles} Current Q rotation: {player.transform.rotation} Angle: {Quaternion.Angle(player.transform.rotation, endRotation)}");
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, endRotation, rotationSpeed);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
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
        float reloadTime = player.GetComponent<Player>().reloadTime;
        float bulletSpeed = player.GetComponent<Player>().bulletSpeed;
        yield return new WaitForSeconds(reloadTime);
        Transform bullet = Object.Instantiate(bulletPrefab);
        bullet.position = player.transform.position + player.transform.forward * 25;
        bullet.up = player.transform.forward;
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
