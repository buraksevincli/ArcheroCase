namespace HHGArchero.Utilities
{
    public class Singleton<T> where T : class, new()
    {
        private static volatile T instance;
        private static readonly object lockObject = new object();

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (lockObject)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}