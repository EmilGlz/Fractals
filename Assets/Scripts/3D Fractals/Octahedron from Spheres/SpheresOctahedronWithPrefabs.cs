using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.D3.OctahedronSpheres
{
    public class SpheresOctahedronWithPrefabs : Singleton<SpheresOctahedronWithPrefabs>
    {
        [SerializeField] private SpheresOctahedronPropertiesWithPrefabs _properties;

        void Start()
        {
            new SphereWithPrefab(Vector3.zero, _properties, 0);
        }
    }

    public class SphereWithPrefab
    {
        private const float _outsideRatio = 0.45f;

        private Vector3 _center;
        private readonly SpheresOctahedronPropertiesWithPrefabs _properties;
        private readonly int _currentIterator;
        private readonly float _currentDiameter;

        public SphereWithPrefab(Vector3 center, SpheresOctahedronPropertiesWithPrefabs properties, int currentIterator)
        {
            _center = center;
            _properties = properties;
            _currentIterator = currentIterator;
            _currentDiameter = math.pow(.5f, currentIterator);
            if (currentIterator == 0)
                SpawnObject(Vector3.zero, 1, false);
            if (currentIterator < properties.IteratorLimit)
                SpheresOctahedronWithPrefabs.Instance.StartCoroutine(GenerateChildren());
        }

        private IEnumerator GenerateChildren()
        {
            yield return new WaitForSeconds(_properties.Delay);
            var children = GetChildSpheres();
            foreach (var child in children)
                SpawnObject(child._center, child._currentDiameter, true);
        }

        private void SpawnObject(Vector3 center, float diameter, bool withAnimation)
        {
            var sphere = Object.Instantiate(_properties.Prefab, _properties.Parent);
            sphere.GetComponent<MeshRenderer>().material = _properties.Material;
            sphere.transform.localPosition = center;
            Vector3 targetScale = Vector3.one * diameter;
            if (!withAnimation)
                sphere.transform.localScale = targetScale;
            else
            {
                SpheresOctahedronWithPrefabs.Instance.StartCoroutine(Utils.ChangeScaleAsync(sphere.transform, Vector3.zero, targetScale, 1f));
            }
        }

        private List<SphereWithPrefab> GetChildSpheres()
        {
            var nextIterator = _currentIterator + 1;
            return new List<SphereWithPrefab>() {
                new SphereWithPrefab(_center + _currentDiameter * _outsideRatio * Vector3.up, _properties,nextIterator),
                new SphereWithPrefab(_center + _currentDiameter * _outsideRatio * Vector3.down, _properties,nextIterator),
                new SphereWithPrefab(_center + _currentDiameter * _outsideRatio * Vector3.left, _properties,nextIterator),
                new SphereWithPrefab(_center + _currentDiameter * _outsideRatio * Vector3.right, _properties,nextIterator),
                new SphereWithPrefab(_center + _currentDiameter * _outsideRatio * Vector3.forward, _properties,nextIterator),
                new SphereWithPrefab(_center + _currentDiameter * _outsideRatio * Vector3.back, _properties,nextIterator),
            };
        }
    }
}
