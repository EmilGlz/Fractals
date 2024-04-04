using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FractalData", menuName = "ScriptableObjects/FractalData", order = 1)]
    public class FractalData : ItemData
    {
        public string SceneName;
    }
}
