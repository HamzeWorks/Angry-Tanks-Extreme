using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bulletcam : MonoBehaviour
{
    
    public GameObject cam,boom;

    public float def_pos;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform.gameObject;
        def_pos = cam.transform.position.x;
        StartCoroutine(onshootCam());
       
    }

    IEnumerator onshootCam()
    {
        cam.GetComponent<Camera>().DOOrthoSize(17, 1.4f);

        yield return new WaitForSeconds(4f);
        cam.GetComponent<Camera>().DOOrthoSize(30, 1.4f);

        //cam.transform.DOMoveX(def_pos, 0.7f);

    }

    private void Update()
    {
        cam.transform.DOMoveX(transform.position.x, 1.5f);

    }
    private void OnEnable()
    {
        if (boom != null)
        {
            if (TankStars.Level.LevelMenu.soundon == false)
            {
                cam.GetComponent<AudioListener>().enabled = false;
            }
        }
    }
}
