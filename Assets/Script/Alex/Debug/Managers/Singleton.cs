using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                _instance = FindAnyObjectByType<T>();

                if (_instance == null) {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake() {
        if (_instance != null && _instance != this) {
            Debug.LogWarning($"More than one {typeof(T)}! Destroying the extra instance.");
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }
    }
}