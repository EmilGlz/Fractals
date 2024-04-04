using System.Collections;
using UnityEngine;
namespace Assets.Scripts.D2.Sierpinski
{
    public class SierpinskiRandomDot : MonoBehaviour
    {
        [SerializeField] private GameObject _dotPrefab;
        [SerializeField] private Transform[] _angles;
        [SerializeField] private float dotRadius = 1f;
        private Vector2 _currentPos;
        void Start()
        {
            StartCoroutine(StartDrawing());
        }

        private IEnumerator StartDrawing()
        {
            _currentPos = GetRandomPositionInTriangle();
            SpawnDot(_currentPos);
            while (true)
            {
                yield return null;
                var anglePos = _angles[Random.Range(0, 3)];
                _currentPos = (_currentPos + anglePos.GetPosition()) / 2f;
                SpawnDot(_currentPos);
            }
        }

        private void SpawnDot(Vector2 pos)
        {
            var obj = Instantiate(_dotPrefab, pos, Quaternion.identity, transform);
            obj.transform.localScale = 0.1f * dotRadius * Vector3.one;
        }

        private Vector2 GetRandomPositionInTriangle()
        {
            float rand1 = Random.Range(0f, 1f);
            float rand2 = Random.Range(0f, 1f);

            if (rand1 + rand2 > 1f)
            {
                rand1 = 1f - rand1;
                rand2 = 1f - rand2;
            }
            float rand3 = 1f - rand1 - rand2;
            Vector2 randomPosition = rand1 * _angles[0].position + rand2 * _angles[1].position + rand3 * _angles[2].position;
            return randomPosition;
        }
    }
}