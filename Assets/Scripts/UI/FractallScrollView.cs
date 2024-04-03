using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FractallScrollView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        void Start()
        {
            EnterView();    
        }

        private void EnterView()
        {
            var list = new FractalsList(_content);
            list.Load();
        }
    }
}
