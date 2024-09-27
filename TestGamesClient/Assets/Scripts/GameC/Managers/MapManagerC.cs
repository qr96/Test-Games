using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerC : MonoBehaviour
{
    // TODO : Move to Resources folder.
    public List<GameObject> maps = new List<GameObject>();

    private void Start()
    {
        foreach (var map in maps)
            map.SetActive(false);

        MoveMap(0);
    }

    public void MoveMap(int mapId)
    {
        foreach (var map in maps)
            map.SetActive(false);

        maps[mapId].SetActive(true);
    }
}
