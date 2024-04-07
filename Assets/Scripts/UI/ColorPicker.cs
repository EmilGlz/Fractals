using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ColorPicker : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler
    {
        [SerializeField] private Image _currentColorImage;
        [SerializeField] private RectTransform _cursor;
        [SerializeField] private TMP_InputField _hexInput;
        [SerializeField] private TMP_Text _validationErrorText;
        private UnityEvent<Color> OnColorChange = new();
        private RectTransform rectTransform;
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            _cursor.gameObject.SetActive(false);
            _validationErrorText.gameObject.SetActive(false);
        }

        public void Initialize(Color startColor, UnityAction<Color> callBack)
        {
            _hexInput.text = Utils.ColorToHex(startColor)[1..];
            UpdateImageColor(startColor);
            OnColorChange.AddListener(callBack);
        }

        public void SetHexValue()
        {
            var hexInput = _hexInput.text;
            if (!ValidationSuccessfull(hexInput))
                return;
            var color = Utils.ParseHexColor("#" + _hexInput.text);
            SetColor(color);
        }

        private bool ValidationSuccessfull(string hexValue)
        {
            if (hexValue.Length != 6)
            {
                _validationErrorText.text = "Please provide 6 digit hex value";
                _validationErrorText.gameObject.SetActive(true);
                return false;
            }
            _validationErrorText.gameObject.SetActive(false);
            return true;
        }

        public void UpdateImageColor(Color color)
        {
            _currentColorImage.color = color;
        }

        private Color Pick()
        {
            Texture2D colorChart = GetComponent<Image>().sprite.texture;
            Color pickedColor = colorChart.GetPixel(
                (int)(_cursor.localPosition.x * (colorChart.width / rectTransform.rect.width)),
                (int)(_cursor.localPosition.y * (colorChart.height / rectTransform.rect.height))
                );
            return pickedColor;
        }

        private void MoveCursor(Vector2 position)
        {
            _cursor.position = position;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MoveCursor(eventData.position);
            _cursor.gameObject.SetActive(true);
            SelectCurrentCursorPosition();
        }

        private void SelectCurrentCursorPosition()
        {
            var output = Pick();
            SetColor(output);
        }

        private void SetColor(Color output)
        {
            _hexInput.text = Utils.ColorToHex(output)[1..];
            UpdateImageColor(output);
            OnColorChange?.Invoke(output);
        }

        public void OnDrag(PointerEventData eventData)
        {
            MoveCursor(eventData.position);
            _cursor.localPosition = ClampPosition(_cursor.localPosition);
        }

        private Vector2 ClampPosition(Vector2 position)
        {
            float cursorWidth = _cursor.GetWidth();
            float minX = cursorWidth / 2f;
            float minY = -(rectTransform.GetHeight() / 2f - cursorWidth / 2f);
            float maxX = rectTransform.GetWidth() - cursorWidth / 2f;
            float maxY = -minY;

            float clampedX = Mathf.Clamp(position.x, minX, maxX);
            float clampedY = Mathf.Clamp(position.y, minY, maxY);

            return new Vector2(clampedX, clampedY);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SelectCurrentCursorPosition();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _cursor.gameObject.SetActive(true);
        }
    }
}
