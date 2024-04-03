using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIList : IDisposable
    {
        protected readonly Transform _parent;
        protected List<ListItem> Items;

        public UIList(Transform parent)
        {
            _parent = parent;
            GetItems();
            SortItems();
        }

        protected virtual void SortItems()
        {
        }

        public virtual void Load()
        {
            foreach (var item in Items)
                item.Load();
        }

        protected virtual void GetItems()
        { 
        }

        public virtual void Dispose()
        {
            foreach (var item in Items)
                item.Dispose();
        }
    }
}
