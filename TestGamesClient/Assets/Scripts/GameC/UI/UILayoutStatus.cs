using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILayoutStatus : UILayout
{
    public TMP_Text levelTmp;
    public UIElementGuageBar hpGuage;
    public UIElementGuageBar expGuage;

    public void SetExpGuage(long max, long now)
    {
        var percent = (float)now / max;
        expGuage.SetGuage(percent, $"{percent * 100:F1}%");
    }

    public void SetLevel(int level)
    {
        levelTmp.text = level.ToString();
    }

    public void SetHpGuage(long max, long now)
    {
        var percent = (float)(now / max);
        hpGuage.SetGuage(percent, $"{now}/{max}");
    }
}
