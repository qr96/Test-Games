using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerC : MonoBehaviour
{
    public SpawnedC monsterPrefab;

    Dictionary<int, SpawnedC> idDic = new Dictionary<int, SpawnedC>();

    public SpawnedC SpawnMonster(int id)
    {
        if (!idDic.ContainsKey(id))
        {
            var prefab = Instantiate(monsterPrefab);
            prefab.SetId(id);
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

    public void RemoveMonster(int id)
    {
        if (idDic.ContainsKey(id))
            idDic[id].SetActive(false);
    }
}
