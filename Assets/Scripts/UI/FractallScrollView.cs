using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FractallScrollView : View
    {
        [SerializeField] private Transform _content;
        private FractalsList _list;
        protected override string BackgroundPath => "Sprites/space";
        public override void Enter()
        {
            base.Enter();
            if(_list == null)
            {
                _list = new FractalsList(_content);
                _list.Load();
            }
        }
    }
}
