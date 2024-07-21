using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingServiceA : MonoBehaviour
{
    public GameObject serviceZone;
    public GameObject waitingZone;

    int maxAvailable;

    //WaitingQue<GuestA> waitingQue = new WaitingQue<GuestA>();
    Queue<GuestA> waitingQue = new Queue<GuestA>();

    int nowAvailable = 0;
    bool usingService = false;

    public void SetSevice(int maxAvailable)
    {
        this.maxAvailable = maxAvailable;
    }

    public void ChargeAvailableCount()
    {
        nowAvailable = maxAvailable;
    }

    public void Enqueue(GuestA t)
    {
        waitingQue.Enqueue(t);
    }

    public bool IsAvailable()
    {
        return !usingService && nowAvailable > 0;
    }

    public int GetWaiterCount()
    {
        return waitingQue.Count;
    }

    public bool TryUsingService(out GuestA waiter, Action<GuestA, int> onRearrangeQue)
    {
        if (IsAvailable() && GetWaiterCount() > 0)
        {
            usingService = true;
            nowAvailable--;
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

    public GameObject GetWaitingZone()
    {
        return waitingZone;
    }

    public GameObject GetServiceZone()
    {
        return serviceZone;
    }

    void OnRearrangeQue(Action<GuestA, int> onRearrangeQue)
    {
        var waitingNumber = 0;
        foreach (var waiter in waitingQue)
            onRearrangeQue?.Invoke(waiter, waitingNumber++);
    }
}
