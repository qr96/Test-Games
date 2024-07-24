using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerB : MonoBehaviour
{
    Dictionary<int, NodeDataB> nodeDic = new Dictionary<int, NodeDataB>();

    private void Start()
    {
        nodeDic.Add(0, new NodeDataB(0));
        nodeDic.Add(1, new NodeDataB(1));
    }
}
