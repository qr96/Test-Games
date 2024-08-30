using ClientDefineC;
using LocalServerC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupInventory : UIPopup
{
    public List<ItemSlot> equippedSlots;
    public SlotView slotView;
    public Button dim;

    public override void OnCreate()
    {
        dim.onClick.AddListener(Hide);
    }

    public void SetPopup(List<Equipment> equipments, Dictionary<EquipType, Equipment> equipped)
    {
        slotView.SetInventory(equipments,
            (item, prefab) =>
            {
                prefab.SetActive(true);
                
                var button = prefab.GetComponent<KButton>();
                button.onClick.AddListener(() => ManagersC.ui.ShowPopup<UIPopupEnhancement>());

                var spritePath = ResourceTable.GetEquipmemtImage(item.TypeId);
                prefab.GetComponent<ItemSlot>().SetImage(spritePath);
            });

        foreach (var equip in equipped)
            SetEquippedImage(equip.Key, equip.Value);
    }

    void SetEquippedImage(EquipType type, Equipment equipment)
    {
        var spritePath = ResourceTable.GetEquipmemtImage(equipment.TypeId);

        int slotNum = type switch
        {
            EquipType.Hat => 3,
            EquipType.Cloth => 1,
            EquipType.Glove => 2,
            EquipType.Shoe => 5,
            EquipType.Ring => 4,
            EquipType.Sword => 0,
            _ => -1
        };

        if (slotNum > -1)
        {
            equippedSlots[slotNum].SetImage(spritePath);
        }
    }
}
