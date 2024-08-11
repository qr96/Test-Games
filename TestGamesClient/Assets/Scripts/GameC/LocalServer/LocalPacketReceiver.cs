using LocalServerC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketReceiver
{
    public static void OnRespawnMonster(int id, Vector2 position)
    {
        var monsterSpawn = ManagersC.obj.SpawnMonster(id);
        var monster = monsterSpawn.GetComponent<EnemyC>();
        monster.transform.position = position;
        monster.gameObject.SetActive(true);
        monster.Respawn();
    }

    public static void OnMonsterAttacked(int id, long damage)
    {
        var monsterObj = ManagersC.obj.GetObject(id);
        var monster = monsterObj.GetComponent<EnemyC>();

        if (monster != null)
        {
            monster.OnDamage(damage);
            ManagersC.ui.GetLayout<UILayoutFieldOverlay>().ShowDamage(damage, monster.transform.position);
        }
    }

    public static void OnMonsterDead(int id)
    {
        var monsterObj = ManagersC.obj.GetObject(id);
        var monster = monsterObj.GetComponent<EnemyC>();

        if (monster != null)
            monster.OnDead();
    }

    public static void OnUpdatePlayerInfo(PlayerInfo info, Stat userStat, Stat nowStat)
    {
        var uilayoutStatus = ManagersC.ui.GetLayout<UILayoutStatus>();
        uilayoutStatus.SetLevel(info.level);
        uilayoutStatus.SetMoney(info.money);
        uilayoutStatus.SetExpGuage(info.level * 100, info.exp);
        uilayoutStatus.SetHpGuage(userStat.hp, nowStat.hp);
    }
}
