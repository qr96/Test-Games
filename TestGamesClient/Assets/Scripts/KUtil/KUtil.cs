using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static Vector3 ChangeX(this Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }
    public static Vector3 ChangeY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }
    public static Vector3 ChangeZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }
}

public static class KUtil
{
    public static string DecimalSeperator(long money)
    {
        if (money == 0)
            return "0";
        return money.ToString("#,##0");
    }

    public static string NumberSuffix(double number)
    {
        if (number > 999999999999999)
            return $"{number / 1000000000000:F1}T";
        else if (number > 999999999999)
            return $"{number / 1000000000:F1}B";
        else if (number > 999999999)
            return $"{number / 1000000:F1}M";
        else if (number > 999999)
            return $"{number / 1000:F1}K";

        return number.ToString("F0");
    }
}
