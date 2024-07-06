using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amulay.Utility;



namespace TankStars.Level
{
    public class blt_sprt_mngr : MonoBehaviour
    {

        public Image[] blt_img;
        ScrollerPlus scrollplus;
        public Slider scrlbar;
        public GameObject[] Tanks;
        string t_selectedTankName;

        void OnEnable()
        {
            init();
        }

       
        // Update is called once per frame
        public void init()
        {
            t_selectedTankName = UpgradeUI.t_selectedTankName;

            if (t_selectedTankName == "tank_yellow")
            {
                for (int i = 2; i > -1; i--)
                {
                    for(int j = 0; j < blt_img.Length; j++)
                    {
                        blt_img[j].sprite = Tanks[0].GetComponent<Tank>()._defaultItems[j].icon;
                    }
                }
            }
            else if (t_selectedTankName == "tank_green1")
            {
                for (int i = 2; i > -1; i--)
                {
                    for (int j = 0; j < blt_img.Length; j++)
                    {
                        blt_img[j].sprite = Tanks[1].GetComponent<Tank>()._defaultItems[j].icon;
                    }
                }
            }
            else if (t_selectedTankName == "tank_green2")
            {
                for (int i = 2; i > -1; i--)
                {
                    for (int j = 0; j < blt_img.Length; j++)
                    {
                        blt_img[j].sprite = Tanks[2].GetComponent<Tank>()._defaultItems[j].icon;
                    }
                }
            }
            else if (t_selectedTankName == "tank_blue")
            {
                for (int i = 2; i > -1; i--)
                {
                    for (int j = 0; j < blt_img.Length; j++)
                    {
                        blt_img[j].sprite = Tanks[3].GetComponent<Tank>()._defaultItems[j].icon;
                    }
                }
            }
        }
    }
    
}