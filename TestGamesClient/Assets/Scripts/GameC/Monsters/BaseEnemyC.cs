using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyC : MonoBehaviour, IDamageableC
{
    public abstract int GetId();
    public abstract void OnSpawn();
    public abstract void OnDamage();
    public abstract void OnPush(Vector2 pushVector);
    public abstract void OnDead();
    public abstract void SetTarget(Transform target);
}
