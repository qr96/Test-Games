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
        Dictionary<int, Equipment> equipments = new Dictionary<int, Equipment>();

        DateTime respawnTime = DateTime.MaxValue;

        int idCounter = 0;

        private void Awake()
        {
            Instance = this;

            userStat = new Stat() { attack = 10, hp = 100 };
            nowStat = userStat.DeepCopy();
            playerInfo = new PlayerInfo();
            equipments.Add(0, new Equipment() { type = 1, level = 0 });
        }

        private void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var typeId = 0;
                    var monster = new Monster(idCounter++, typeId);
                    monsters.Add(monster.Id, monster);
                    monster.Spawn(MonsterTable.GetStat(typeId), new Vector2(8 + i, -0.5f * j));
                    LocalPacketReceiver.OnRespawnMonster(monster.Id, monster.TypeId, monster.position);
                }
            }

            {
                var typeId = 1;
                var monster = new Monster(idCounter++, typeId);
                monsters.Add(monster.Id, monster);
                monster.Spawn(MonsterTable.GetStat(typeId), new Vector2(14, 0));
                LocalPacketReceiver.OnRespawnMonster(monster.Id, monster.TypeId, monster.position);
            }

            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat, equipments);
        }

        private void Update()
        {
            if (DateTime.Now >= respawnTime)
            {
                while (monsterDeadList.Count > 0)
                {
                    var respawnId = monsterDeadList.Pop();
                    var monster = monsters[respawnId];
                    monster.Respawn(MonsterTable.GetStat(monster.TypeId));
                    LocalPacketReceiver.OnRespawnMonster(monster.Id, monster.TypeId, monster.position);
                }
                respawnTime = DateTime.MaxValue;
            }
        }

        public void AttackMonster(int id)
        {
            var coefficient = UnityEngine.Random.Range(0.3f, 1f);
            var attack = userStat.attack + EquipmentTable.GetStat(equipments[0]).attack;
            var damage = (long)(attack * coefficient);

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
        }

        public void OnPlayerDamaged(int id)
        {
            var damage = monsters[id].nowStat.attack;
            nowStat.hp -= damage;
            if (nowStat.hp < 0) nowStat.hp = 0;

            LocalPacketReceiver.OnPlayerDamaged(damage);
            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat, equipments);
        }

        public void EnhanceEquipment(int id)
        {
            var success = EquipmentTable.GetSuccessPercenet(equipments[id].level);
            var destroyed = EquipmentTable.GetDestroyPercent(equipments[id].level);
            var price = EquipmentTable.GetEnhancePrice(equipments[id]);
            var rand = UnityEngine.Random.Range(0f, 1f);
            var result = 0;

            if (price > playerInfo.money)
                result = 1;
            else if (rand < success)
                result = 2;
            else if (rand < success + destroyed)
                result = 3;

            if (result == 2)
                equipments[id].level++;
            else if (result == 3)
                equipments[id].level = 0;

            LocalPacketReceiver.OnResultEnhance(result, id, equipments[id]);
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
            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat, equipments);
        }
    }
}
