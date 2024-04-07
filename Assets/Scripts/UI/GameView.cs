using Assets.Scripts.UI;
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
        void Start()
        {
            Main.Instance.OnFractalConstructed.AddListener(InitButtons);
        }
        public override void Enter()
        {
            _canCheckFPS = false;
            base.Enter();
            InitButtons();
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
            fpsText.gameObject.SetActive(_canCheckFPS);
            var backButton = Utils.FindGameObject<Button>("BackButton", transform);
            var colorButton = Utils.FindGameObject<Button>("ColorButton", transform);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                Main.Instance.EnterView<FractallScrollView>();
                Main.Instance.LoadScene("MainMenu");
            });
            if (Main.Instance.FractalManager != null)
            {
                colorButton.gameObject.SetActive(true);
                colorButton.onClick.RemoveAllListeners();
                colorButton.onClick.AddListener(() =>
                {
                    ColorPickerPopup.Create(OnColorChange, Main.Instance.FractalManager.CurrentColor);
                });
            }
            else
                colorButton.gameObject.SetActive(false);
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
        void OnDestroy()
        {
            Main.Instance.OnFractalConstructed.RemoveListener(InitButtons);
        }
    }
}
