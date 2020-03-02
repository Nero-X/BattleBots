using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread
{
    public bool IsStarted => Coroutine != null;
    public bool IsPaused = false;
    public bool IsLooped;
    
    private List<Command> Commands;
    private int Current;
    private MonoBehaviour Owner;
    private Coroutine Coroutine;

    public Thread(MonoBehaviour owner, List<Command> commands, bool loop)
    {
        Owner = owner;
        Commands = commands;
        IsLooped = loop;
    }

    private IEnumerator Process()
    {
        do
        {
            for (; Current < Commands.Count; Current++)
            {
                yield return Owner.StartCoroutine(Commands[Current].Execute());
            }
            Current = 0;
        } while (IsLooped);
        Coroutine = null;
    }

    public void Run()
    {
        Stop(true);
        Current = 0;
        Coroutine = Owner.StartCoroutine(Process());
    }

    public void Pause(bool forced)
    {
        Stop(forced);
        IsPaused = true;
    }

    public void Resume()
    {
        if(IsPaused)
        {
            Coroutine = Owner.StartCoroutine(Process());
            IsPaused = false;
        }
    }

    public void Stop(bool forced)
    {
        /*if (forced) Owner.StopAllCoroutines();
        else Owner.StopCoroutine(Coroutine);*/
        if(IsStarted)
        {
            Owner.StopCoroutine(Coroutine);
            IsPaused = false;
        }
    }
}
