using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amulay.Utility
{
    public class PoolsManager : SingletonForce<PoolsManager>
    {
        private Dictionary<string, Pool<Component>> a;
        //type . is dont destroy on load?
        protected override void Awake()
        {

        }

        public void AddPool(Pool<Component> s, string id)
        {
            if (a.ContainsKey(id))
                throw new System.Exception("Pool id is not unique");
            a.Add(id, s);
        }

        public T GetObjectFromPool<T>(string poolId) where T : Component
        {
            if (a.ContainsKey(poolId) == false)
            {
                Debug.LogWarning("pool id is wrong");
                return default(T);
            }
            return a[poolId].acquire as T;
        }
    }
}