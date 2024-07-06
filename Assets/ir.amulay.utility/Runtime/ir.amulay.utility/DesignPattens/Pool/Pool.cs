using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Amulay.Utility
{
    public class Pool<T> where T : Component   //UnityEngine.Object? GameObject? none gameObjects
    {
        /// Summary: prefab.
        public T source { get; protected set; }
        public Transform container { get; protected set; } = null;
        /// Summary: generate new object form source when pool is empty
        public bool isFlexible { get; set; } = false;
        public bool reparentPooledObjects { get; set; } = false;
        public Queue<T> pooledObjects { get; protected set; } //TEST: use stack
        public T acquire { get => Get(); }

        // public string id { get; protected set; }
        public event Action<T> onAddObjectInPool;
        public event Action<T> onReleseObject;
        public event Action<T> onAcquireObject;

        public Pool()
        {
            // PoolsManager.instance.AddPool(this as Pool<Component>, UnityEngine.Random.value.ToString());
            pooledObjects = new Queue<T>();
        }

        public Pool(T source, bool isFlexible = false)
        {
            this.source = source;
            this.isFlexible = isFlexible;
            pooledObjects = new Queue<T>();
        }

        public Pool(T source, bool isFlexible = false, int initialSizeOnStart = 0, Transform container = null, bool reparentPooledObjects = true)
        {
            this.source = source;
            this.isFlexible = isFlexible;
            this.container = container;
            this.reparentPooledObjects = reparentPooledObjects;

            pooledObjects = new Queue<T>(initialSizeOnStart);
            for (int i = 0; i < initialSizeOnStart; i++)
                Instantiate();
        }

        // public Pool(T source, bool isFlexible = false, int initialSizeOnStart = 0, Transform container = null, bool reparentPooledObjects = true, int capacity = 0)
        // {
        //     this.source = source;
        //     this.isFlexible = isFlexible;
        //     this.container = container;

        //     pooledObjects = new Queue<T>(capacity);
        //     for (int i = 0; i < initialSizeOnStart; i++)
        //         Instantiate();
        // }

        protected virtual void Instantiate()
        {
            Add(GameObject.Instantiate<T>(source, container));
        }

        /// Summary: add object in pool directly
        public virtual void Add(T item)
        {
            RestObject(item);
            pooledObjects.Enqueue(item);
            onAddObjectInPool?.Invoke(item);
        }

        public virtual void Relese(T item)
        {
            //TODO: check item is not in queue
            if (item == null)
                return;
            RestObject(item);
            pooledObjects.Enqueue(item);
            onReleseObject?.Invoke(item);
        }

        private void RestObject(T item)
        {
            item.gameObject.SetActive(false);
            if (reparentPooledObjects)
                item.transform.SetParent(container);
        }

        public virtual async void Relese(T item, float delay)
        {
            if (item == null)
                return;
            await System.Threading.Tasks.Task.Delay((int)(delay * 1000));
            if (this == null || item == null)
                return;
            Relese(item);
        }

        /// Summary: acquire object from pool
        public virtual T Get()
        {
            T obj;
            if (pooledObjects.Count > 0)
                obj = pooledObjects.Dequeue();
            else if (isFlexible)
                obj = GameObject.Instantiate<T>(source, container);
            else
                obj = default(T);

            obj?.gameObject?.SetActive(true);
            onAcquireObject?.Invoke(obj);
            return obj;
        }

        public virtual T Get(float lifeTime)
        {
            T obj = Get();
            Relese(obj, lifeTime);
            return obj;
        }
    }
}