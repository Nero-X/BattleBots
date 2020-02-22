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
    int arg;
    float playerSpeed;

    public MoveCommand(GameObject player, int arg) : base(player)
    {
        this.arg = arg;
    }

    public override IEnumerator<YieldInstruction> Execute()
    {
        playerSpeed = player.GetComponent<Player>().playerSpeed;
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
    int arg;
    float rotationSpeed;

    public TurnCommand(GameObject player, int arg) : base(player)
    {
        this.arg = arg;
    }

    public override IEnumerator<YieldInstruction> Execute()
    {
        //Debug.Log("Turn call");
        rotationSpeed = player.GetComponent<Player>().rotationSpeed;
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
