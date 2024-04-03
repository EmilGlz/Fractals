using Assets.Scripts.UI;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameView : View
    {
        void Awake()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            var backButton = Utils.FindGameObject<Button>("BackButton", transform);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                Main.Instance.EnterView<FractallScrollView>();
                Main.Instance.LoadScene(0);
            });
        }
    }
}
