using ClientDefineC;
using LocalServerC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupEquipmentInfo : UIPopup
{
    public Button dim;
    public KButton enhanceButton;
    public KButton equipButton;
    public ItemSlot itemSlot;

    Equipment equipment;

    public override void OnCreate()
    {
        dim.onClick.AddListener(() => Hide());
        enhanceButton.onClick.AddListener(() => ManagersC.ui.ShowPopup<UIPopupEnhancement>().SetPopup(equipment));
        equipButton.onClick.AddListener(() => OnClickEquipButton());
    }

    public void SetPopup(Equipment equip)
    {
        equipment = equip;
        var spritePath = ResourceTable.GetEquipmemtImage(equip.TypeId);
        itemSlot.SetImage(spritePath);
    }

    void OnClickEquipButton()
    {
        LocalPacketSender.SendEquipItem(equipment.Id);
        Hide();
    }
}
