using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManagerB : MonoBehaviour
{
    int nodeIdCounter = 0;
    Dictionary<int, NodeDataB> nodeDic = new Dictionary<int, NodeDataB>();

    private void Start()
    {
        CreateNode();
    }

    public void CreateNode(Texture2D texture = null)
    {
        var nodeId = nodeIdCounter;
        while (nodeDic.ContainsKey(nodeId))
            nodeId = ++nodeIdCounter;

        var createdNode = new NodeDataB(nodeId);
        foreach (var node in nodeDic.Values)
            if (createdNode.GetPosition() == node.GetPosition())
                createdNode.SetPosition(createdNode.GetPosition() + new Vector2(80f, -80f));
        nodeDic.Add(nodeId, createdNode);

        ManagersB.ui.GetLayout<TreeMakerLayoutB>().AddNode(createdNode);
        ManagersB.ui.GetLayout<TreeMakerLayoutB>().SetNode(nodeId, texture);

        Debug.Log($"CreateNode() created nodeID : {nodeId}");
    }

    public void LoadGallaryImage(Action<Texture2D> onLoadFinish)
    {
        NativeGallery.GetImageFromGallery((image) =>
        {
            FileInfo selectedImage = new FileInfo(image);

            if (!string.IsNullOrEmpty(image))
                StartCoroutine(LoadImageCo(image, onLoadFinish));
        });
    }

    IEnumerator LoadImageCo(string imagePath, Action<Texture2D> onLoadFinish)
    {
        byte[] imageData = File.ReadAllBytes(imagePath);
        string imageName = Path.GetFileName(imagePath).Split('.')[0];
        string saveImagePath = Application.persistentDataPath + "/Image";

        File.WriteAllBytes(saveImagePath + imageName + ".jpg", imageData);

        var tempImage = File.ReadAllBytes(imagePath);

        Texture2D texture = new Texture2D(1080, 1920);
        if (texture.LoadImage(tempImage))
            onLoadFinish?.Invoke(texture);
        else
            Debug.LogError("LoadImageCo() Failed to load image");

        yield return null;
    }
}
