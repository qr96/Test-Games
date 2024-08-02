using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KStateMachine<T> where T : Enum
{
    T nowState;

    Dictionary<T, Action<T>> onEnterEvent = new Dictionary<T, Action<T>>();
    Dictionary<T, Action> onUpdateEvent = new Dictionary<T, Action> ();
    Dictionary<T, Action> onFixedUpdateEvent = new Dictionary<T, Action>();
    Dictionary<T, Action> onLeaveEvent = new Dictionary<T, Action>();

    public T GetState()
    {
        return nowState;
    }

    public void SetState(T state)
    {
        var prevState = nowState;
        nowState = state;

        if (onLeaveEvent.ContainsKey(prevState))
        {
            onLeaveEvent[nowState]?.Invoke();
        }
        if (onEnterEvent.ContainsKey(state))
        {
            onEnterEvent[state]?.Invoke(prevState);
        }
    }

    public void SetEvent(T state, Action<T> onEnter, Action onUpdate = null, Action onLeave = null)
    {
        if (onEnter != null)
            onEnterEvent.Add(state, onEnter);
        if (onUpdate != null)
            onUpdateEvent.Add(state, onUpdate);
        if (onLeave != null)
            onLeaveEvent.Add(state, onLeave);
    }

    public void Update()
    {
        if (onUpdateEvent.ContainsKey(nowState))
        {
            onUpdateEvent[nowState]?.Invoke();
        }
    }

    public void FixedUpdate()
    {
        if (onFixedUpdateEvent.ContainsKey(nowState))
        {
            onFixedUpdateEvent[nowState]?.Invoke();
        }
    }
}
