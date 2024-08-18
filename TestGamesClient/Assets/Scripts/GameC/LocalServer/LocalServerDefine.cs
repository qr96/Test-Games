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
        public int TypeId;

        public Stat nowStat;
        public Vector2 position;

        public Monster(int id, int typeId)
        {
            Id = id;
            TypeId = typeId;
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
            if (nowStat.hp <= 0)
            {
                Debug.LogError("Can't attack dead monster");
                return;
            }

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
        public int type;
        public int level;
    }

    public static class EquipmentTable
    {
        public static Stat GetStat(Equipment equipment)
        {
            var stat = new Stat();
            stat.attack = equipment.level + 10;
            return stat;
        }

        public static Stat GetStatNextLevel(Equipment equipment)
        {
            var stat = new Stat();
            stat.attack = equipment.level + 1 + 10;
            return stat;
        }

        public static float GetSuccessPercenet(int level)
        {
            return 0.7f;
        }

        public static float GetDestroyPercent(int level)
        {
            return 0.05f;
        }

        public static long GetEnhancePrice(Equipment equipment)
        {
            return (equipment.level + 1) * 1000;
        }
    }

    public class PlayerTable
    {
        public static long GetNeedExp(int nowLevel)
        {
            var needExp = 0L;

            if (nowLevel < 10)
                needExp = (nowLevel + 1) * 10;
            else if (nowLevel % 5 == 0)
                needExp = GetNeedExp(nowLevel - 1) * 2;
            else
                needExp = (long)(GetNeedExp(nowLevel - 1) * 1.1D);

            return needExp;
        }
    }

    public class MonsterTable
    {
        public static Stat GetStat(int typeId)
        {
            if (typeId == 0)
                return new Stat() { attack = 5, hp = 30 };
            else if (typeId == 1)
                return new Stat() { attack = 10, hp = 500 };

            return new Stat();
        }
    }
}
