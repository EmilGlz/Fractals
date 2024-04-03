using UnityEngine;
namespace Assets.Scripts.ScriptableObjects
{
    public class ItemData : ScriptableObject
    {
        public string Id;
        public string Title;
        public GameObject Prefab;
        public Sprite Thumbnail;
    }
}
