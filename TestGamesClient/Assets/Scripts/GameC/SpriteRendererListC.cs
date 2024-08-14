using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererListC : MonoBehaviour
{
    public List<SpriteRenderer> spriteRenders;

    public void SetMaterial(Material mat)
    {
        foreach (var renderer in spriteRenders)
            renderer.material = mat;
    }
}
