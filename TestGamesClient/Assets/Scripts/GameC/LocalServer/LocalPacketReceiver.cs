using ClientDefineC;
using LocalServerC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketReceiver
{
    public static void OnRespawnMonster(int id, int monsterCode, Vector2 position)
    {
        var assetkey = ResourceTable.GetMonsterPrefabPath(monsterCode);
        var monsterSpawn = ManagersC.obj.SpawnPrefab(id, assetkey);
        monsterSpawn.transform.position = position;
        monsterSpawn.gameObject.SetActive(true);

        var monster = monsterSpawn.GetComponent<BaseEnemyC>();
        monster.OnSpawn();
    }

    public static void OnMonsterAttacked(int id, List<long> damages)
    {
        var monsterObj = ManagersC.obj.GetObject(id);
        var monster = monsterObj.GetComponent<BaseEnemyC>();

        if (monster == null)
        {
            Debug.LogError($"Monster Not exists. {id}");
            return;
        }

        monster.OnDamage();
        ManagersC.ui.GetLayout<UILayoutFieldOverlay>().ShowDamage(damages, monster.transform.position);
    }

    public static void OnMonsterDead(int id)
    {
        var monsterObj = ManagersC.obj.GetObject(id);
        var monster = monsterObj.GetComponent<BaseEnemyC>();

        if (monster != null)
            monster.OnDead();
    }

    public static void OnPlayerDamaged(long damage)
    {

    }

    public static void OnUpdatePlayerInfo(PlayerInfo info, Stat userStat, Stat nowStat)
    {
        var uilayoutStatus = ManagersC.ui.GetLayout<UILayoutStatus>();
        uilayoutStatus.SetLevel(info.level);
        uilayoutStatus.SetMoney(info.money);
        uilayoutStatus.SetExpGuage(PlayerTable.GetNeedExp(info.level), info.exp);
        uilayoutStatus.SetHpGuage(userStat.hp, nowStat.hp);

        //ManagersC.ui.GetPopup<UIPopupEnhancement>().SetPopup(info.equipped[0]);
        ManagersC.ui.GetPopup<UIPopupInventory>().SetPopup(info.equipmentInventory, info.equipped);
    }

    public static void OnResultEnhance(int result, int id, Equipment equipment)
    {
        ManagersC.ui.GetPopup<UIPopupEnhancement>().SetPopup(equipment);
        if (result == 0)
            ManagersC.ui.GetLayout<UIToastMessageLayout>().ShowMessage("강화 실패");
        else if (result == 1)
            ManagersC.ui.GetLayout<UIToastMessageLayout>().ShowMessage("보유 머니가 부족합니다.");
        else if (result == 2)
            ManagersC.ui.GetLayout<UIToastMessageLayout>().ShowMessage("강화 성공");
        else if (result == 3)
            ManagersC.ui.GetLayout<UIToastMessageLayout>().ShowMessage("장비가 파괴되었습니다.");
    }

    public static void OnSpawnItem(int itemId, int itemCode, Vector2 position)
    {
        var assetKey = ResourceTable.GetDroppedItemPrefabPath(10001);
        var prefab = ManagersC.obj.SpawnPrefab(itemId, assetKey);
        prefab.transform.position = position;
        prefab.gameObject.SetActive(true);
        prefab.GetComponent<DroppedItem>().OnSpawnItem(itemId, itemCode);
    }

    public static void UseSkill(int id, int skillCode, Vector2 position)
    {
        var assetKey = ResourceTable.GetSkillEffectPrefabPath(skillCode);
        var prefab = ManagersC.obj.SpawnPrefab(id, assetKey);
        prefab.transform.position = position;
        prefab.SetActive(true);
    }
}
