using UnityEngine;

namespace HHGArchero.Utilities
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static volatile T instance;

        public static T Instance => instance;

        [SerializeField] private bool dontDestroyOnLoad; 
        
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                if (!dontDestroyOnLoad) return;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}