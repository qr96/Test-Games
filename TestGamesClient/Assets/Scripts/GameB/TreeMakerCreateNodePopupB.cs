using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class TreeMakerCreateNodePopupB : UIPopup
{
    public RawImage imagerView;
    public KButton closeButton;
    public KButton createButton;
    public KButton saveButton;

    Texture2D textureTmp;

    public override void OnCreate()
    {
        closeButton.onClick.AddListener(Hide);
        createButton.onClick.AddListener(LoadGallaryImage);
        saveButton.onClick.AddListener(SaveNode);
    }

    void LoadGallaryImage()
    {
        ManagersB.data.LoadGallaryImage(OnLoadImage);
    }

    void OnLoadImage(Texture2D texture)
    {
        if (texture != null)
        {
            imagerView.texture = texture;
            textureTmp = texture;
        }
    }

    void SaveNode()
    {
        ManagersB.data.CreateNode(textureTmp);
        imagerView.texture = null;
        Hide();
    }
}
