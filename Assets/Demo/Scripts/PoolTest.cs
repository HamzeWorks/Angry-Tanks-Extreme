using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amulay.Utility;

public class PoolTest : MonoBehaviour
{
    public static Pool<Transform> pool;
    [SerializeField] private GameObject prefab;

    Invoker.Action_Duration action;
    Invoker.Action_Duration action2;
    private void Awake()
    {
        pool = new Pool<Transform>(prefab.transform, true, 3, this.transform);
        action = Invoker.DoUpdate(3, () =>
         {
             Debug.Log("salam1");
             action.Stop();
         });

        action2 = Invoker.DoUpdate(3, () =>
       {
           Debug.Log("salam2");
       });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Transform acquire = pool.acquire;
            pool.Relese(acquire, 3);
            acquire.transform.position = Random.insideUnitSphere * 3;
        }
    }
}
