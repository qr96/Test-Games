using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyC : MonoBehaviour, IDamageableC, IPushableC
{
    Rigidbody2D rigid;
    RigidMoverC rigidMover;

    Transform target;

    DateTime endKnockBack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigidMover = GetComponent<RigidMoverC>();
    }

    private void Update()
    {
        if (DateTime.Now > endKnockBack)
        {
            rigidMover.enabled = true;
        }

        if (target != null)
        {
            rigidMover.Move(target.position);
        }
    }

    public void OnDamage(long damage)
    {

    }

    public void OnPush(Vector2 pushVector)
    {
        endKnockBack = DateTime.Now.AddSeconds(0.3f);
        rigidMover.enabled = false;
        rigid.AddForce(pushVector, ForceMode2D.Impulse);
    }

    public void SetTartget(Transform target)
    {
        this.target = target;
    }
}
