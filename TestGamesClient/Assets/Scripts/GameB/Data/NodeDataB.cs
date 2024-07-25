using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataB
{
    int id;
    List<int> pointers;
    Vector2 position;

    public NodeDataB(int id)
    {
        this.id = id;
        pointers = new List<int>() { -1, -1 };
        position = new Vector2(432, -511);
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

    public void SetPosition(Vector2 position)
    {
        this.position = position;
    }

    public Vector2 GetPosition()
    {
        return position;
    }
}
