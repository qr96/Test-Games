using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEventTriggerA : MonoBehaviour
{
    Vector3 desPos;
    float eventRange;
    Action onArrive;

    // 콜백에서 새로운 콜백을 등록하는 경우 콜백이 제거되지 않도록 방어 장치
    bool needRemoveEvent; 

    void Update()
    {
        var leftDis = (desPos - transform.position).sqrMagnitude;

        if (leftDis < eventRange)
        {
            if (onArrive != null)
            {
                needRemoveEvent = true;
                onArrive.Invoke();
                if (needRemoveEvent)
                    onArrive = null;
            }
        }
    }

    public void SetTrigger(Vector3 desPos, float eventRange, Action onArrive)
    {
        this.desPos = desPos;
        this.eventRange = eventRange;
        this.onArrive = onArrive;

        needRemoveEvent = false;
    }
}
