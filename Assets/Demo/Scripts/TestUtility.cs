using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amulay.Utility;

public class TestUtility : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        Invoker.Do(1, () => { print("HI"); });
        print(DontdestroyOnLoadAccessor.instance.GetAllRootsOfDontDestroyOnLoad().Length);
        print(DontdestroyOnLoadAccessor.instance.GetAllRootsOfDontDestroyOnLoad().Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
