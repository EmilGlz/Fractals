using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.D3.Sierpinski
{
    public class SierpinskiTetrahedron : Singleton<SierpinskiTetrahedron>, IFractalManager
    {
        [SerializeField] Transform[] _vertices;
        [SerializeField] private PyramidProperties _properties;

        public Color CurrentColor { 
            get => _properties.Material.color; 
            set => _properties.Material.color = value;
        }
        public bool CanChangeColor => true;

        void Start()
        {
            new Pyramid(Utils.GetPositions(_vertices), _properties, 0);
        }
    }

    public class Pyramid
    {
        private readonly Vector3[] _pivotPositions;
        private readonly PyramidProperties _properties;
        private readonly int _currentIterator;
        private readonly bool _spawnWithAnimation;
        private GameObject Figure;
        public Pyramid(Vector3[] pivotPositions, PyramidProperties properties, int currentIterator, bool spawnWithAnimation = false)
        {
            _pivotPositions = pivotPositions;
            _properties = properties;
            _currentIterator = currentIterator;
            _spawnWithAnimation = spawnWithAnimation;
            if (currentIterator == 0)
                Figure = SpawnObject(_pivotPositions[0], false);
            if (currentIterator < properties.IteratorLimit)
                SierpinskiTetrahedron.Instance.StartCoroutine(GenerateChildren());
        }

        IEnumerator GenerateChildren()
        {
            yield return new WaitForSeconds(_properties.Delay);
            var children = GenerateInsideTriangles();
            foreach (var child in children)
            {
                var obj = SpawnObject(child._pivotPositions[0], child._spawnWithAnimation);
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
                { "AB", Utils.GetMiddle(_pivotPositions[0], _pivotPositions[1]) },
                { "BC", Utils.GetMiddle(_pivotPositions[1], _pivotPositions[2]) },
                { "AC", Utils.GetMiddle(_pivotPositions[0], _pivotPositions[2]) },
                { "AD", Utils.GetMiddle(_pivotPositions[0], _pivotPositions[3]) },
                { "BD", Utils.GetMiddle(_pivotPositions[1], _pivotPositions[3]) },
                { "CD", Utils.GetMiddle(_pivotPositions[2], _pivotPositions[3]) },
            };
            var nextIterator = _currentIterator + 1;
            var newPyramids = new List<Pyramid>() {
                new (new Vector3[]
                {
                    vertices["A"],
                    vertices["AB"],
                    vertices["AC"],
                    vertices["AD"],
                }, _properties, nextIterator, spawnWithAnimation: true),
                new (new Vector3[]
                {
                    vertices["AB"],
                    vertices["B"],
                    vertices["BC"],
                    vertices["BD"],
                }, _properties, nextIterator),
                new (new Vector3[]
                {
                    vertices["AC"],
                    vertices["BC"],
                    vertices["C"],
                    vertices["CD"],
                }, _properties, nextIterator),
                new (new Vector3[]
                {
                    vertices["AD"],
                    vertices["BD"],
                    vertices["CD"],
                    vertices["D"],
                }, _properties, nextIterator),
            };
            return newPyramids;
        }

        private GameObject SpawnObject(Vector3 pivotPos, bool withAnimation)
        {
            var res = Object.Instantiate(_properties.PyramidPrefab, _properties.Parent);
            var targetScale = Figure != null ? Vector3.one * Figure.transform.localScale.x / 2f : Vector3.one;
            var startingScale = Figure != null ? Vector3.one * Figure.transform.localScale.x : Vector3.one;
            res.transform.SetParent(_properties.Parent);
            res.transform.position = pivotPos;
            if (!withAnimation)
                res.transform.localScale = targetScale;
            else
            {
                res.transform.localScale = startingScale;
                SierpinskiTetrahedron.Instance.StartCoroutine(Utils.DecreaseScaleAsync(res.transform, targetScale, _properties.Delay * 0.8f));
            }
            return res;
        }
    }
}
