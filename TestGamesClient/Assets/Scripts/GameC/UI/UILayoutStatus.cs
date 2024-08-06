using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayoutStatus : UILayout
{
    public UIElementGuageBar expGuage;

    public void SetExpGuage(long max, long now)
    {
        var percent = (float)now / max;
        expGuage.SetGuage(percent, $"{percent * 100:F1}%");
    }
}
