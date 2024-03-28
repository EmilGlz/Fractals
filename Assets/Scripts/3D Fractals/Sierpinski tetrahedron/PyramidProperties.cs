using UnityEngine;
namespace Scripts
{
    [System.Serializable]
    public struct PyramidProperties
    {
        public GameObject PyramidPrefab;
        public Transform Parent;
        public Material Material;
        public int IteratorLimit;
        public float Delay;
    }
}
