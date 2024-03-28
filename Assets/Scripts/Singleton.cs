using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual bool KeepAlive => false;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (KeepAlive)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}
