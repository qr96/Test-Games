using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUtil
{
    public static Vector2 CalculatePushVector(Vector2 myVector, Vector2 enemyVector)
    {
        var pushVector = Vector2.zero;
        pushVector = enemyVector - myVector;
        pushVector = pushVector.normalized;
        return pushVector;
    }
}
