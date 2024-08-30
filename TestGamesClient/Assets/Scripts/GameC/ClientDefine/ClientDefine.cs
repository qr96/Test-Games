using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace ClientDefineC
{
    public class ResourceTable
    {
        // This must be converted to table.
        public static string GetEquipmemtImage(int equipmentId)
        {
            var path = "SPUM/SPUM_Sprites2/Items/";
            if (equipmentId == 10001)
                path = path + "4_Helmet/Helmet_1";
            else if (equipmentId == 20001)
                path = path + "2_Cloth/Cloth_1";
            else if (equipmentId == 30001)
                path = "";
            else if (equipmentId == 40001)
                path = path + "3_Pant/Foot_1";
            else if (equipmentId == 50001)
                path = "";
            else if (equipmentId == 60001)
                path = path + "6_Weapons/Sword_1";
            return path;
        }
    }
}
