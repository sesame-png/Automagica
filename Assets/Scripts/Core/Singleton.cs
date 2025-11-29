using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    public static T Instance => _instance;
    private static T _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
    }

    private void OnDestroy()
    {
        if (_instance == (object)this)
        {
            _instance = null;
        }
    }
}