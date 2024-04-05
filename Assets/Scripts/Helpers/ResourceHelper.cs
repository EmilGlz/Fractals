using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ResourceHelper
    {
        public static GameObject LoadPrefab(string path, Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError("Failed to load prefab at path: " + path);
                return null;
            }
            GameObject instance = Object.Instantiate(prefab);
            instance.transform.SetParent(parent);
            instance.transform.localScale = Vector3.one;
            return instance;
        }

        public static List<T> LoadAllScriptableObjects<T>(string folderPath) where T : ScriptableObject
        {
            List<T> loadedAssets = new();

            // Load all assets from the specified folder
            T[] assets = Resources.LoadAll<T>(folderPath);
            foreach (T asset in assets)
            {
                loadedAssets.Add(asset);
            }

            return loadedAssets;
        }

        public static T Load<T>(string path) where T : Object
        {
            T loadedObject = Resources.Load<T>(path);

            if (loadedObject == null)
                Debug.LogError("Failed to load resource at path: " + path);

            return loadedObject;
        }
    }
}
