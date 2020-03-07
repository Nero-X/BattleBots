using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread
{
    public bool IsStarted => ProcessCoroutine != null;
    public bool IsPaused = false;
    public bool IsLooped;

    public delegate void EventHandler();
    public event EventHandler OnFinish;
    
    private List<Command> Commands;
    private int Current;
    private MonoBehaviour Owner;
    private Coroutine ProcessCoroutine;
    private Coroutine CommandCoroutine;

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
                yield return CommandCoroutine = Owner.StartCoroutine(Commands[Current].Execute());
            }
            Current = 0;
        } while (IsLooped);
        ProcessCoroutine = null;
        OnFinish?.Invoke();
    }

    public void Run()
    {
        Stop(true);
        Current = 0;
        ProcessCoroutine = Owner.StartCoroutine(Process());
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
            ProcessCoroutine = Owner.StartCoroutine(Process());
            IsPaused = false;
        }
    }

    public void Stop(bool forced)
    {
        if(IsStarted)
        {
            if (forced) Owner.StopCoroutine(CommandCoroutine);
            Owner.StopCoroutine(ProcessCoroutine);
            IsPaused = false;
        }
    }
}
