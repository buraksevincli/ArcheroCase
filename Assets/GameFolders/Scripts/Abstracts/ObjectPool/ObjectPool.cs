using System.Collections.Generic;
using UnityEngine;

namespace HHGArchero.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _poolQueue;

        public ObjectPool(T prefab, int initialPoolSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _poolQueue = new Queue<T>();

            for (int i = 0; i < initialPoolSize; i++)
            {
                T obj = Object.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _poolQueue.Enqueue(obj);
            }
        }

        public T Get()
        {
            T obj = _poolQueue.Count > 0 ? _poolQueue.Dequeue() : Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }
}