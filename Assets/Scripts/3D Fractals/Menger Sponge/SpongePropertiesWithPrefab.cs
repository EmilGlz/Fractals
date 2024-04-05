using UnityEngine;

namespace Assets.Scripts.D3.Menger
{
    [System.Serializable]
    public struct SpongePropertiesWithPrefab
    {
        public GameObject Prefab;
        public Transform Parent;
        public Material Material;
        public int IteratorLimit;
        public float Delay;
    }
}
