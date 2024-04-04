using Assets.Scripts.UI;
using DG.Tweening;
using Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameView : View
    {
        private TMP_Text fpsText;
        float deltaTime = 0.0f;
        private bool _canCheckFPS;
        private ColorPicker _colorPicker;

        void Awake()
        {
            InitButtons();
        }

        public override void Enter()
        {
            base.Enter();
            _canCheckFPS = true;
            _colorPicker = Utils.FindGameObject<ColorPicker>("ColorImage", transform);
            _colorPicker.OnColorChange.RemoveAllListeners();
            _colorPicker.OnColorChange.AddListener(OnColorChange);
            _colorPicker.GetComponent<RectTransform>().SetPosY(0);
        }

        private void OnColorChange(Color color)
        {
            Main.Instance.FractalManager.CurrentColor = color;
        }

        public override void Exit()
        {
            base.Exit();
            _canCheckFPS = false;
        }

        private void InitButtons()
        {
            fpsText = Utils.FindGameObject<TMP_Text>("FPSText", transform);
            var backButton = Utils.FindGameObject<Button>("BackButton", transform);
            var colorButton = Utils.FindGameObject<Button>("ColorButton", transform);
            backButton.onClick.RemoveAllListeners();
            colorButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                Main.Instance.EnterView<FractallScrollView>();
                Main.Instance.LoadScene("MainMenu");
            });
            colorButton.onClick.AddListener(OpenColorPicker);
        }

        private void OpenColorPicker()
        {
            var popupRect = _colorPicker.GetComponent<RectTransform>();
            popupRect.SetPosY(0);
            var popupHeight = popupRect.GetHeight();
            popupRect.DOMoveY(popupHeight, .5f);
        }

        private void CloseColorPicker()
        {
            var popupRect = _colorPicker.GetComponent<RectTransform>();
            popupRect.SetPosY(popupRect.GetHeight());
            popupRect.DOMoveY(0, .5f);
        }

        void Update()
        {
            if (!_canCheckFPS)
                return;
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            if (fpsText != null)
                fpsText.text = text;
        }
    }
}
