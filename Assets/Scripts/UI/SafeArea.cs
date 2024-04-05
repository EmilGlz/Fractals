using UnityEngine;

namespace Assets.Scripts
{
    public class SafeArea : MonoBehaviour
    {
        RectTransform rectTransform;
        Rect safeArea;
        Vector2 minAnchor;
        Vector2 maxAnchor;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            safeArea = Screen.safeArea;
            minAnchor = safeArea.position;
            maxAnchor = minAnchor + safeArea.size;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
            rectTransform.pivot = Vector2.zero;
        }

        public float Height => rectTransform != null ? rectTransform.GetHeight() : 0;
        public float Width => rectTransform != null ? rectTransform.GetWidth() : 0;
        public Vector2 AnchorMax => rectTransform.anchorMax;
        public Vector2 AnchorMin => rectTransform.anchorMin;
    }
}
