using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ResourceHelper
    {
        public static GameObject LoadPrefab(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError("Failed to load prefab at path: " + path);
                return null;
            }
            GameObject instance = Object.Instantiate(prefab);
            return instance;
        }

        public static List<T> LoadAllScriptableObjects<T>() where T : ScriptableObject
        {
            List<T> loadedAssets = new List<T>();

            // Get all paths to assets in the Resources folder
            string[] assetPaths = GetAllResourcePaths<T>();

            // Load all assets that are of type T
            foreach (string path in assetPaths)
            {
                T asset = Resources.Load<T>(path);
                if (asset != null)
                {
                    loadedAssets.Add(asset);
                }
                else
                {
                    Debug.LogWarning("Failed to load asset at path: " + path);
                }
            }

            return loadedAssets;
        }

        private static string[] GetAllResourcePaths<T>() where T : ScriptableObject
        {
            List<string> paths = new List<string>();

            // Find all assets of type T in the Resources folder
            string typeName = typeof(T).Name;
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeName);

            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                if (!string.IsNullOrEmpty(path))
                {
                    paths.Add(GetResourcePath(path));
                }
            }

            return paths.ToArray();
        }

        private static string GetResourcePath(string absolutePath)
        {
            int resourcesIndex = absolutePath.IndexOf("Resources/");
            if (resourcesIndex != -1)
            {
                string relativePath = absolutePath.Substring(resourcesIndex + "Resources/".Length);
                int extensionIndex = relativePath.LastIndexOf('.');
                if (extensionIndex != -1)
                {
                    return relativePath.Substring(0, extensionIndex);
                }
                else
                {
                    return relativePath;
                }
            }
            else
            {
                Debug.LogError("Asset is not in the Resources folder: " + absolutePath);
                return null;
            }
        }
    }
}
