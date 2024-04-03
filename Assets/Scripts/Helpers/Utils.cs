using System;
using System.Collections;
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

    public static Vector3[] GetPositions(Transform[] transforms)
    {
        var res = new Vector3[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
            res[i] = transforms[i].position;
        return res;
    }

    public static void LookAt(Transform transform, Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0f; // Ignore vertical component

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    public static IEnumerator DecreaseScaleAsync(Transform transform, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float timer = 0f;
        while (timer < duration)
        {
            float progress = timer / duration;
            Vector3 currentScale = Vector3.Lerp(initialScale, targetScale, progress);
            transform.localScale = currentScale;
            yield return null;
            timer += Time.deltaTime;
        }
        transform.localScale = targetScale;
    }

    public static GameObject FindGameObject(string name, GameObject parentOrSelf)
    {
        if (parentOrSelf == null)
            return null;
        if (parentOrSelf.name == name)
            return parentOrSelf;
        var components = parentOrSelf.GetComponentsInChildren<Transform>(true);
        foreach (Transform component in components)
        {
            if (component.gameObject.name == name)
                return component.gameObject;
        }
        return null;
    }

    public static GameObject FindGameObjectBySubstr(string str, GameObject parentOrSelf)
    {
        if (parentOrSelf == null)
            return null;
        if (parentOrSelf.name.IndexOf(str, StringComparison.Ordinal) != -1)
            return parentOrSelf;
        var components = parentOrSelf.GetComponentsInChildren<Transform>(true);
        foreach (Transform component in components)
        {
            if (component.gameObject.name.IndexOf(str, StringComparison.Ordinal) != -1)
                return component.gameObject;
        }
        return null;
    }

    public static T FindGameObject<T>(string name, Transform parentOrSelf)
    where T : MonoBehaviour
    {
        if (parentOrSelf == null)
            return null;
        var go = FindGameObject(name, parentOrSelf.gameObject);
        if (go == null) return null;
        return go.GetComponent<T>();
    }
}