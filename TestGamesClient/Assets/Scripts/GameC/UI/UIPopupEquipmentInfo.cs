using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupEquipmentInfo : UIPopup
{
    public Button dim;
    public KButton enhanceButton;

    public override void OnCreate()
    {
        dim.onClick.AddListener(() => Hide());
        enhanceButton.onClick.AddListener(() => ManagersC.ui.ShowPopup<UIPopupEnhancement>());
    }
}
