using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

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

    SpawnedC spawned;
    Collider2D collider;
    Rigidbody2D rigid;
    RigidMoverC rigidMover;

    public Transform target;
    IDamageableC attackTarget;

    DateTime attackEndTime;
    DateTime deadEndTime;
    bool dead = false;

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
        collider = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        rigidMover = GetComponent<RigidMoverC>();

        detectTrigger.SetTriggerEvent((col) => SetTartget(col.transform));
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
    }

    public void Respawn()
    {
        sm.SetState(State.Respawn);
    }

    public int GetId()
    {
        return spawned.GetId();
    }

    public void OnDamage(long damage)
    {
        sm.SetState(State.Attacked);
    }

    public void OnPush(Vector2 pushVector)
    {
        rigid.AddForce(pushVector, ForceMode2D.Impulse);
    }

    public void OnDead()
    {
        dead = true;
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

    void StateRespawnEnter(State prevState)
    {
        collider.enabled = true;
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
    }

    void StateAttackUpdate()
    {
        if (DateTime.Now > attackEndTime)
        {
            attackEndTime = DateTime.MaxValue;
            sm.SetState(State.Idle);
        }
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
        collider.enabled = false;
        rigidMover.enabled = false;
        rigid.velocity = Vector3.zero;
        animator.SetBool("Moving", false);
        animator.SetBool("Dead", true);
        detectTrigger.gameObject.SetActive(false);
        attackTrigger.gameObject.SetActive(false);
        target = null;
        attackTarget = null;
        SetAttackedMaterial(false);
        deadEndTime = DateTime.Now.AddSeconds(1);
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

    void OnEndDeadState()
    {
        Debug.Log("OnEndDead");
        var id = GetComponent<SpawnedC>().GetId();
        animator.Rebind();
        gameObject.SetActive(false);
        ManagersC.obj.RemoveMonster(id);
    }
}
