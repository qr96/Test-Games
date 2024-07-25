using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerB : MonoBehaviour
{
    int nodeIdCounter = 0;
    Dictionary<int, NodeDataB> nodeDic = new Dictionary<int, NodeDataB>();

    private void Start()
    {
        CreateNode();
        CreateNode();
    }

    public void CreateNode()
    {
        var nodeId = nodeIdCounter;
        while (nodeDic.ContainsKey(nodeId))
            nodeId = ++nodeIdCounter;

        var createdNode = new NodeDataB(nodeId);
        foreach (var node in nodeDic.Values)
            if (createdNode.GetPosition() == node.GetPosition())
                createdNode.SetPosition(createdNode.GetPosition() + new Vector2(80f, -80f));
        nodeDic.Add(nodeId, createdNode);

        ManagersB.ui.GetLayout<TreeMakerLayoutB>().AddNode(createdNode);

        Debug.Log($"CreateNode() created nodeID : {nodeId}");
    }
}
