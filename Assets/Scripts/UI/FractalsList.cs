using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FractalsList : UIList
    {
        public FractalsList(Transform parent) : base(parent)
        {
        }

        protected override void GetItems()
        {
            Items = new System.Collections.Generic.List<ListItem>();
            var datas = ResourceHelper.LoadAllScriptableObjects<FractalData>();
            foreach (var data in datas)
                Items.Add(new FractalListItem(data, _parent));
        }

        protected override void SortItems()
        {
            Items.Sort((x, y) => x.Data.Id.CompareTo(y.Data.Id));
        }
    }
}
