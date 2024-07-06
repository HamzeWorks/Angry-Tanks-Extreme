using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundmanager : MonoBehaviour
{

    public GameObject[] backs;

    public GameObject[] clouds;
    GameObject GO;

   
    // Start is called before the first frame update
    void Start()
    {

      GO =  Instantiate(backs[Random.Range(0, backs.Length)], new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;

        StartCoroutine(instantiatecloud());
    }
 IEnumerator instantiatecloud()
    {
        
        yield return new WaitForSeconds(0.01f);
        if (GameObject.Find("Takhte Jamshid(Clone)"))
        {
            Instantiate(clouds[0], Vector3.zero, Quaternion.identity);
            print("000");
        }
        else if (GameObject.Find("Tehran(Clone)"))
        {
            Instantiate(clouds[1], Vector3.zero, Quaternion.identity);
            print("1111");

        }
        else if (GameObject.Find("Tabriz(Clone)"))
        {
            Instantiate(clouds[2], Vector3.zero, Quaternion.identity);
            print("222");

        }
        else if (GameObject.Find("33Pol(Clone)"))
        {
            Instantiate(clouds[3], Vector3.zero, Quaternion.identity);
            print("3333");

        }

    } 
}
