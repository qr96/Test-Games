using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeMakerConnectorRendererB : Graphic
{
    public float thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float w = rectTransform.rect.width;
        float h = rectTransform.rect.height;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(0, 0);
        vh.AddVert(vertex);

        vertex.position = new Vector3(thickness, 0);
        vh.AddVert(vertex);

        vertex.position = new Vector3(w, -h);
        vh.AddVert(vertex);

        vertex.position = new Vector3(w - thickness, -h);
        vh.AddVert(vertex);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
    }
}
