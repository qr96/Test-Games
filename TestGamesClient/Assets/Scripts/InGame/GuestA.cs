using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestA : MonoBehaviour
{
    public SmoothMoverA mover;
    public PositionEventTriggerA moveTrigger;

    float stopDis = 0.004f;

    Coroutine actionCo;

    WaitingServiceA nowUsingService;

    public void SetMover(float speed, float correctionDis, float stopDis)
    {
        this.stopDis = stopDis;

        mover.SetMover(speed, correctionDis, stopDis);
    }

    public void SetNowUsingService(WaitingServiceA nowUsingService)
    {
        this.nowUsingService = nowUsingService;
    }

    public WaitingServiceA GetNowUsingService()
    {
        return nowUsingService;
    }    

    public void DoMove(Vector3 desPos, Action onFinish)
    {
        mover.Move(desPos);
        moveTrigger.SetTrigger(desPos, stopDis, onFinish);
    }

    public void DoAction(float actionTime, Action onFinish)
    {
        if (actionCo != null)
            StopCoroutine(actionCo);

        actionCo = StartCoroutine(ActionCo(actionTime, onFinish));
    }

    IEnumerator ActionCo(float actionTime, Action onFinish)
    {
        yield return new WaitForSeconds(actionTime);
        onFinish?.Invoke();
    }
}
