using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElementGuageBar : MonoBehaviour
{
    public Image fillGuage;
    public TMP_Text guageText;

    public void SetGuage(float fillAmount, string guageStr = null)
    {
        fillGuage.fillAmount = fillAmount;
        guageText.gameObject.SetActive(!string.IsNullOrEmpty(guageStr));
        guageText.text = guageStr;
    }
}
