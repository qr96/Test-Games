using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceA<T>
{
    Queue<T> waitingQue = new Queue<T>();
    bool usingService = false;

    public void Enqueue(T obj)
    {
        waitingQue.Enqueue(obj);
    }

    public bool IsUsingService()
    {
        return usingService;
    }

    public int GetWaiterCount()
    {
        return waitingQue.Count;
    }

    public bool TryUsingService(out T waiter, Action<T, int> onRearrangeQue)
    {
        if (waitingQue.Count > 0)
        {
            usingService = true;
            waiter = waitingQue.Dequeue();
            OnRearrangeQue(onRearrangeQue);
            return true;
        }

        waiter = default;
        return false;
    }

    public void CompleteToUsingService()
    {
        usingService = false;
    }

    void OnRearrangeQue(Action<T, int> onRearrangeQue)
    {
        var waitingNumber = 0;
        foreach (var waiter in waitingQue)
            onRearrangeQue?.Invoke(waiter, waitingNumber++);
    }
}
