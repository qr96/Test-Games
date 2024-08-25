using LocalServerC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupInventory : UIPopup
{
    public SlotView slotView;
    public Button dim;

    public override void OnCreate()
    {
        dim.onClick.AddListener(Hide);
    }

    public void SetPopup(List<Equipment> equipments)
    {
        slotView.SetInventory(equipments,
            (item, prefab) =>
            {
                prefab.SetActive(true);
                
                var button = prefab.GetComponent<KButton>();
                button.onClick.AddListener(() => ManagersC.ui.ShowPopup<UIPopupEnhancement>());
            });
    }
}
