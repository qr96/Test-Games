using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingServiceA : MonoBehaviour
{
    public GameObject serviceZone;
    public GameObject waitingZone;

    WaitingQue<GuestA> waitingQue = new WaitingQue<GuestA>();

    public void Enqueue(GuestA t)
    {
        waitingQue.Enqueue(t);
    }

    public bool IsUsingService()
    {
        return waitingQue.IsUsingService();
    }

    public int GetWaiterCount()
    {
        return waitingQue.GetWaiterCount();
    }

    public bool TryUsingService(out GuestA waiter, Action<GuestA, int> onRearrangeQue)
    {
        return waitingQue.TryUsingService(out waiter, onRearrangeQue);
    }

    public void CompleteToUsingService()
    {
        waitingQue.CompleteToUsingService();
    }

    public GameObject GetWaitingZone()
    {
        return waitingZone;
    }

    public GameObject GetServiceZone()
    {
        return serviceZone;
    }
}
