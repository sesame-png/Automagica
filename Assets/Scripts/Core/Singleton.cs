using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
    }

    private void OnDestroy()
    {
        if (instance == (object)this)
        {
            instance = null;
        }
    }
}