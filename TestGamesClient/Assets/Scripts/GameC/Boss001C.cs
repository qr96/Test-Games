using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss001C : BaseEnemyC
{
    public TriggerEvent2D detectTrigger;
    public TriggerEvent2D attackTrigger;

    public SpriteRendererListC renderers;
    public Animator animator;

    public Material normalMat;
    public Material attackedMat;

    public float knockBackDuration;
    public float attackDelay;
    public float damageDelay;

    KStateMachine<State> sm = new KStateMachine<State>();

    SpawnedC spawned;
    Collider2D col;
    Rigidbody2D rigid;
    RigidMoverC rigidMover;

    public Transform target;
    IDamageableC attackTarget;

    DateTime damageDelayEnd;
    DateTime attackEndTime;
    DateTime deadEndTime;
    bool dead = false;

    public int state;

    enum State
    {
        None = 0,
        Respawn = 1,
        Idle = 2,
        Move = 3,
        Attack = 4,
        Attacked = 5,
        Dead = 6
    }

    private void Awake()
    {
        spawned = GetComponent<SpawnedC>();
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        rigidMover = GetComponent<RigidMoverC>();

        detectTrigger.SetTriggerEvent((col) => SetTarget(col.transform));
        attackTrigger.SetTriggerEvent(OnEnterAttackTrigger, OnExitAttackTrigger);

        sm.SetEvent(State.Respawn, StateRespawnEnter);
        sm.SetEvent(State.Idle, StateIdleEnter, StateIdleUpdate);
        sm.SetEvent(State.Move, StateMoveEnter, StateMoveUpdate);
        sm.SetEvent(State.Attack, StateAttackEnter, StateAttackUpdate);
        sm.SetEvent(State.Attacked, StateAttackedEnter);
        sm.SetEvent(State.Dead, StateDeadEnter, StateDeadUpdate);
    }

    private void Update()
    {
        sm.Update();
        state = (int)sm.GetState();
    }

    public override void OnSpawn()
    {
        sm.SetState(State.Respawn);
    }

    public override int GetId()
    {
        return spawned.GetId();
    }

    public override void OnDamage()
    {
        //sm.SetState(State.Attacked);
        SetAttackedMaterial(true);
        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.2f);
            SetAttackedMaterial(false);
        }
    }

    public override void OnPush(Vector2 pushVector)
    {

    }

    public override void OnDead()
    {
        dead = true;
    }

    public override void SetTarget(Transform target)
    {
        this.target = target;
    }

    void OnEnterAttackTrigger(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            attackTarget = collider.gameObject.GetComponent<IDamageableC>();
    }

    void OnExitAttackTrigger(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            attackTarget = null;
    }

    void StateRespawnEnter(State prevState)
    {
        col.enabled = true;
        rigidMover.enabled = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Dead", false);
        detectTrigger.gameObject.SetActive(true);
        attackTrigger.gameObject.SetActive(true);
        target = null;
        attackTarget = null;
        sm.SetState(State.Idle);
        deadEndTime = DateTime.MaxValue;
        attackEndTime = DateTime.MaxValue;
        dead = false;
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
        else if (dead)
            sm.SetState(State.Dead);
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
        damageDelayEnd = DateTime.Now.AddSeconds(damageDelay);
    }

    void StateAttackUpdate()
    {
        if (dead)
        {
            sm.SetState(State.Dead);
        }
        else if (DateTime.Now > damageDelayEnd)
        {
            damageDelayEnd = DateTime.MaxValue;
            if (target != null)
                LocalPacketSender.SendOnPlayerDamaged(GetId());
        }
        else if (DateTime.Now > attackEndTime)
        {
            attackEndTime = DateTime.MaxValue;
            sm.SetState(State.Idle);
        }
    }

    void StateAttackedEnter(State prevState)
    {
        SetAttackedMaterial(true);

        transform.DOLocalRotate(transform.localEulerAngles.ChangeZ(-15f), knockBackDuration / 4f)
            .OnComplete(() => transform.DOLocalRotate(transform.localEulerAngles.ChangeZ(10f), knockBackDuration / 2f)
            .OnComplete(() =>
            {
                transform.DOLocalRotate(transform.localEulerAngles.ChangeZ(0f), knockBackDuration / 4f);

                SetAttackedMaterial(false);

                if (dead)
                    sm.SetState(State.Dead);
                if (attackTarget != null)
                    sm.SetState(State.Attack);
                else if (target != null)
                    sm.SetState(State.Move);
                else
                    sm.SetState(State.Idle);
            }));
    }

    void StateDeadEnter(State prevState)
    {
        col.enabled = false;
        rigidMover.enabled = false;
        rigid.velocity = Vector3.zero;
        animator.SetBool("Moving", false);
        animator.SetBool("Dead", true);
        detectTrigger.gameObject.SetActive(false);
        attackTrigger.gameObject.SetActive(false);
        target = null;
        attackTarget = null;
        SetAttackedMaterial(false);
        deadEndTime = DateTime.Now.AddSeconds(0.3);
        attackEndTime = DateTime.MaxValue;
    }

    void StateDeadUpdate()
    {
        if (DateTime.Now > deadEndTime)
        {
            deadEndTime = DateTime.MaxValue;
            OnEndDeadState();
        }
    }

    void SetAttackedMaterial(bool attacked)
    {
        if (attacked)
            renderers.SetMaterial(attackedMat);
        else
            renderers.SetMaterial(normalMat);
    }

    void OnEndDeadState()
    {
        animator.Rebind();
        gameObject.SetActive(false);
        ManagersC.obj.RemoveMonster(GetId());
    }
}
