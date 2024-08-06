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

        Stat userStat;
        PlayerInfo playerInfo;

        int idCounter = 0;

        private void Awake()
        {
            Instance = this;

            stats.Add(0, new Stat() { attack = 10, hp = 30 });
            userStat = new Stat() { attack = 10, hp = 100 };
            playerInfo = new PlayerInfo();
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

            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo);
        }

        public void AttackMonster(int id)
        {
            monsters[id].OnDamage(userStat.attack);
        }

        public void AddExp(long exp)
        {
            playerInfo.exp += exp;
            while (playerInfo.exp >= playerInfo.level * 100)
            {
                playerInfo.exp -= playerInfo.level * 100;
                playerInfo.level++;
            }
            LocalPacketReceiver.OnUpdatePlayerInfo(playerInfo);
        }
    }
}
