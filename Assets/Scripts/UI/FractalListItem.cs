using Assets.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class FractalListItem : ListItem
    {
        public FractalListItem(FractalData data, Transform parent) : base(data, parent)
        {
        }

        public override void Load()
        {
            base.Load();
            // TODO UI stuff
            var image = Utils.FindGameObject<Image>("Thumbnail", ItemTemplate);
            var title = Utils.FindGameObject<TMP_Text>("Title", ItemTemplate);
            var arf = image.gameObject.GetComponent<AspectRatioFitter>();
            image.sprite = Data.Thumbnail;
            title.text = Data.Title;
            arf.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            arf.aspectRatio = Data.Thumbnail.textureRect.width / Data.Thumbnail.textureRect.height;
        }

        protected override void OnClick()
        {
            if (Data is not FractalData fractal)
                return;
            Main.Instance.EnterView<GameView>();
            Main.Instance.LoadScene(fractal.SceneIndex);
        }
    }
}
