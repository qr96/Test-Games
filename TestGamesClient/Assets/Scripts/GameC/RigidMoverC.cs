using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RigidMoverC : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    Rigidbody2D rigid;

    public float speed = 0.01f;
    public float correctionDis = 0.1f;
    public float stopDis = 0.004f;

    public Vector3 desPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        desPos = transform.position;
    }

    private void Update()
    {
        var leftDis = (desPos - transform.position).sqrMagnitude;

        if (leftDis > stopDis)
            rigid.velocity = (desPos - transform.position).normalized * speed;
        else
            rigid.velocity = Vector2.zero;
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
