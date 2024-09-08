using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector2 maxMapSize;
    public Vector2 minMapSize;

    float halfHeight;
    float halfWidth;

    Vector3 newPos;

    private void Start()
    {
        var camera = Camera.main;
        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * camera.aspect;
    }

    private void LateUpdate()
    {
        newPos.x = Mathf.Clamp(target.position.x, minMapSize.x + halfWidth, maxMapSize.x - halfWidth);
        newPos.y = Mathf.Clamp(target.position.y, minMapSize.y + halfHeight, maxMapSize.y - halfHeight);
        newPos.z = transform.position.z;

        transform.position = newPos;
    }
}
