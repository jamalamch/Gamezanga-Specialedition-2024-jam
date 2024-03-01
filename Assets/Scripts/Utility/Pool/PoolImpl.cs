using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pool
{
    [Serializable]
    public class PoolImpl<T> : IPool<T> where T : Component
    {        
        public T Prefab { get; }
        public int NumberToInstantiate { get; }

        private MonoBehaviour _parent;
        private GameObject _poolParent;
        private Transform _poolTransport;
        private Queue<T> _pool;
        private List<T> _picked;
        private bool _isInit = false;



        public PoolImpl (T prefab, int numberToInstantiate, MonoBehaviour parent)
        {
            Precondition.CheckNotNull(prefab);
            Precondition.CheckNotNull(parent);
            
            Prefab = prefab;
            NumberToInstantiate = numberToInstantiate;
            _parent = parent;
            Init();
        }
        
        
        
        public void Init ()
        {
            if (_isInit)
                throw new Exception($"{GetType()} Pool is already Init");

            _poolParent = new GameObject($"{Prefab.name} Pool");
            _poolParent.transform.SetParent(_parent.transform);
            _poolTransport = _poolParent.transform;
            _pool = new Queue<T>();
            _picked = new List<T>();
            _isInit = true;
            BaseInstantiation();
        }

        public T Pick ()
        {
            T instance = PickInstance();
            return instance;
        }

        public T PickDelayed(float delay)
        {
            T instance = PickInstance();
            DOVirtual.DelayedCall(delay, () => Return(instance));
            return instance;
        }

        public void Return (T instance)
        {
            if (IsInPool(instance))
                return;
            ReturnInstance(instance);
        }

        public void ReturnDelayed (T instance, float delay)
        {
            if (IsInPool(instance))
                return;
            DOVirtual.DelayedCall(delay, () => Return(instance));
        }

        public void ReturnAll ()
        {
            for (int i = _picked.Count - 1; i >= 0; i--)
                Return(_picked[i]);
        }

        public void Clear ()
        {
            ReturnAll();
            GameObject.Destroy(_poolParent);
            _pool.Clear();
            _picked.Clear();
            _isInit = false;
        }
        public T[] GetAll()
        {
            return _picked.ToArray();
        }

        private void BaseInstantiation ()
        {
            List<T> instances = new List<T>();
            for (int i = 0; i < NumberToInstantiate; i++)
                instances.Add(PickInstance());
            for (int i = 0; i < instances.Count; i++)
                ReturnInstance(instances[i]);
        }
        
        private bool IsInPool (T instance)
        {
            if (_pool.Contains(instance))
                return true;
            return false;
        }

        private T PickInstance ()
        {
            if (!_isInit)
                Init();

            T instance;

            if (_pool.Count > 0)
                instance = _pool.Dequeue();
            else
            {
                instance = GameObject.Instantiate(Prefab) as T;
                instance.name = $"{Prefab.name} instance";
                instance.transform.SetParent(_poolTransport);
            }

            _picked.Add(instance);
            instance.gameObject.SetActive(true);
            return instance;
        }

        private void ReturnInstance (T instance)
        {
            if (IsInPool(instance))
                return;

            _pool.Enqueue(instance);
            _picked.Remove(instance);
            instance.transform.SetParent(_poolTransport);
            instance.gameObject.SetActive(false);
        }

        private IEnumerator ReturnDelayedRoutine (T instance, float delay)
        {
            yield return new WaitForSeconds(delay);
            Return(instance);
        }

    }
}