using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMoverA : MonoBehaviour
{
    public float speed = 0.01f;
    public float correctionDis = 0.1f;
    public float stopDis = 0.004f;

    public Vector3 desPos;

    private void Awake()
    {
        desPos = transform.position;
    }

    private void Update()
    {
        var leftDis = (desPos - transform.position).sqrMagnitude;

        if (leftDis > correctionDis)
            transform.position = Vector3.MoveTowards(transform.position, desPos, speed);
        else if (leftDis > stopDis)
            transform.position = Vector3.Lerp(transform.position, desPos, 0.5f);
    }

    public void SetMover(float speed, float correctionDis, float stopDis)
    {
        this.speed = speed;
        this.correctionDis = correctionDis;
        this.stopDis = stopDis;
    }

    public void Move(Vector3 desPos)
    {
        this.desPos = desPos;
    }
}
