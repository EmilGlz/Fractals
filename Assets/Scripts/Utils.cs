using UnityEngine;

public static class Utils
{
    public static Vector3 GetMiddle(Vector3 pos1, Vector3 pos2)
    {
        return (pos1 + pos2) / 2f;
    }

    public static Vector3 GetMiddle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        return (pos1 + pos2 + pos3) / 3f;
    }
}
