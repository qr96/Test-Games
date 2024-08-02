using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalServerC
{
    public interface IDamageable
    {
        public void OnDamage(long damage);
    }

    public class Stat
    {
        public long hp;
        public long attack;

        public Stat DeepCopy()
        {
            var stat = new Stat();
            stat.hp = hp;
            stat.attack = attack;
            return stat;
        }

        public void Set(Stat stat)
        {
            hp = stat.hp;
            attack = stat.attack;
        }
    }

    public class Monster : IDamageable
    {
        Stat nowStat;

        Action onSpawn;
        Action onDamage;
        Action onDead;

        public void SetEvents(Action onSpawn, Action onDamage, Action onDead)
        {
            this.onSpawn = onSpawn;
            this.onDamage = onDamage;
            this.onDead = onDead;
        }

        public void Spawn(Stat stat)
        {
            nowStat.Set(stat);
            onSpawn?.Invoke();
        }

        public void OnDamage(long damage)
        {
            nowStat.hp -= damage;
            onDamage?.Invoke();

            if (nowStat.hp <= 0)
                OnDead();
        }

        protected void OnDead()
        {
            onDead?.Invoke();
        }
    }
}
