using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMoveAIC : MonoBehaviour
{
    public float speed;

    public List<GameObject> desiredList = new List<GameObject>();
    public List<GameObject> unDesiredList = new List<GameObject>();

    // cos0 = 0, cos90 = 4, cos180 = 8, cos 270 = 12
    public int[] weights = new int[16];

    private void Update()
    {
        for (int i = 0; i < weights.Length; i++)
            weights[i] = 0;
        foreach (var desired in desiredList)
            SetDesiredWeights(desired.transform.position);
        foreach (var undesired in unDesiredList)
            SetUndesiredWeights(undesired.transform.position);

        if (!IsEmptyDesiredAndUndesired())
        {
            var direc = IndexToVector(FindMaxWeightIndex());
            direc = direc * speed * Time.deltaTime;
            transform.position += (Vector3)direc;
        }
    }

    void SetDesiredWeights(Vector2 desiredPos)
    {
        var index = VectorToIndex(desiredPos - (Vector2)transform.position);
        weights[index] += 100;
        for (int i = 1; i < 5; i++)
        {
            weights[(index + i) % 16] += 100 - i * 10;
            weights[(index + 16 - i) % 16] += 100 - i * 10;
        }
        for (int i = 5; i < 8; i++)
        {
            weights[(index + i) % 16] += 60 - (i - 4) * 20;
            weights[(index + 16 - i) % 16] += 60 - (i - 4) * 20;
        }
        weights[(index + 8) % 16] -= 10;
    }

    void SetUndesiredWeights(Vector2 undesiredPos)
    {
        var index = VectorToIndex(undesiredPos - (Vector2)transform.position);
        weights[index] -= 100;
        for (int i = 1; i < 5; i++)
        {
            weights[(index + i) % 16] -= 100 - i * 5;
            weights[(index + 16 - i) % 16] -= 100 - i * 5;
        }
        for (int i = 5; i < 8; i++)
        {
            weights[(index + i) % 16] -= 60 - (i - 4) * 10;
            weights[(index + 16 - i) % 16] -= 60 - (i - 4) * 10;
        }
        weights[(index + 8) % 16] += 10;
    }

    bool IsEmptyDesiredAndUndesired()
    {
        return desiredList.Count <= 0 && unDesiredList.Count <= 0;
    }

    int FindMaxWeightIndex()
    {
        var maxW = weights[0];
        var maxI = 0;

        for (int i = 1; i < weights.Length; i++)
        {
            if (weights[i] > maxW)
            {
                maxW = weights[i];
                maxI = i;
            }
        }

        return maxI;
    }

    int VectorToIndex(Vector2 vec)
    {
        var dot = Vector2.Dot(vec.normalized, Vector2.right);
        var index = Mathf.RoundToInt((1f - dot) * 4f);
        if (vec.y > 0) index = 16 - index;
        index = index % 16;
        return index;
    }

    Vector2 IndexToVector(int index)
    {
        var angle = -index * 20f;
        return AngleToVector(angle);
    }

    Vector2 AngleToVector(float angle)
    {
        var radian = angle * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
