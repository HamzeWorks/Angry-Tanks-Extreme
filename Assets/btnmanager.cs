using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnmanager : MonoBehaviour
{
    [SerializeField]
    GameObject aboutus_Panel;
    [SerializeField]
    GameObject aboutus_Btn;

    [SerializeField]
    GameObject aboutus_text;
    [SerializeField]
    GameObject privacy_policy_text;
    [SerializeField]
    GameObject privacy_policy_btn_text;
    int touch = 0;
    public void BackToMenu()
    {
        aboutus_Btn.SetActive(true);
        aboutus_Panel.SetActive(false);
    }
    public void GoToPrivacy()
    {

        if (touch == 0)
        {
            privacy_policy_text.SetActive(true);
            aboutus_text.SetActive(false);
            privacy_policy_btn_text.GetComponent<Text>().text = "About Us";
            touch = 1;
        }else
        {
            //OnEnable();
            privacy_policy_btn_text.GetComponent<Text>().text = "Privacy Policy";
            aboutus_text.SetActive(true);
            privacy_policy_text.SetActive(false);
            touch = 0;
        }
        print(touch + " is touch !");
    }
    public void GoToAboutUs()
    {
        aboutus_Panel.SetActive(true);
        aboutus_Btn.SetActive(false);
        privacy_policy_btn_text.GetComponent<Text>().text = "Privacy Policy";
        aboutus_text.SetActive(true);
        privacy_policy_text.SetActive(false);
    }
    private void OnEnable()
    {
        touch = 0;
        if (touch == 0)
        {
            aboutus_text.SetActive(true);
            privacy_policy_text.SetActive(false);
            privacy_policy_btn_text.GetComponent<Text>().text = "About Us";
        }
    }

}
