using Unity.Collections;
using Unity.Mathematics;
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

    public static void ModifyScale(ref this Matrix4x4 matrix, Vector3 newScale)
    {
        // Set the scale components of the matrix to the new scale values
        matrix.m00 = newScale.x; // Set x component of the X axis
        matrix.m11 = newScale.y; // Set y component of the Y axis
        matrix.m22 = newScale.z; // Set z component of the Z axis
    }

    public static int GetMatricesCountFrom1000(this NativeArray<Matrix4x4> matrices)
    {
        for (int i = 0; i < 1000; i++)
        {
            if (matrices[i].GetScale() == Vector3.zero)
                return i;
        }
        return -1;
    }

    public static void Insert<T>(this ref NativeArray<T> matrices, T matrice) where T : struct
    {
        NativeArray<T> newArray = new(matrices.Length + 1, Allocator.TempJob);
        for (int i = 0; i < matrices.Length; i++)
            newArray[i] = matrices[i];
        newArray[^1] = matrice;
        matrices.Dispose();
        matrices = newArray;
    }

    public static void ChangeFirstElementThatIsZero(this ref NativeArray<float3> array, float3 element)
    {
        if(array.Length == 0 || !array.IsCreated)
            array = new NativeArray<float3>(1, Allocator.Persistent);
        for (int i = 0; i < array.Length; i++)
        {
            if (math.all(array[i] == float3.zero))
            { 
                array[i] = element;
                return;
            }
        }
    }
}
