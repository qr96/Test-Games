using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILayoutStatus : UILayout
{
    public TMP_Text levelTmp;
    public TMP_Text moneyTmp;
    public UIElementGuageBar hpGuage;
    public UIElementGuageBar expGuage;
    public KButton equipmentBtn;

    private void Awake()
    {
        equipmentBtn.onClick.AddListener(() =>
        {
            ManagersC.ui.ShowPopup<UIPopupEnhancement>();
        });
    }

    public void SetExpGuage(long max, long now)
    {
        var percent = (float)now / max;
        expGuage.SetGuage(percent, $"{percent * 100:F1}%");
    }

    public void SetLevel(int level)
    {
        levelTmp.text = level.ToString();
    }

    public void SetMoney(long money)
    {
        moneyTmp.text = KUtil.DecimalSeperator(money);
    }

    public void SetHpGuage(long max, long now)
    {
        var percent = (float)now / max;
        hpGuage.SetGuage(percent, $"{now}/{max}");
    }
}
