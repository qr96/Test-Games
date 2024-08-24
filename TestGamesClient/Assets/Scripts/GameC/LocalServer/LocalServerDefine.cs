using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalServerC
{
    public enum SpawnType
    {
        None = 0,
        Player = 1,
        Monster = 2,
        Npc = 3
    }

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
        //public long exp;
        public double exp;
        public long money;

        public Dictionary<int, Equipment> equipped = new Dictionary<int, Equipment>();
        public List<Equipment> equipmentInventory = new List<Equipment>();

        public PlayerInfo()
        {
            name = "None";
            level = 1;
            exp = 0;
            money = 0;
        }

        public void AddEquipment(Equipment equip)
        {
            equipmentInventory.Add(equip);
            for (int i = 0; i < equipmentInventory.Count; i++)
                equipmentInventory[i].Id = i;
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
        public int Id;
        public int TypeId;
        public int level;

        public Equipment(int typeId)
        {
            TypeId = typeId;
            level = 0;
        }
    }

    public class MapInfo
    {
        public List<SpawnedInfo> infoList = new List<SpawnedInfo>();
    }

    public class SpawnedInfo
    {
        public SpawnType spawnType;
        public int typeId;
        public Vector2 position;
        
        public SpawnedInfo(SpawnType spawnType, int typeId, Vector2 position)
        {
            this.spawnType = spawnType;
            this.typeId = typeId;
            this.position = position;
        }
    }

    public class EquipmentTable
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
        public static double GetNeedExp(int nowLevel)
        {
            var needExp = 0D;

            if (nowLevel < 10)
                needExp = (nowLevel + 1) * 10;
            else if (nowLevel % 10 == 0)
                needExp = GetNeedExp(nowLevel - 1) * 1.5D;
            else
                needExp = GetNeedExp(nowLevel - 1) * 1.1D;

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

    public class MapInfoTable
    {
        public static MapInfo GetMapInfo(int mapId)
        {
            var mapInfo = new MapInfo();

            // This infos will be converted to read from json
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    mapInfo.infoList.Add(new SpawnedInfo(SpawnType.Monster, 0, new Vector2(8 + i, -0.5f * j)));
            }
            mapInfo.infoList.Add(new SpawnedInfo(SpawnType.Monster, 1, new Vector2(14f, 0f)));

            return mapInfo;
        }
    }
}
