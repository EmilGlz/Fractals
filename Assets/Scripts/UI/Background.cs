using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Background : IDisposable
    {
        private const string _prefabName = "Prefabs/Background";
        private readonly Action _onClick;
        private GameObject _instance;
        public Background(Action onClick)
        {
            _onClick = onClick;
            _instance = ResourceHelper.LoadPrefab(_prefabName, Main.Instance.PopupLayer);
            _instance.transform.SetAsLastSibling();
            
            var img = _instance.GetComponent<Image>();
            img.sprite = null;
            img.color = new Color(0, 0, 0, .5f);

            var rect = _instance.GetComponent<RectTransform>();
            rect.anchorMin = Vector3.zero;
            rect.anchorMax = Vector3.one;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            var btn = _instance.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                _onClick.Invoke();
            });
        }

        public void Dispose()
        {
            if(_instance != null)
            {
                UnityEngine.Object.Destroy(_instance);
                _instance = null;
            }
        }
    }
}
