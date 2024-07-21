using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingServiceA : MonoBehaviour
{
    public GameObject serviceZone;
    public GameObject waitingZone;

    int maxAvailable;
    Action onNeedCharge;
    Action<GuestA, int> onRearrangeQue;

    Queue<GuestA> waitingQue = new Queue<GuestA>();

    int nowAvailable = 0;
    bool usingService = false;

    public void SetSevice(int maxAvailable, Action onNeedCharge, Action<GuestA, int> onRearrangeQue)
    {
        this.maxAvailable = maxAvailable;
        this.onNeedCharge = onNeedCharge;
        this.onRearrangeQue = onRearrangeQue;
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

    public bool TryUsingService(out GuestA waiter)
    {
        if (IsAvailable() && GetWaiterCount() > 0)
        {
            usingService = true;
            nowAvailable--;
            waiter = waitingQue.Dequeue();
            RearrangeQue();
            return true;
        }

        waiter = default;
        return false;
    }

    public void CompleteToUsingService()
    {
        if (nowAvailable <= 0) onNeedCharge?.Invoke();
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

    void RearrangeQue()
    {
        var waitingNumber = 0;
        foreach (var waiter in waitingQue)
            onRearrangeQue?.Invoke(waiter, waitingNumber++);
    }
}
