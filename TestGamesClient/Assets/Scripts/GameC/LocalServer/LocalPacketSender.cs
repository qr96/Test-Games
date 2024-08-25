using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketSender
{
    static LocalServerC.LocalServer server = LocalServerC.LocalServer.Instance;

    public static void SendAttack(int id)
    {
        var position = ManagersC.obj.GetObject(id).transform.position;
        server.AttackMonster(id, position);
    }

    public static void SendAcquireItem(int id)
    {
        server.AcquireItem(id);
    }

    public static void SendEnhanceEquipment(int equipId)
    {
        server.EnhanceEquipment(equipId);
    }

    public static void SendOnPlayerDamaged(int id)
    {
        server.OnPlayerDamaged(id);
    }
}
