using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Component // TODO: Add constraint so only the type of the derived class can be used as generic
{
    private static T _instance;
    private static readonly object _padlock = new object();

    public static T Instance
    {
        get
        {
            lock (_padlock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
