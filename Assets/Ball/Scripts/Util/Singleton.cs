using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Space] private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        
    }
}