using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMakerLayoutB : UILayout
{
    public Dictionary<int, TreeNodeB> nodeDic = new Dictionary<int, TreeNodeB>();
    public TreeNodeB nodePrefab;
    public Transform nodeParent;

    private void Start()
    {
        AddNode(new NodeDataB(0));
        AddNode(new NodeDataB(1));
    }

    public void AddNode(NodeDataB node)
    {
        var nodeGo = Instantiate(nodePrefab, nodeParent);
        nodeGo.SetNodeId(node.GetId());
        nodeGo.gameObject.SetActive(true);
        nodeDic.Add(node.GetId(), nodeGo);
    }

    public void ConnectNodes(int nodeId, int nodeChoiceId, int targetNodeId)
    {
        if (nodeId == targetNodeId)
        {
            Debug.Log("Can't connect with same node");
            return;
        }

        if (nodeDic.ContainsKey(nodeId))
            nodeDic[nodeId].Connect(nodeChoiceId, targetNodeId);
        else
            Debug.Log($"ConnectNodes() nodeId {nodeId} doesn't exist");
    }

    public void DisconnectNodes(int nodeId, int nodeChoiceId)
    {
        if (nodeDic.ContainsKey(nodeId))
            nodeDic[nodeId].Disconnect(nodeChoiceId);
        else
            Debug.Log($"ConnectNodes() nodeId {nodeId} doesn't exist");
    }
}
