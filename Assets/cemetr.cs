using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cemetr : MonoBehaviour
{
    GameObject cam;
    public GameObject cemetry;

    void Start()
    {
        
        cam = GameObject.Find("Main Camera");
        StartCoroutine(docemetryjob());
    }

   IEnumerator docemetryjob()
    {

        yield return new WaitForSeconds(1.5f);
        cemetry.SetActive(true);
        cam.transform.DOMove(new Vector3( transform.position.x,transform.position.y,transform.position.z - 10), 0.5f);
    }
}
