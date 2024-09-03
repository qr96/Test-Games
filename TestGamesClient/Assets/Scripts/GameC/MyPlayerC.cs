using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerB : MonoBehaviour, IDamageableC
{
    public Rigidbody2D rigid;
    public Animator animator;
    public TriggerEvent2D attackTrigger;

    public float speed;
    public float attackDelay = 1f;
    public float pushPower = 3f;

    HashSet<BaseEnemyC> targets = new HashSet<BaseEnemyC>();

    Vector2 input;
    DateTime attackEnd;
    bool isOnAttack;

    private void Start()
    {
        attackTrigger.SetTriggerEvent(onEnter: OnAttackStart, onExit: OnAttackEnd);
    }

    private void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (isOnAttack)
            OnAttack();
        else
            animator.SetBool("Moving", input.sqrMagnitude > 0);

        rigid.velocity = new Vector2(input.x * speed * Time.fixedDeltaTime, input.y * speed * Time.fixedDeltaTime);

        if (input.x > 0)
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        else if (input.x < 0)
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);
    }

    public void OnDamage()
    {
        //throw new NotImplementedException();
    }

    void OnAttack()
    {
        if (DateTime.Now < attackEnd)
            return;
        StartCoroutine(AttackCoroutine(0.3f));
    }

    void OnAttackStart(Collider2D collider)
    {
        var target = collider.gameObject.GetComponent<BaseEnemyC>();

        if (target != null)
            targets.Add(target);
        if (targets.Count > 0)
            isOnAttack = true;
    }

    void OnAttackEnd(Collider2D collider)
    {
        targets.Remove(collider.GetComponent<BaseEnemyC>());
        if (targets.Count == 0)
            isOnAttack = false;
    }

    IEnumerator AttackCoroutine(float delay)
    {
        rigid.velocity = Vector3.zero;
        attackEnd = DateTime.Now.AddSeconds(attackDelay);
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack");
        speed = 150f;

        yield return new WaitForSeconds(delay);

        ShowAttackEffect();

        foreach (var target in targets)
        {
            var targetId = target.GetId();
            LocalPacketSender.SendAttack(targetId);

            target.OnDamage();
            target.OnPush(CalculatePushVector(transform.position, target.transform.position) * pushPower);
            target.SetTarget(transform);
        }
        speed = 200f;
    }

    void ShowAttackEffect()
    {
        var attackPefab = ManagersC.obj.SpawnPrefabLocal(3);
        var attackEffect = attackPefab.GetComponent<SpriteRenderer>();
        attackEffect.DOFade(1f, 0.01f)
            .OnComplete(() =>
            attackEffect.DOFade(0f, 0.29f)
            .OnComplete(() =>
            ManagersC.obj.RemovePrefabLocal(attackPefab.Id)));

        var attackEffectPivot = new Vector3(-0.89f, 0.61f, 0f);
        if (input.x > 0)
        {
            attackEffectPivot.x *= -1;
            attackPefab.transform.position = transform.position + attackEffectPivot;
            attackPefab.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (input.x < 0)
        {
            attackPefab.transform.position = transform.position + attackEffectPivot;
            attackPefab.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    Vector2 CalculatePushVector(Vector2 myVector, Vector2 enemyVector)
    {
        var pushVector = Vector2.zero;
        pushVector = enemyVector - myVector;
        pushVector = pushVector.normalized;
        return pushVector;
    }
}
