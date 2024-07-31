using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyC : MonoBehaviour, IDamageableC, IPushableC
{
    public TriggerEvent2D detectTrigger;
    public TriggerEvent2D attackTrigger;

    public SPUM_SpriteList sprites;
    public Animator animator;

    public Material normalMat;
    public Material attackedMat;

    public float knockBackDuration;
    public float attackDelay;

    KStateMachine<State> sm = new KStateMachine<State>();

    Rigidbody2D rigid;
    RigidMoverC rigidMover;

    Transform target;
    IDamageableC attackTarget;

    DateTime attackEndTime;

    enum State
    {
        None = 0,
        Idle = 1,
        Move = 2,
        Attack = 3,
        Attacked = 4
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigidMover = GetComponent<RigidMoverC>();

        detectTrigger.SetTriggerEvent((col) => SetTartget(col.transform));
        attackTrigger.SetTriggerEvent(OnEnterAttackTrigger, OnExitAttackTrigger);

        sm.SetEvent(State.Idle, StateIdleEnter, StateIdleUpdate);
        sm.SetEvent(State.Move, StateMoveEnter, StateMoveUpdate);
        sm.SetEvent(State.Attack, StateAttackEnter, StateAttackUpdate);
        sm.SetEvent(State.Attacked, StateAttackedEnter);

        sm.SetState(State.Idle);
    }

    private void Update()
    {
        sm.Update();
    }

    public void OnDamage(long damage)
    {
        sm.SetState(State.Attacked);
    }

    public void OnPush(Vector2 pushVector)
    {
        rigid.AddForce(pushVector, ForceMode2D.Impulse);
    }

    public void SetTartget(Transform target)
    {
        this.target = target;
    }

    void OnEnterAttackTrigger(Collider2D collider)
    {
        attackTarget = collider.gameObject.GetComponent<IDamageableC>();
    }

    void OnExitAttackTrigger(Collider2D collider)
    {
        attackTarget = null;
    }

    void StateIdleEnter(State prevState)
    {
        rigidMover.enabled = false;
        animator.SetBool("Moving", false);
    }

    void StateIdleUpdate()
    {
        if (attackTarget != null)
            sm.SetState(State.Attack);
        else if (target != null)
            sm.SetState(State.Move);
    }

    void StateMoveEnter(State prevState)
    {
        rigidMover.enabled = true;
        animator.SetBool("Moving", true);
    }

    void StateMoveUpdate()
    {
        if (attackTarget != null)
            sm.SetState(State.Attack);
        else if (target != null)
        {
            rigidMover.Move(target.position);
            if (target.position.x > transform.position.x)
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.ChangeY(180f));
            else
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.ChangeY(0f));
        }
        else
            sm.SetState(State.Idle);
    }

    void StateAttackEnter(State prevState)
    {
        rigidMover.enabled = false;
        animator.SetTrigger("Attack");
        attackEndTime = DateTime.Now.AddSeconds(attackDelay);
    }

    void StateAttackUpdate()
    {
        if (DateTime.Now > attackEndTime)
            sm.SetState(State.Idle);
    }

    void StateAttackedEnter(State prevState)
    {
        rigidMover.enabled = false;
        animator.SetBool("Moving", false);

        SetAttackedMaterial(true);

        transform.DOLocalRotate(transform.localEulerAngles.ChangeZ(-15f), knockBackDuration / 4f)
            .OnComplete(() => transform.DOLocalRotate(transform.localEulerAngles.ChangeZ(10f), knockBackDuration / 2f)
            .OnComplete(() =>
            {
                transform.DOLocalRotate(transform.localEulerAngles.ChangeZ(0f), knockBackDuration / 4f);
                rigidMover.enabled = true;

                SetAttackedMaterial(false);

                if (attackTarget != null)
                    sm.SetState(State.Attack);
                else if (target != null)
                    sm.SetState(State.Move);
                else
                    sm.SetState(State.Idle);
            }));
    }

    void SetAttackedMaterial(bool attacked)
    {
        if (attacked)
        {
            foreach (var sr in sprites._hairList)
                sr.material = attackedMat;
            foreach (var sr in sprites._bodyList)
                sr.material = attackedMat;
            foreach (var sr in sprites._armorList)
                sr.material = attackedMat;
            foreach (var sr in sprites._clothList)
                sr.material = attackedMat;
            foreach (var sr in sprites._pantList)
                sr.material = attackedMat;
        }
        else
        {
            foreach (var sr in sprites._hairList)
                sr.material = normalMat;
            foreach (var sr in sprites._bodyList)
                sr.material = normalMat;
            foreach (var sr in sprites._armorList)
                sr.material = normalMat;
            foreach (var sr in sprites._clothList)
                sr.material = normalMat;
            foreach (var sr in sprites._pantList)
                sr.material = normalMat;
        }
    }
}
