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
            foreach (var data in Main.Instance.FractalDatas)
                Items.Add(new FractalListItem(data, _parent));
        }
    }
}
