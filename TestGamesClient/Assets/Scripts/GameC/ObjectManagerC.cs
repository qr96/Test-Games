using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerC : MonoBehaviour
{
    // TODO : from resources folder and db.
    public List<SpawnedC> prefabs = new List<SpawnedC>();

    // server
    Dictionary<int, SpawnedC> idDic = new Dictionary<int, SpawnedC>();
    Dictionary<int, Stack<SpawnedC>> pool = new Dictionary<int, Stack<SpawnedC>>();

    // client
    int localId;
    Dictionary<int, SpawnedC> localIdDic = new Dictionary<int, SpawnedC>();

    private void Awake()
    {
        for (int i = 0; i < prefabs.Count; i++)
            pool.Add(i, new Stack<SpawnedC>());
    }

    public SpawnedC SpawnPrefab(int id, int typeId)
    {
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

    public SpawnedC SpawnPrefabLocal(int typeId)
    {
        var id = localId++;

        if (pool[typeId].Count > 0)
        {
            var prefab = pool[typeId].Pop();
            prefab.SetId(id, typeId);
            localIdDic.Add(id, prefab);
        }
        else
        {
            var prefab = Instantiate(prefabs[typeId]);
            prefab.SetId(id, typeId);
            localIdDic.Add(id, prefab);
        }

        localIdDic[id].SetActive(true);
        return localIdDic[id];
    }

    public SpawnedC GetObjectLocal(int id)
    {
        if (localIdDic.ContainsKey(id))
            return localIdDic[id];
        else
        {
            Debug.LogError($"Object is not exist. id : {id}");
            return default;
        }
    }

    public void RemovePrefabLocal(int id)
    {
        if (localIdDic.ContainsKey(id))
        {
            var typeId = localIdDic[id].GetTypeId();
            pool[typeId].Push(localIdDic[id]);
            localIdDic[id].SetActive(false);
            localIdDic.Remove(id);
        }
    }
}
