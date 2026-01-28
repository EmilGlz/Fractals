using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PropertiesPopup : PopupBase
    {
        private const string _contentPath = "Prefabs/PropertiesContent";
        public PropertiesPopup() : base(() => PopupAlignment.Top)
        {
        }
        public static void Create()
        {
            var popup = new PropertiesPopup();
            popup.Show();
        }

        protected override void Show()
        {
            base.Show();
            PopupVerticalLayoutGroup.childControlWidth = PopupVerticalLayoutGroup.childForceExpandWidth = true;
            var content = ResourceHelper.LoadPrefab(_contentPath, PopupRect);
            content.SetActive(false);
            var contentRect = content.GetComponent<RectTransform>();
            contentRect.anchoredPosition = Vector2.zero;
            var contentLg = content.GetComponent<VerticalLayoutGroup>();
            contentLg.padding = new RectOffset(0, 0, 0, 0);
            contentLg.spacing = 6;
            contentLg.childAlignment = TextAnchor.MiddleCenter;
            contentLg.childControlWidth = contentLg.childControlHeight = false;
            contentLg.childForceExpandHeight = contentLg.childForceExpandWidth = false;
            var colorBtn = Utils.FindGameObject<Button>("ChangeColorButton", contentRect);
            var fpsToggle = Utils.FindGameObject<Toggle>("FPSToggle", contentRect);
            colorBtn.onClick.RemoveAllListeners();
            colorBtn.onClick.AddListener(() => 
            {
                ColorPickerPopup.Create(OnColorChange, Main.Instance.FractalManager.CurrentColor); 
            });
            fpsToggle.onValueChanged.RemoveAllListeners();
            fpsToggle.isOn = CanShowFps;
            fpsToggle.onValueChanged.AddListener(FpsToggleClicked);
            Utils.RunAsync(() =>
            {
                content.SetActive(true);
                Utils.ForceUpdateLayout(content);
            });
        }

        private void OnColorChange(Color color)
        {
            Main.Instance.FractalManager.CurrentColor = color;
        }

        private void FpsToggleClicked(bool value)
        {
            CanShowFps = value;
        }

        private bool CanShowFps
        {
            get
            {
                var gameView = Main.Instance.GetView<GameView>();
                if (gameView == null)
                    return false;
                return gameView.CanShowFps;
            }
            set
            {
                var gameView = Main.Instance.GetView<GameView>();
                if (gameView == null)
                    return;
                gameView.CanShowFps = value;
            }
        }
    }
}
