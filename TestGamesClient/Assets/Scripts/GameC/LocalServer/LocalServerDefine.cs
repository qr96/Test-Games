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

    public class PlayerInfo
    {
        public string name;
        public int level;
        public long exp;
        public long money;

        public PlayerInfo()
        {
            name = "None";
            level = 1;
            exp = 0;
            money = 0;
        }
    }

    public class Monster : IDamageable
    {
        public int Id;

        public Stat nowStat;
        public Vector2 position;

        public Monster(int id)
        {
            Id = id;
            nowStat = new Stat();
        }

        public void Spawn(Stat stat, Vector2 position)
        {
            nowStat.Set(stat);
            this.position = position;
        }

        public void Respawn(Stat stat)
        {
            nowStat.Set(stat);
        }

        public void OnDamage(long damage)
        {
            nowStat.hp -= damage;
            LocalPacketReceiver.OnMonsterAttacked(Id, damage);

            if (nowStat.hp <= 0)
                OnDead();
        }

        protected void OnDead()
        {
            LocalServer.Instance.OnDeadMonster(Id);
        }
    }

    public class Equipment
    {
        public int Id;
        public int type;
        public int level;
    }

    public static class DamageCalculator
    {
        public static long GetEquipmentDamage(Equipment equipment)
        {
            return 10 + equipment.level;
        }
    }
}
