using Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ColorPickerPopup : PopupBase
    {
        private const string _contentPath = "Prefabs/ColorContent";
        private readonly Action<Color> _onChangeColor;
        private readonly Color _startingColor;
        private ColorPicker _colorPicker;
        public static void Create(Action<Color> onChangeColor, Color startingColor)
        {
            var popup = new ColorPickerPopup(onChangeColor, startingColor);
            popup.Show();
        }

        public ColorPickerPopup(Action<Color> onChangeColor, Color startingColor, Func<PopupAlignment> getAlignment = null, Transform parent = null) : base(()=> PopupAlignment.Top, parent, onBack: PropertiesPopup.Create)
        {
            _onChangeColor = onChangeColor;
            _startingColor = startingColor;
        }

        protected override void Show()
        {
            base.Show();
            PopupVerticalLayoutGroup.childControlWidth = PopupVerticalLayoutGroup.childForceExpandWidth = true;
            var content = ResourceHelper.LoadPrefab(_contentPath, PopupRect);
            content.SetActive(false);
            content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            var contentLg = content.GetComponent<VerticalLayoutGroup>();
            contentLg.padding = new RectOffset(0, 0, 0, 0);
            contentLg.spacing = 30;
            contentLg.childAlignment = TextAnchor.MiddleCenter;
            contentLg.childControlWidth = contentLg.childControlHeight = true;
            contentLg.childForceExpandHeight = contentLg.childForceExpandWidth = false;
            _colorPicker = content.GetComponentInChildren<ColorPicker>();
            _colorPicker.Initialize(_startingColor, OnChangeColor);
            Utils.RunAsync(() =>
            {
                content.SetActive(true);
                Utils.ForceUpdateLayout(content);
            });
        }

        private void OnChangeColor(Color color)
        {
            _onChangeColor?.Invoke(color);
        }
    }
}
