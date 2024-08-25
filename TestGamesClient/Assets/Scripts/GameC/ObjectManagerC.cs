using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerC : MonoBehaviour
{
    public List<SpawnedC> prefabs = new List<SpawnedC>();

    Dictionary<int, SpawnedC> idDic = new Dictionary<int, SpawnedC>();
    Dictionary<int, Stack<SpawnedC>> pool = new Dictionary<int, Stack<SpawnedC>>();

    public SpawnedC SpawnPrefab(int id, int typeId)
    {
        if (!pool.ContainsKey(typeId))
            pool.Add(typeId, new Stack<SpawnedC>());

        if (pool[typeId].Count > 0)
        {
            var prefab = pool[typeId].Pop();
            prefab.SetId(id, typeId);
            idDic.Add(id, prefab);
        }
        else
        {
            var prefab = Instantiate(prefabs[typeId]);
            prefab.SetId(id, typeId);
            idDic.Add(id, prefab);
        }

        return idDic[id];
    }

    public SpawnedC GetObject(int id)
    {
        if (idDic.ContainsKey(id))
            return idDic[id];
        else
        {
            Debug.LogError($"Object is not exist. id : {id}");
            return default;
        }
    }

    public void RemovePrefab(int id)
    {
        if (idDic.ContainsKey(id))
        {
            var typeId = idDic[id].GetTypeId();
            pool[typeId].Push(idDic[id]);
            idDic[id].SetActive(false);
            idDic.Remove(id);
        }
    }
}
