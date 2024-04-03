using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.D2.Sierpinski
{
    public class SierpinskiTriangle : Singleton<SierpinskiTriangle>
    {
        [SerializeField] private Transform[] _angles;
        [SerializeField] private TriangleSettings _triangleSettings;
        private void Start()
        {
            new Triangle(new Vector2[] {
                _angles[0].GetPosition(),
                _angles[1].GetPosition(),
                _angles[2].GetPosition(),
            }, 0, _triangleSettings);
        }

    }
    public class Triangle
    {
        private readonly Vector2[] _angles;
        private readonly int _iterateCount;
        private readonly TriangleSettings _triangleSettings;

        public Triangle(Vector2[] angles, int iterateCount, TriangleSettings triangleSettings)
        {
            _angles = angles;
            _iterateCount = iterateCount;
            _triangleSettings = triangleSettings;
            if (_iterateCount < _triangleSettings.IterateLimit)
                SierpinskiTriangle.Instance.StartCoroutine(GenerateChildren());
        }

        IEnumerator GenerateChildren()
        {
            yield return null;
            DrawDriangleInside();
            GetInsideTriangles();
        }

        public void DrawDriangleInside()
        {
            var lineRendererObj = new GameObject("LineRenderer", typeof(LineRenderer));
            lineRendererObj.transform.SetParent(_triangleSettings.LineSettings.Parent);
            var lineRenderer = lineRendererObj.GetComponent<LineRenderer>();
            lineRenderer.startWidth = _triangleSettings.LineSettings.Width;
            lineRenderer.endWidth = _triangleSettings.LineSettings.Width;
            lineRenderer.startColor = _triangleSettings.LineSettings.Color;
            lineRenderer.endColor = _triangleSettings.LineSettings.Color;
            lineRenderer.material = _triangleSettings.LineSettings.Material;
            lineRenderer.positionCount = 4;
            lineRenderer.SetPosition(0, _angles[0]);
            lineRenderer.SetPosition(1, _angles[1]);
            lineRenderer.SetPosition(2, _angles[2]);
            lineRenderer.SetPosition(3, _angles[0]);
        }

        public List<Triangle> GetInsideTriangles()
        {
            Dictionary<string, Vector2> vertices = new()
        {
            { "A",  _angles[0] },
            { "B",  _angles[1] },
            { "C",  _angles[2] },
            { "AB",  (_angles[0] + _angles[1]) / 2f},
            { "AC",  (_angles[0] + _angles[2]) / 2f},
            { "BC",  (_angles[1] + _angles[2]) / 2f},
        };
            List<Triangle> triangles = new()
        {
            new Triangle(new Vector2[]{
                vertices["A"],
                vertices["AB"],
                vertices["AC"],
            }, _iterateCount + 1, _triangleSettings),
            new Triangle(new Vector2[]{
                vertices["B"],
                vertices["AB"],
                vertices["BC"],
            }, _iterateCount + 1, _triangleSettings),
            new Triangle(new Vector2[]{
                vertices["C"],
                vertices["AC"],
                vertices["BC"],
            }, _iterateCount + 1, _triangleSettings),
        };
            return triangles;
        }
    }
}