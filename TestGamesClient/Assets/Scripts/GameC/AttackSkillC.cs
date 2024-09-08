using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillC : MonoBehaviour
{
    public TriggerEvent2D trigger;

    int skillCode;
    GameObject caster;
    float damageDelay;
    float pushPower;
    float coolTime;

    HashSet<BaseEnemyC> targets = new HashSet<BaseEnemyC>();
    DateTime coolEnd;

    private void Awake()
    {
        StartCoroutine(AttackSkillCo());
        trigger.SetTriggerEvent(onEnter: AddTarget, onExit: RemoveTarget);
    }

    public void SetSkill(GameObject caster, int skillCode, float damageDelay, float pushPower, float coolTime)
    {
        this.caster = caster;
        this.skillCode = skillCode;
        this.damageDelay = damageDelay;
        this.pushPower = pushPower;
        this.coolTime = coolTime;
    }

    IEnumerator AttackSkillCo()
    {
        while (true)
        {
            if (targets.Count > 0 && DateTime.Now > coolEnd)
            {
                coolEnd = DateTime.Now.AddSeconds(coolTime);
                LocalPacketSender.SendUseSkil(skillCode, caster.transform.position);

                yield return new WaitForSeconds(damageDelay);

                foreach (var target in targets)
                {
                    var targetId = target.GetId();
                    LocalPacketSender.SendAttack(targetId);

                    target.OnDamage();
                    target.OnPush(CUtil.CalculatePushVector(caster.transform.position, target.transform.position) * pushPower);
                    target.SetTarget(transform);
                }
            }

            yield return null;
        }
    }

    void AddTarget(Collider2D collider)
    {
        var target = collider.gameObject.GetComponent<BaseEnemyC>();

        if (target != null)
            targets.Add(target);
    }

    void RemoveTarget(Collider2D collider)
    {
        targets.Remove(collider.GetComponent<BaseEnemyC>());
    }
}
