using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalServerC
{
    public class LocalServer : MonoBehaviour
    {
        public static LocalServer Instance;

        Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();
        Stack<int> monsterDeadList = new Stack<int>();

        Stat userStat;
        Stat nowStat;
        PlayerInfo playerInfo;

        DateTime respawnTime = DateTime.MaxValue;

        int idCounter = 0;

        private void Awake()
        {
            Instance = this;

            userStat = new Stat() { attack = 10, hp = 100 };
            nowStat = userStat.DeepCopy();
            playerInfo = new PlayerInfo() { money = 100000000, level = 50 };

            playerInfo.equipped.Add(EquipType.Hat, new Equipment(0));
            playerInfo.equipped.Add(EquipType.Cloth, new Equipment(0));
            playerInfo.equipped.Add(EquipType.Glove, new Equipment(0));
            playerInfo.equipped.Add(EquipType.Shoe, new Equipment(0));
            playerInfo.equipped.Add(EquipType.Ring, new Equipment(0));
            playerInfo.equipped.Add(EquipType.Sword, new Equipment(60001));
        }

        private void Start()
        {
            var mapInfo = MapInfoTable.GetMapInfo(1);
            foreach (var item in mapInfo.infoList)
            {
                if (item.spawnType == SpawnType.Monster)
                {
                    var monster = new Monster(idCounter++, item.typeId);
                    monsters.Add(monster.Id, monster);
                    monster.Spawn(MonsterTable.GetStat(item.typeId), item.position);
                    LocalPacketReceiver.OnRespawnMonster(monster.Id, monster.TypeId, monster.position);
                }
            }

            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat);
        }

        private void Update()
        {
            //if (DateTime.Now >= respawnTime)
            //{
            //    while (monsterDeadList.Count > 0)
            //    {
            //        var respawnId = monsterDeadList.Pop();
            //        var monster = monsters[respawnId];
            //        monster.Respawn(MonsterTable.GetStat(monster.TypeId));
            //        LocalPacketReceiver.OnRespawnMonster(monster.Id, monster.TypeId, monster.position);
            //    }
            //    respawnTime = DateTime.MaxValue;
            //}
        }

        public void AttackMonster(int id, Vector2 position)
        {
            var coefficient = UnityEngine.Random.Range(0.3f, 1f);
            var attack = userStat.attack + EquipmentTable.GetStat(playerInfo.equipped[0]).attack;
            var damage = (long)(attack * coefficient);

            monsters[id].position = position;
            monsters[id].OnDamage(damage);
        }

        public void OnDeadMonster(int id)
        {
            if (monsterDeadList.Count <= 0)
                respawnTime = DateTime.Now.AddSeconds(10);
            monsterDeadList.Push(id);

            playerInfo.money += 100;
            AddExp(10);

            LocalPacketReceiver.OnMonsterDead(id);
            LocalPacketReceiver.OnSpawnItem(idCounter++, 0, monsters[id].position);
        }

        public void OnPlayerDamaged(int id)
        {
            var damage = monsters[id].nowStat.attack;
            nowStat.hp -= damage;
            if (nowStat.hp < 0) nowStat.hp = 0;

            LocalPacketReceiver.OnPlayerDamaged(damage);
            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat);
        }

        public void EnhanceEquipment(int id)
        {
            var equipment = playerInfo.equipped[0];
            var success = EquipmentTable.GetSuccessPercenet(equipment.level);
            var destroyed = EquipmentTable.GetDestroyPercent(equipment.level);
            var price = EquipmentTable.GetEnhancePrice(equipment);
            var rand = UnityEngine.Random.Range(0f, 1f);
            var result = 0;

            if (price > playerInfo.money)
                result = 1;
            else if (rand < success)
                result = 2;
            else if (rand < success + destroyed)
                result = 3;

            if (result == 2)
                equipment.level++;
            else if (result == 3)
                equipment.level = 0;

            LocalPacketReceiver.OnResultEnhance(result, id, equipment);
        }

        public void AcquireItem(int id)
        {
            var valid = true;
            if (valid)
                playerInfo.ObtainItem(new Equipment(1));
            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat);
        }

        public void EquipItem(int index)
        {
            playerInfo.EquipItem(playerInfo.equipmentInventory[index]);
        }

        void AddExp(long exp)
        {
            playerInfo.exp += exp;
            var needExp = PlayerTable.GetNeedExp(playerInfo.level);
            while (playerInfo.exp >= needExp)
            {
                playerInfo.exp -= needExp;
                playerInfo.level++;
                needExp = PlayerTable.GetNeedExp(playerInfo.level);
            }
            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat);
        }
    }
}
