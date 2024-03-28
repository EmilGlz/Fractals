using UnityEngine;

public static class Extensions
{
    public static Vector2 GetPosition(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y); 
    }
}
