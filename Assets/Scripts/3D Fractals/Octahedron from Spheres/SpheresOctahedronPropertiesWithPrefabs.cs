using UnityEngine;

namespace Scripts.D3.OctahedronSpheres
{
    [System.Serializable]
    public struct SpheresOctahedronPropertiesWithPrefabs 
    {
        public int IteratorLimit;
        public float Delay;
        public GameObject Prefab;
        public Transform Parent;
        public Material Material;
    }
}
