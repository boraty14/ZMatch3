using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T: SingletonScriptableObject<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] assets = Resources.LoadAll<T>("ScriptableSingletonObjects");
                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception("Could not find any singleton scriptable object");
                }
                if(assets.Length > 1)
                {
                    Debug.LogWarning($"There are multiple instance of scriptable object {nameof(T)}");
                }
                _instance = assets[0];
            }

            return _instance;
        }
    }
}
