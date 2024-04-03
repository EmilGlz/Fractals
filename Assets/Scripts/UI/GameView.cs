using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameView : View
    {
        public TMP_Text fpsText;
        float deltaTime = 0.0f;
        private bool _canCheckFPS;

        void Awake()
        {
            InitButtons();
        }

        public override void Enter()
        {
            base.Enter();
            _canCheckFPS = true;
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
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                Main.Instance.EnterView<FractallScrollView>();
                Main.Instance.LoadScene(0);
            });
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
