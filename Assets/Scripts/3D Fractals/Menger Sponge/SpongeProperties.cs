using UnityEngine;

namespace Scripts.D3.Menger
{
    [System.Serializable]
    public struct SpongeProperties
    {
        public GameObject Prefab;
        public Transform Parent;
        public int IterationLimit;
    }
}
