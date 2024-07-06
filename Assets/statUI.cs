using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amulay.Utility;

public class statUI : MonoBehaviour
{

    upgradesystem upg;
    Slider HP_STATS;
    public Image HP_lvl_Image;
    static public statUI instance;
    public Sprite[] lvls;
    public Text HP_Text;
    // Start is called before the first frame update
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        HP_STATS = GetComponent<Slider>();
        upg = upgradesystem.instance;
        TankInfo();

        
    }



    // Update is called once per frame
    public void TankInfo()
    {
        
            #region TANK 1
            if (name == "Slider1")
            {
                switch (PlayerPrefs.GetInt("tank1"))
                {
                    case 0:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank1"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[0].Items[i].health.ToString();
                        }
                        break;
                    case 1:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank1"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[0].Items[i].health.ToString();

                        }
                        break;
                    case 2:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank1"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[0].Items[i].health.ToString();
                        }
                        break;

                }
            }
        #endregion
        #region TANK 2

        if (name == "Slider2")
        {
            switch (PlayerPrefs.GetInt("tank2"))
            {
                case 0:

                    for (int i = 0; i <= PlayerPrefs.GetInt("tank2"); i++)
                    {
                        HP_lvl_Image.sprite = lvls[i];
                        HP_Text.text = upg.tanks[1].Items[i].health.ToString();

                    }
                    break;
                case 1:

                    for (int i = 0; i <= PlayerPrefs.GetInt("tank2"); i++)
                    {
                        HP_lvl_Image.sprite = lvls[i];
                        HP_Text.text = upg.tanks[1].Items[i].health.ToString();

                    }
                    break;
                case 2:

                    for (int i = 0; i <= PlayerPrefs.GetInt("tank2"); i++)
                    {
                        HP_lvl_Image.sprite = lvls[i];
                        HP_Text.text = upg.tanks[1].Items[i].health.ToString();

                    }
                    break;

            }
        }
            #endregion
            #region TANK 3
            if (name == "Slider3")
            {
                switch (PlayerPrefs.GetInt("tank3"))
                {
                    case 0:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank3"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[2].Items[i].health.ToString();

                        }
                        break;
                    case 1:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank3"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[2].Items[i].health.ToString();

                        }
                        break;
                    case 2:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank3"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[2].Items[i].health.ToString();

                        }
                        break;

                }
            }
            #endregion
            #region TANK 4
            if (name == "Slider4")
            {
                switch (PlayerPrefs.GetInt("tank4"))
                {
                    case 0:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank4"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[3].Items[i].health.ToString();

                        }
                        break;
                    case 1:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank4"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[3].Items[i].health.ToString();

                        }
                        break;
                    case 2:

                        for (int i = 0; i <= PlayerPrefs.GetInt("tank4"); i++)
                        {
                            HP_lvl_Image.sprite = lvls[i];
                            HP_Text.text = upg.tanks[3].Items[i].health.ToString();

                        }
                        break;

                }
            }
            #endregion
        
    }
    private void LateUpdate()
    {
        TankInfo();
    }
}
