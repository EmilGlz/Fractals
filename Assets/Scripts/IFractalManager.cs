using Assets.Scripts;
using UnityEngine;

namespace Scripts
{
    public interface IFractalManager
    {
        public void PromoteClass()
        {
            if (Main.Instance != null)
                Main.Instance.Construct(this);
        }
        public bool CanChangeColor { get; }
        public abstract Color CurrentColor { get; set; }
    }
}
