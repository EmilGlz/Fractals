using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierpinskiTriangle : MonoBehaviour
{
    [SerializeField] private Transform[] _angles;
    [SerializeField] private int _iterateLimit = 3;
    [SerializeField] private SierpinskiLineSettings _lineSettings;
    private void Start()
    {
        var baseTriangle = new Triangle(new Vector2[] {
                _angles[0].GetPosition(),
                _angles[1].GetPosition(),
                _angles[2].GetPosition(),
            }, 0, _iterateLimit, _lineSettings);
    }

}
public class Triangle
{
    private readonly Vector2[] _angles;
    private readonly int _iterateCount;
    private readonly int _iterateLimit;
    private readonly SierpinskiLineSettings _lineSettings;

    public Triangle(Vector2[] angles, int iterateCount, int iterateLimit, SierpinskiLineSettings lineSettings)
    {
        _angles = angles;
        _iterateCount = iterateCount;
        _iterateLimit = iterateLimit;
        _lineSettings = lineSettings;
        if (_iterateCount < iterateLimit)
            Main.Instance.StartCoroutine(GenerateChildren());
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
        lineRendererObj.transform.SetParent(_lineSettings.Parent);
        var lineRenderer = lineRendererObj.GetComponent<LineRenderer>();
        lineRenderer.startWidth = _lineSettings.Width;
        lineRenderer.endWidth = _lineSettings.Width;
        lineRenderer.startColor = _lineSettings.Color;
        lineRenderer.endColor = _lineSettings.Color;
        lineRenderer.material = _lineSettings.Material;
        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, _angles[0]);
        lineRenderer.SetPosition(1, _angles[1]);
        lineRenderer.SetPosition(2, _angles[2]);
        lineRenderer.SetPosition(3, _angles[0]);
    }

    public List<Triangle> GetInsideTriangles()
    {
        Dictionary<string, Vector2> vertices = new Dictionary<string, Vector2>()
        {
            { "A",  _angles[0] },
            { "B",  _angles[1] },
            { "C",  _angles[2] },
            { "AB",  (_angles[0] + _angles[1]) / 2f},
            { "AC",  (_angles[0] + _angles[2]) / 2f},
            { "BC",  (_angles[1] + _angles[2]) / 2f},
        };
        List<Triangle> triangles = new List<Triangle>()
        {
            new Triangle(new Vector2[]{
                vertices["A"],
                vertices["AB"],
                vertices["AC"],
            }, _iterateCount + 1, _iterateLimit, _lineSettings),
            new Triangle(new Vector2[]{
                vertices["B"],
                vertices["AB"],
                vertices["BC"],
            }, _iterateCount + 1, _iterateLimit, _lineSettings),
            new Triangle(new Vector2[]{
                vertices["C"],
                vertices["AC"],
                vertices["BC"],
            }, _iterateCount + 1, _iterateLimit, _lineSettings),
        };
        return triangles;
    }
}
