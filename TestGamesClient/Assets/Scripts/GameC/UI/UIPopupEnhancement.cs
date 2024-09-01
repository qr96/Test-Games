using ClientDefineC;
using LocalServerC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupEnhancement : UIPopup
{
    public ItemSlot itemSlot;
    public TMP_Text info;
    public TMP_Text money;

    public KButton enhanceBtn;
    public Button touchBlock;

    int equipmentId;

    public override void OnCreate()
    {
        enhanceBtn.onClick.AddListener(Enhance);
        touchBlock.onClick.AddListener(Hide);
    }

    public void SetPopup(Equipment equipment)
    {
        var enhanceInfo = "";
        enhanceInfo += $"강화 단계 : {equipment.level} > {equipment.level + 1}\n";
        enhanceInfo += $"성공 확률 : {EquipmentTable.GetSuccessPercenet(equipment.level) * 100}%\n";
        enhanceInfo += $"파괴 확률 : {EquipmentTable.GetDestroyPercent(equipment.level) * 100}%\n";
        enhanceInfo += "\n";

        enhanceInfo += EquipmentTable.GetStat(equipment).attack > 0 ? $"공격력 : +{EquipmentTable.GetStat(equipment).attack}" : "";
        enhanceInfo += EquipmentTable.GetStat(equipment).hp > 0 ? $"체력 : +{EquipmentTable.GetStat(equipment).hp}" : "";

        var spritePath = ResourceTable.GetEquipmemtImage(equipment.TypeId);
        itemSlot.SetImage(spritePath);

        info.text = enhanceInfo;
        money.text = $"소모 골드 : {KUtil.DecimalSeperator(EquipmentTable.GetEnhancePrice(equipment))}";

        equipmentId = equipment.Id;
    }

    void Enhance()
    {
        LocalPacketSender.SendEnhanceEquipment(equipmentId);
    }
}
