using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class ColorPicker : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector] public UnityEvent<Color> OnColorChange = new();
        public Color output;
        public void OnPointerClick(PointerEventData eventData)
        {
            output = Pick(Camera.main.WorldToScreenPoint(eventData.position), GetComponent<Image>());
            //OnColorChange?.Invoke(output);
        }

        Color Pick(Vector2 screenPoint, Image imageToPick)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(imageToPick.rectTransform, screenPoint, Camera.main, out Vector2 point);
            point += imageToPick.rectTransform.sizeDelta / 2f;
            Texture2D t = GetComponent<Image>().sprite.texture;
            Vector2Int m_point = new(
                (int)((t.width * point.x) / imageToPick.rectTransform.sizeDelta.x),
                (int)((t.height * point.y) / imageToPick.rectTransform.sizeDelta.y)
                );
            return t.GetPixel(m_point.x, m_point.y);
        }
    }
}
