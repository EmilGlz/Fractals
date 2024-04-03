using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Main : Singleton<Main>
    {
        protected override bool KeepAlive => true;
        private List<View> _views;
        private View _currentView;
        private GameObject _canvas;
        void Start()
        {
            InitCanvas();
            GetViews();
            HideAllViews();
            EnterView<FractallScrollView>();
        }

        public void InitCanvas()
        {
            _canvas = ResourceHelper.LoadPrefab("Prefabs/Canvas");
            _canvas.transform.SetParent(transform);
        }

        private void HideAllViews()
        {
            foreach (var view in _views)
            { 
                view.gameObject.SetActive(true);
                view.Exit();
            }
        }

        public void LoadScene(int index)
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == index)
                return;
            SceneManager.LoadScene(index);
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
            _views = new List<View>();
            for(int i = 0; i < _canvas.transform.childCount ; i++)
            {
                if(_canvas.transform.GetChild(i).TryGetComponent<View>(out var view))
                    _views.Add(view);
            }
        }
    }
}
