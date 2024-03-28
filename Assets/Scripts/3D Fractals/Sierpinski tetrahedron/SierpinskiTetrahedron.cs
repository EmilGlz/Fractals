using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.D3.Sierpinski
{
    public class SierpinskiTetrahedron : MonoBehaviour
    {
        [SerializeField] Transform[] _vertices;
        [SerializeField] private PyramidProperties _properties;
        void Start()
        {
            new Pyramid(new Vector3[] {
                _vertices[0].position,
                _vertices[1].position,
                _vertices[2].position,
                _vertices[3].position,
            }, _properties, 0);
        }
    }

    public class Pyramid
    {
        private readonly Vector3[] _pivotPositions;
        private readonly PyramidProperties _properties;
        private readonly int _currentIterator;
        private readonly Vector3 _baseCenter;
        private GameObject Figure;
        public Pyramid(Vector3[] pivotPositions, PyramidProperties properties, int currentIterator)
        {
            _pivotPositions = pivotPositions;
            _properties = properties;
            _currentIterator = currentIterator;
            _baseCenter = GetMiddle(_pivotPositions[0], _pivotPositions[1], _pivotPositions[2]);
            if (currentIterator < properties.IteratorLimit)
                Main.Instance.StartCoroutine(GenerateChildren());
        }

        IEnumerator GenerateChildren()
        {
            yield return new WaitForSeconds(_properties.Delay);
            var children = GenerateInsideTriangles();
            foreach (var child in children)
            {
                var obj = SpawnObject(child._pivotPositions[0], child._baseCenter);
                child.Figure = obj;
            }
            Object.Destroy(Figure);
        }

        private List<Pyramid> GenerateInsideTriangles()
        {
            var vertices = new Dictionary<string, Vector3>() {
                { "A", _pivotPositions[0] },
                { "B", _pivotPositions[1] },
                { "C", _pivotPositions[2] },
                { "D", _pivotPositions[3] },
                { "AB", GetMiddle(_pivotPositions[0], _pivotPositions[1]) },
                { "BC", GetMiddle(_pivotPositions[1], _pivotPositions[2]) },
                { "AC", GetMiddle(_pivotPositions[0], _pivotPositions[2]) },
                { "AD", GetMiddle(_pivotPositions[0], _pivotPositions[3]) },
                { "BD", GetMiddle(_pivotPositions[1], _pivotPositions[3]) },
                { "CD", GetMiddle(_pivotPositions[2], _pivotPositions[3]) },
            };
            var nextIterator = _currentIterator + 1;
            var newPyramids = new List<Pyramid>() {
                new Pyramid(new Vector3[]
                {
                    vertices["A"],
                    vertices["AC"],
                    vertices["AB"],
                    vertices["AD"],
                }, _properties, nextIterator),
                new Pyramid(new Vector3[]
                {
                    vertices["B"],
                    vertices["AB"],
                    vertices["BC"],
                    vertices["BD"],
                }, _properties, nextIterator),
                new Pyramid(new Vector3[]
                {
                    vertices["C"],
                    vertices["BC"],
                    vertices["AC"],
                    vertices["CD"],
                }, _properties, nextIterator),
                new Pyramid(new Vector3[]
                {
                    vertices["AD"],
                    vertices["CD"],
                    vertices["BD"],
                    vertices["D"],
                },_properties,nextIterator),
            };
            return newPyramids;
        }

        private GameObject SpawnObject(Vector3 pivotPos, Vector3 parentBaseCenter)
        {
            var res = Object.Instantiate(_properties.PyramidPrefab, _properties.Parent);
            res.transform.localScale = Vector3.one * (Figure != null ? Figure.transform.localScale.x / 2f : .5f);
            res.transform.SetParent(_properties.Parent);
            res.transform.position = pivotPos;
            LookAt(res.transform, parentBaseCenter);
            return res;
        }

        void LookAt(Transform transform, Vector3 targetPosition)
        {
            Vector3 directionToTarget = targetPosition - transform.position;
            directionToTarget.y = 0f; // Ignore vertical component

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }

        private static Vector3 GetMiddle(Vector3 pos1, Vector3 pos2)
        {
            return (pos1 + pos2) / 2f;
        }

        private static Vector3 GetMiddle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            return (pos1 + pos2 + pos3) / 3f;
        }
    }
}
