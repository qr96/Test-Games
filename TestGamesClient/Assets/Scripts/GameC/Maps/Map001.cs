using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map001 : MonoBehaviour
{
    public List<TriggerEvent2D> triggers;

    private void Start()
    {
        triggers[0].SetTriggerEvent((col) =>
        {
            ManagersC.map.MoveMap(1);
        });
    }

}
