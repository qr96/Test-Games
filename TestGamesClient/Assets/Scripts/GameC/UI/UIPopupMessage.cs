using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPopupMessage : UIPopup
{
    public TMP_Text titleTmp;
    public TMP_Text messageTmp;
    public KButton yesBtn;
    public KButton noBtn;
    public KButton okBtn;

    Action onYes;

    public override void OnCreate()
    {
        yesBtn.onClick.AddListener(OnYes);
        noBtn.onClick.AddListener(Hide);
        okBtn.onClick.AddListener(OnYes);
    }

    public void SetPopup(string title, string message, Action onClickYes = null)
    {
        titleTmp.text = title;
        messageTmp.text = message;
        onYes = onClickYes;

        yesBtn.gameObject.SetActive(onClickYes != null);
        noBtn.gameObject.SetActive(onClickYes != null);
        okBtn.gameObject.SetActive(onClickYes == null);
    }

    void OnYes()
    {
        onYes?.Invoke();
        Hide();
    }
}
