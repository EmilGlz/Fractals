using UnityEngine;

public static class Extensions
{
    public static Vector2 GetPosition(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y); 
    }

    public static float GetPosX(this RectTransform rect)
    {
        return rect.anchoredPosition.x;
    }

    public static void SetPosX(this RectTransform rect, float value)
    {
        rect.anchoredPosition = new Vector2(value, rect.anchoredPosition.y);
    }

    public static float GetPosY(this RectTransform rect)
    {
        return rect.anchoredPosition.y;
    }

    public static void SetPosY(this RectTransform rect, float value)
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, value);
    }

    public static float GetWidth(this RectTransform rect)
    {
        return rect.sizeDelta.x;
    }

    public static void SetWidth(this RectTransform rect, float value)
    {
        rect.sizeDelta = new Vector2(value, rect.sizeDelta.y);
    }

    public static float GetHeight(this RectTransform rect)
    {
        return rect.sizeDelta.y;
    }

    public static void SetHeight(this RectTransform rect, float value)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, value);
    }

    public static float GetLeft(this RectTransform rect)
    {
        return rect.offsetMin.x;
    }

    public static void SetLeft(this RectTransform rect, float value)
    {
        rect.offsetMin = new Vector2(value, rect.offsetMin.y);
    }

    public static float GetBottom(this RectTransform rect)
    {
        return rect.offsetMin.y;
    }

    public static void SetBottom(this RectTransform rect, float value)
    {
        rect.offsetMin = new Vector2(rect.offsetMin.x, value);
    }

    public static float GetRight(this RectTransform rect)
    {
        return -rect.offsetMax.x;
    }

    public static void SetRight(this RectTransform rect, float value)
    {
        rect.offsetMax = new Vector2(-value, rect.offsetMax.y);
    }

    public static float GetTop(this RectTransform rect)
    {
        return -rect.offsetMax.y;
    }

    public static void SetTop(this RectTransform rect, float value)
    {
        rect.offsetMax = new Vector2(rect.offsetMax.x, -value);
    }

    public static Vector3 GetScale(this Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = matrix.GetColumn(0).magnitude;
        scale.y = matrix.GetColumn(1).magnitude;
        scale.z = matrix.GetColumn(2).magnitude;
        return scale;
    }
}
