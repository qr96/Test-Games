using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerC : MonoBehaviour
{
    // Common
    Dictionary<string, Stack<SpawnedC>> pool = new Dictionary<string, Stack<SpawnedC>>();
    Dictionary<string, SpawnedC> prefabCache = new Dictionary<string, SpawnedC>();

    // Server
    Dictionary<int, SpawnedC> idDic = new Dictionary<int, SpawnedC>();
    
    // Client
    int localId;
    Dictionary<int, SpawnedC> localIdDic = new Dictionary<int, SpawnedC>();

    public SpawnedC SpawnPrefab(int id, string assetKey)
    {
        SpawnedC spawned = GetPrefab(assetKey);

        if (spawned == null)
        {
            Debug.LogError($"Failed to Load prefab. assetKey : {assetKey}");
            return null;
        }

        spawned.Set(id, assetKey, (id) => RemovePrefab(id));
        idDic.Add(id, spawned);

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

    public SpawnedC SpawnPrefabLocal(string assetKey)
    {
        var id = localId++;
        SpawnedC spawned = GetPrefab(assetKey);

        if (spawned == null)
        {
            Debug.LogError($"Failed to Load prefab. assetKey : {assetKey}");
            return null;
        }

        spawned.Set(id, assetKey, (id) => RemovePrefabLocal(id));
        localIdDic.Add(id, spawned);
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

    SpawnedC GetPrefab(string assetKey)
    {
        SpawnedC spawned;

        if (!pool.ContainsKey(assetKey))
            pool.Add(assetKey, new Stack<SpawnedC>());

        if (pool[assetKey].Count > 0)
            spawned = pool[assetKey].Pop();
        else
        {
            if (!prefabCache.ContainsKey(assetKey))
            {
                var prefab = Resources.Load<SpawnedC>(assetKey);
                if (prefab == null)
                {
                    Debug.LogError($"Prefab not found. assetKey : {assetKey}");
                    return null;
                }

                prefabCache.Add(assetKey, prefab);
            }

            spawned = Instantiate(prefabCache[assetKey]);
        }

        return spawned;
    }
}
