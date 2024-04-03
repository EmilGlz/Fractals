using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ListItem : IDisposable
    {
        public ListItem(ItemData data, Transform parent)
        {
            Data = data;
            Parent = parent;
        }
        protected RectTransform ItemTemplate;

        public ItemData Data { get; }
        protected Transform Parent { get; }

        public virtual void Load()
        {
            var obj = UnityEngine.Object.Instantiate(Data.Prefab, Parent);
            ItemTemplate = obj.GetComponent<RectTransform>();
            var btn = ItemTemplate.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}
