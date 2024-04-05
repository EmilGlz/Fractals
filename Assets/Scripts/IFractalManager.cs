using Assets.Scripts;
using UnityEngine;

namespace Scripts
{
    public interface IFractalManager
    {
        public void PromoteClass() {
            Main.Instance.Construct(this);
        }
        public bool CanChangeColor { get; }
        public abstract Color CurrentColor { get; set; }
    }
}
