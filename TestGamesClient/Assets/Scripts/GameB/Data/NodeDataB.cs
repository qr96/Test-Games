using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataB
{
    int id;
    List<int> pointers;
    
    public NodeDataB(int id)
    {
        this.id = id;
        pointers = new List<int>() { -1, -1 };
    }

    public int GetId()
    {
        return id;
    }

    public void SetPointer(int index, int targetId)
    {
        if (pointers.Count > index)
            pointers[index] = targetId;
    }
}
