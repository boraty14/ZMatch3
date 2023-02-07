using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Scripts.Helpers
{
    /// <summary>
    /// A simple base class to simplify object pooling in Unity 2021.
    /// </summary>
    /// <typeparam name="T">A MonoBehaviour object you'd like to perform pooling on.</typeparam>
    public abstract class PoolerBase<T> : MonoBehaviour where T : MonoBehaviour 
    {
        private T _prefab;
        private ObjectPool<T> _pool;

        private ObjectPool<T> Pool {
            get {
                if (_pool == null) throw new InvalidOperationException("You need to call InitPool before using it.");
                return _pool;
            }
            set => _pool = value;
        }

        protected void InitPool(T prefab, int initial = 10, int max = 100, bool collectionChecks = false) {
            _prefab = prefab;
            Pool = new ObjectPool<T>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                collectionChecks,
                initial,
                max);
        }

        #region Overrides
        protected virtual T CreateSetup() => Instantiate(_prefab,transform);
        protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
        protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
        protected virtual void DestroySetup(T obj) => Destroy(obj);
        #endregion

        #region Getters
        public T GetItemFromPool() => Pool.Get();
        public void Release(T obj) => Pool.Release(obj);
        #endregion
    }
}