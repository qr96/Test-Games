using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeMakerCreateNodePopupB : UIPopup
{
    public RawImage imagerView;
    public KButton closeButton;
    public KButton createButton;

    public override void OnCreate()
    {
        closeButton.onClick.AddListener(Hide);
        createButton.onClick.AddListener(LoadGallaryImage);
    }

    void LoadGallaryImage()
    {

    }
}
