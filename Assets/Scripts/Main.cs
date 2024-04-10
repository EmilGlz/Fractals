using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.UI;
using Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Main : Singleton<Main>
    {
        public List<FractalData> FractalDatas;
        protected override bool KeepAlive => true;
        private List<View> _views;
        private View _currentView;
        public RectTransform Canvas { get; private set; }
        public RectTransform PopupLayer { get; private set; }
        private SafeArea SafeArea;
        void Start()
        {
            AdjustPerformance();
            InitCanvas();
            GetViews();
            HideAllViews();
            EnterView<FractallScrollView>();
            InitDeviceDatas();
        }

        public IFractalManager FractalManager { get; private set; }
        public UnityEvent OnFractalConstructed;
        public void Construct(IFractalManager fractalManager)
        {
            FractalManager = fractalManager;
            OnFractalConstructed?.Invoke();
        }

        public void InitCanvas()
        {
            Canvas = ResourceHelper.LoadPrefab("Prefabs/Canvas", transform).GetComponent<RectTransform>();
            PopupLayer = Utils.FindGameObject("PopupLayer", Canvas.gameObject).GetComponent<RectTransform>();
        }

        private void HideAllViews()
        {
            foreach (var view in _views)
            { 
                view.gameObject.SetActive(true);
                view.Exit();
            }
        }

        public void LoadScene(string sceneName)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == sceneName)
                return;
            FractalManager = null;
            SceneManager.LoadScene(sceneName);
        }

        public void EnterView<T>() where T : View
        {
            if (_currentView != null)
                _currentView.Exit();
            _currentView = _views.FirstOrDefault(v => v is T);
            _currentView.Enter();
        }

        private void GetViews()
        {
            _views = Canvas.transform.GetComponentsInChildren<View>().ToList();
            foreach (var item in _views)
                item.gameObject.SetActive(true);
        }

        public T GetView<T>() where T : View
        { 
            var res = _views.FirstOrDefault(v => v is T a);
            return res != null ? res as T : null;
        }

        private void AdjustPerformance()
        {
            Application.targetFrameRate = Device.TargetFrameRate; // Use device defaults
        }

        private void InitDeviceDatas()
        {
            SafeArea = Canvas.GetComponentInChildren<SafeArea>();
            Vector2 screenSize = new(Canvas.GetWidth(), Canvas.GetHeight());
            Device.Height = screenSize.y;
            Device.Width = screenSize.x;
            Device.BottomOffset = (int)(screenSize.y * SafeArea.AnchorMin.y);
            Device.TopOffset = (int)(screenSize.y * (1 - SafeArea.AnchorMax.y));
            Device.LeftOffset = (int)(screenSize.x * SafeArea.AnchorMin.x);
            Device.RightOffset = (int)(screenSize.x * (1 - SafeArea.AnchorMax.x));
        }
    }
}
