using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image image;

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetImage(string spritePath)
    {
        try
        {
            var sprite = Resources.Load<Sprite>(spritePath);
            image.sprite = sprite;
            image.enabled = sprite != null;
        }
        catch
        {
            image.enabled = false;
            Debug.LogError($"Failed to load image. {spritePath}");
        }
    }

    public void EmptySlot(bool isEmpty)
    {
        image.enabled = !isEmpty;
    }
}
