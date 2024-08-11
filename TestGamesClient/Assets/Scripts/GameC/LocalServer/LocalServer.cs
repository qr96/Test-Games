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
        Dictionary<int, Stat> stats = new Dictionary<int, Stat>();

        Stack<int> monsterDeadList = new Stack<int>();

        Stat userStat;
        Stat nowStat;
        PlayerInfo playerInfo;
        Equipment weapon;

        DateTime respawnTime = DateTime.MaxValue;

        int idCounter = 0;

        private void Awake()
        {
            Instance = this;

            stats.Add(0, new Stat() { attack = 10, hp = 30 });
            userStat = new Stat() { attack = 10, hp = 100 };
            nowStat = userStat.DeepCopy();
            playerInfo = new PlayerInfo();
            weapon = new Equipment() { Id = 1, type = 1, level = 1 };
        }

        private void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var monster = new Monster(idCounter++);
                    monsters.Add(monster.Id, monster);
                    monster.Spawn(stats[0], new Vector2(4 + i, -0.5f * j));
                    LocalPacketReceiver.OnRespawnMonster(monster.Id, monster.position);
                }
            }

            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat);
        }

        private void Update()
        {
            if (DateTime.Now >= respawnTime)
            {
                while (monsterDeadList.Count > 0)
                {
                    var respawnId = monsterDeadList.Pop();
                    var monster = monsters[respawnId];
                    monster.Respawn(stats[0]);
                    LocalPacketReceiver.OnRespawnMonster(monster.Id, monster.position);
                }
                respawnTime = DateTime.MaxValue;
            }
        }

        public void AttackMonster(int id)
        {
            var coefficient = UnityEngine.Random.Range(0.3f, 1f);
            var attack = userStat.attack + DamageCalculator.GetEquipmentDamage(weapon);
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

        void AddExp(long exp)
        {
            playerInfo.exp += exp;
            while (playerInfo.exp >= playerInfo.level * 100)
            {
                playerInfo.exp -= playerInfo.level * 100;
                playerInfo.level++;
            }
            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo, userStat, nowStat);
        }
    }
}
