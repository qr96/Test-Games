using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyC : MonoBehaviour, IDamageableC, IPushableC
{
    public TriggerEvent2D detectCollider;
    public SPUM_SpriteList sprites;

    public Material normalMat;
    public Material attackedMat;

    public float knockBackDuration;

    Rigidbody2D rigid;
    RigidMoverC rigidMover;

    Transform target;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigidMover = GetComponent<RigidMoverC>();

        detectCollider.SetTriggerEvent((col) => SetTartget(col.transform));
    }

    private void Update()
    {
        if (target != null)
        {
            rigidMover.Move(target.position);
        }
    }

    public void OnDamage(long damage)
    {
        transform.DOLocalRotate(new Vector3(0f, 0f, -15f), knockBackDuration / 4f)
            .OnComplete(() => transform.DOLocalRotate(new Vector3(0f, 0f, 10f), knockBackDuration / 2f)
            .OnComplete(() =>
                {
                    transform.DOLocalRotate(Vector3.zero, knockBackDuration / 4f);
                    rigidMover.enabled = true;

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
                }));

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

    public void OnPush(Vector2 pushVector)
    {
        rigidMover.enabled = false;
        rigid.AddForce(pushVector, ForceMode2D.Impulse);
    }

    public void SetTartget(Transform target)
    {
        this.target = target;
    }
}
