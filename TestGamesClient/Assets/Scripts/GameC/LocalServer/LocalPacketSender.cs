using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketSender
{
    static LocalServerC.LocalServer server = LocalServerC.LocalServer.Instance;

    public static void SendAttack(int id)
    {
        server.AttackMonster(id);
    }

    public static void SendEnhanceEquipment(int equipId)
    {
        server.EnhanceEquipment(equipId);
    }
}
