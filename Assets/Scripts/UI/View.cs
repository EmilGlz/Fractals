using UnityEngine;

namespace Assets.Scripts
{
    public class View : MonoBehaviour
    {
        public virtual void Enter()
        {
            gameObject.SetActive(true);
        }
        public virtual void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}
