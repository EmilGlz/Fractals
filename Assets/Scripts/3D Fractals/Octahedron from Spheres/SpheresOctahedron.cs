using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.D3.OctahedronSpheres
{
    public class SpheresOctahedron : Singleton<SpheresOctahedron>
    {
        [SerializeField] SpheresOctahedronProperties _properties;
        void Start()
        {
            new Sphere(Vector3.zero, _properties, 0);
        }
    }

    public class Sphere
    {
        private const float _outsideRatio = 0.45f;

        private Vector3 _center;
        private readonly SpheresOctahedronProperties _properties;
        private readonly int _currentIterator;
        private readonly float _currentDiameter;

        public Sphere(Vector3 center, SpheresOctahedronProperties properties, int currentIterator)
        {
            _center = center;
            _properties = properties;
            _currentIterator = currentIterator;
            _currentDiameter = math.pow(.5f, currentIterator);
            if (currentIterator == 0)
                SpawnObject(Vector3.zero, 1);
            if (currentIterator < properties.IteratorLimit)
                SpheresOctahedron.Instance.StartCoroutine(GenerateChildren());
        }

        private IEnumerator GenerateChildren()
        {
            yield return new WaitForSeconds(_properties.Delay);
            var children = GetChildSpheres();
            foreach (var child in children)
                SpawnObject(child._center, child._currentDiameter);
        }

        private void SpawnObject(Vector3 center, float diameter)
        {
            Instancer.Instance.SpawnMesh(center, Quaternion.identity, Vector3.one * diameter);
        }

        private List<Sphere> GetChildSpheres()
        {
            var nextIterator = _currentIterator + 1;
            return new List<Sphere>() {
                new Sphere(_center + _currentDiameter * _outsideRatio * Vector3.up, _properties,nextIterator),
                new Sphere(_center + _currentDiameter * _outsideRatio * Vector3.down, _properties,nextIterator),
                new Sphere(_center + _currentDiameter * _outsideRatio * Vector3.left, _properties,nextIterator),
                new Sphere(_center + _currentDiameter * _outsideRatio * Vector3.right, _properties,nextIterator),
                new Sphere(_center + _currentDiameter * _outsideRatio * Vector3.forward, _properties,nextIterator),
                new Sphere(_center + _currentDiameter * _outsideRatio * Vector3.back, _properties,nextIterator),
            };
        }
    }
}
