using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class View : MonoBehaviour
    {
        private Sprite _backgroundSprite;
        protected virtual string BackgroundPath => "";
        private void UpdateBackground()
        {
            bool canShowImage = !string.IsNullOrEmpty(BackgroundPath);
            var img = Utils.FindGameObject<Image>("Background", Main.Instance.Canvas);
            if(!canShowImage)
            {
                img.gameObject.SetActive(false);
                return;
            }
            if (_backgroundSprite == null)
                _backgroundSprite = ResourceHelper.Load<Sprite>(BackgroundPath);
            if(_backgroundSprite == null)
            {
                img.gameObject.SetActive(false);
                return;
            }
            img.sprite = _backgroundSprite;
            img.gameObject.SetActive(true);
        }

        public virtual void Enter()
        {
            gameObject.SetActive(true);
            UpdateBackground();
        }
        public virtual void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}
