using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread
{
    public bool IsRunning => ProcessCoroutine != null;
    public bool IsPaused = false;
    public bool IsLooped;

    public delegate void EventHandler();
    public event EventHandler OnFinish; // Событие выполняется по завершению выполнения потока. Событие не выполняется для зацикленных потоков
    
    private List<Command> Commands; // Список команд
    private int Current; // Индекс выполняющейся команды в списке
    private MonoBehaviour Owner;
    private Coroutine ProcessCoroutine; // Корутина потока
    private Coroutine CommandCoroutine; // Корутина команды

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
        if(IsRunning)
        {
            Stop(forced);
            IsPaused = true;
        }
    }

    public void Resume()
    {
        if(IsPaused)
        {
            ProcessCoroutine = Owner.StartCoroutine(Process());
            IsPaused = false;
        }
    }

    // forced = true - принудительная(мнгновенная) остановка. forced = false - подождать оканчания выполнения текущей команды и завершить
    public void Stop(bool forced)
    {
        if(IsRunning)
        {
            if (forced) Owner.StopCoroutine(CommandCoroutine);
            Owner.StopCoroutine(ProcessCoroutine);
            CommandCoroutine = null;
            ProcessCoroutine = null;
            IsPaused = false;
        }
    }
}
