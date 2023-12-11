using UnityEngine;

public class Singleton <T>: MonoBehaviour where T: Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null )
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(_instance != null && _instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
    }
}
