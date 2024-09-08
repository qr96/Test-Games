using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedC : MonoBehaviour
{
    public int Id;
    public int TypeId;

    Action<int> removeFunc;

    public void Set(int id, int typeId, Action<int> removeFunc)
    {
        Id = id;
        TypeId = typeId;
        this.removeFunc = removeFunc;
    }

    public int GetId()
    {
        return Id;
    }

    public int GetTypeId()
    {
        return TypeId;
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
