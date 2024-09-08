using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedC : MonoBehaviour
{
    public int Id;
    public int TypeId;

    public void SetId(int id, int typeId)
    {
        Id = id;
        TypeId = typeId;
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

    public void ReturnPool()
    {
        ManagersC.obj.RemovePrefab(Id);
    }
}
