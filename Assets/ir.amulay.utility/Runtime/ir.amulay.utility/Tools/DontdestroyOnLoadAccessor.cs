using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Amulay.Utility
{
    // use as "spy" to get all the roots from DontdestroyOnLoad from the "inside" :)
    public class DontdestroyOnLoadAccessor : SingletonDontDestroyOnLoadForce<DontdestroyOnLoadAccessor>
    {
        public GameObject[] GetAllRootsOfDontDestroyOnLoad()
        {
            return this.gameObject.scene.GetRootGameObjects();
        }
    }
}