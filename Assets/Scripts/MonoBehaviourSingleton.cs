using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this.GetComponent<T>();
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}