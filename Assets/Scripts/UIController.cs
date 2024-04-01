using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
