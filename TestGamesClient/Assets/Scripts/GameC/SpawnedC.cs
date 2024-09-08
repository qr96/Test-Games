using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedC : MonoBehaviour
{
    public int Id;
    public string AssetKey;

    Action<int> removeFunc;

    public void Set(int id, string assetKey, Action<int> removeFunc)
    {
        Id = id;
        AssetKey = assetKey;
        this.removeFunc = removeFunc;
    }

    public int GetId()
    {
        return Id;
    }

    public string GetTypeId()
    {
        return AssetKey;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Remove()
    {
        removeFunc?.Invoke(Id);
    }
}
