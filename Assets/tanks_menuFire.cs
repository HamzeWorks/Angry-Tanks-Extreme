using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tanks_menuFire : MonoBehaviour
{
    public GameObject tank_obj,Fire_FX,Muzzle;

    const string Fire_Anim = "Fire";
    const string Idle_Anim = "Idle";
     Animator myAnimator;
    bool DoAnim;

    public GameObject[] firepoints;

    void Start()
    {
        myAnimator = tank_obj.GetComponent<Animator>();
        DoAnim = true;
    }

    void OnMouseDown()
    {
        if (DoAnim)
        {
            StartCoroutine(TankAnim());
        }
    }
    IEnumerator TankAnim()
    {
        DoAnim = false;
        myAnimator.SetBool("CanFire", true);
        GameObject FX,muzzle;
        yield return new WaitForSeconds(0.2f);
        for(int i = 0; i <= 5; i++)
        {
         Instantiate(Fire_FX, firepoints[i].transform.position, firepoints[i].transform.rotation);
         Instantiate(Muzzle, firepoints[i].transform.position, firepoints[i].transform.rotation);
            yield return new WaitForSeconds(0.2f);

        }



        yield return new WaitForSeconds(1.5f);
        myAnimator.SetTrigger(Idle_Anim);
       
        myAnimator.SetBool("CanFire", false);

        DoAnim = true;
    }
}
