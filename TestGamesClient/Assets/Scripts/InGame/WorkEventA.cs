using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkEventA : MonoBehaviour
{
    public TriggerEvent2D trigger;

    float workDuration;
    Action onWorkFinish;

    Coroutine workCo;

    private void Awake()
    {
        trigger.SetTriggerEvent(onEnter: StartWork, onExit: StopWork);
    }

    public void SetWorkEvent(float duration, Action onFinish)
    {
        workDuration = duration;
        onWorkFinish = onFinish;
    }

    void StartWork(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (workCo != null)
            StopCoroutine(workCo);
        workCo = StartCoroutine(WorkCo(workDuration, onWorkFinish));
    }

    void StopWork(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (workCo != null)
            StopCoroutine(workCo);
    }

    IEnumerator WorkCo(float duration, Action finish)
    {
        yield return new WaitForSeconds(duration);
        finish?.Invoke();
    }
}
