using ClientDefineC;
using LocalServerC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupInventory : UIPopup
{
    public List<Image> equippedSlots;
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
            });

        foreach (var equip in equipped)
            SetEquippedImage(equip.Key, equip.Value);
    }

    void SetEquippedImage(EquipType type, Equipment equipment)
    {
        var spritePath = ResourceTable.GetEquipmemtImage(equipment.TypeId);
        var sprite = Resources.Load<Sprite>(spritePath);

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
            if (sprite != null)
                equippedSlots[slotNum].sprite = sprite;
            equippedSlots[slotNum].enabled = sprite != null;
        }
    }
}
