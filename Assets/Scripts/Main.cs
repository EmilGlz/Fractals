using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.UI;
using Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Main : Singleton<Main>
    {
        public List<FractalData> FractalDatas;
        protected override bool KeepAlive => true;
        private List<View> _views;
        private View _currentView;
        private RectTransform _canvas;
        void Start()
        {
            AdjustPerformance();
            InitCanvas();
            GetViews();
            HideAllViews();
            EnterView<FractallScrollView>();
        }

        public IFractalManager FractalManager { get; private set; }
        public void Construct(IFractalManager fractalManager)
        {
            FractalManager = fractalManager;
        }

        public void InitCanvas()
        {
            _canvas = ResourceHelper.LoadPrefab("Prefabs/Canvas").GetComponent<RectTransform>();
            _canvas.SetParent(transform);
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
            for (int i = 0; i < _canvas.transform.childCount; i++)
                _canvas.transform.GetChild(i).gameObject.SetActive(true);
            _views = _canvas.transform.GetComponentsInChildren<View>().ToList();
        }

        private void AdjustPerformance()
        {
            Application.targetFrameRate = Device.TargetFrameRate; // Use device defaults
        }

        public float DeviceHeight => _canvas.GetHeight();
        public float DeviceWidth => _canvas.GetWidth();
    }
}
