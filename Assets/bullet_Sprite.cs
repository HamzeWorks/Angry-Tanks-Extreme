using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amulay.Utility;


namespace TankStars.Level
{
    public class bullet_Sprite : MonoBehaviour
    {
        Image img;
        ScrollerPlus scrollplus;
        string t_selectedTankName;
       // public Sprite[] bullets;
        public Slider upgProgress;
        static public bullet_Sprite instance;

        public GameObject[] Tanks;

        Slider selectedSlider;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Update is called once per frame
        public void OnEnable()
        {
            t_selectedTankName = UpgradeUI.t_selectedTankName;
            img = GetComponent<Image>();

            #region Sliders_Stats
            if (t_selectedTankName == "tank_yellow")
            {
                print("esme babash : " + transform.parent.name);
                switch (transform.parent.name)
                {
                    case "bullet (1)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank1missile1");
                        break;
                    case "bullet (2)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank1missile2");
                        break;
                    case "bullet (3)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank1missile3");
                        break;
                }  
            }else if (t_selectedTankName == "tank_green1")
            {
                switch (transform.parent.name)
                {
                    case "bullet (1)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank2missile1");
                        break;
                    case "bullet (2)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank2missile2");
                        break;
                    case "bullet (3)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank2missile3");
                        break;
                }
            }
            else if (t_selectedTankName == "tank_green2")
            {
                switch (transform.parent.name)
                {
                    case "bullet (1)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank3missile1");
                        break;
                    case "bullet (2)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank3missile2");
                        break;
                    case "bullet (3)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank3missile3");
                        break;
                }
            }
            else if (t_selectedTankName == "tank_blue")
            {
                switch (transform.parent.name)
                {
                    case "bullet (1)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank4missile1");
                        break;
                    case "bullet (2)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank4missile2");
                        break;
                    case "bullet (3)":
                        upgProgress = GetComponent<Slider>();
                        upgProgress.value = PlayerPrefs.GetInt("tank4missile3");
                        break;
                }
            }


            #endregion

        }
        public void UpgradeMissiles()
        {
            if (t_selectedTankName == "tank_yellow")
            {
                for (int i = 0; i <= 3; i++)
                {
                    print(i + " is i");
                }         
            }
            else if (t_selectedTankName == "tank_green1")
            {
                for (int i = 0; i < 3; i++)
                {
                    img.sprite = Tanks[1].GetComponent<Tank>()._defaultItems[i].icon;
                    
                }
            }
            else if (t_selectedTankName == "tank_green2")
            {
                for (int i = 0; i < 3; i++)
                {
                    img.sprite = Tanks[2].GetComponent<Tank>()._defaultItems[i].icon;
                }
            }
            else if (t_selectedTankName == "tank_blue")
            {
                for (int i = 0; i < 3; i++)
                {
                    img.sprite = Tanks[3].GetComponent<Tank>()._defaultItems[i].icon;
                }
            }
        }
     /*   public void BulletInfo()
        {
            if (name != "upgrader")
            {
                img = GetComponent<Image>();
            }
            else {

                scrlbar = GetComponent<Slider>();
            }


            if (t_selectedTankName == "tank_yellow")
            {
                switch (name)
                {

                    case "blt1":
                        img.sprite = bullets[0];

                        break;
                    case "blt2":
                        img.sprite = bullets[1];

                        break;
                    case "blt3":
                        img.sprite = bullets[2];

                        break;
                }
                if (name == "upgrader")
                {
                    //ScrollBar of Btns
                    switch (PlayerPrefs.GetInt("tank1missile1"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank1missile2"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank1missile3"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                }
            }
            else if (t_selectedTankName == "tank_green1")
            {
                switch (name)
                {


                    case "blt1":
                        img.sprite = bullets[3];

                        break;
                    case "blt2":
                        img.sprite = bullets[4];

                        break;
                    case "blt3":
                        img.sprite = bullets[5];

                        break;
                }
                if (name == "upgrader")
                {
                    switch (PlayerPrefs.GetInt("tank2missile1"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank2missile2"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank2missile3"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                }
            }
            else if (t_selectedTankName == "tank_green2")
            {
                switch (name)
                {


                    case "blt1":
                        img.sprite = bullets[6];

                        break;
                    case "blt2":
                        img.sprite = bullets[7];

                        break;
                    case "blt3":
                        img.sprite = bullets[8];

                        break;
                }
                if (name == "upgrader")
                {
                    switch (PlayerPrefs.GetInt("tank3missile1"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank3missile2"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank3missile3"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                }
            }
            else if (t_selectedTankName == "tank_blue")
            {
                switch (name)
                {


                    case "blt1":
                        img.sprite = bullets[9];

                        break;
                    case "blt2":
                        img.sprite = bullets[10];

                        break;
                    case "blt3":
                        img.sprite = bullets[11];

                        break;
                }
                if (name == "upgrader")
                {
                    switch (PlayerPrefs.GetInt("tank4missile1"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank4missile2"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                    switch (PlayerPrefs.GetInt("tank4missile3"))
                    {
                        case 0:
                            scrlbar.value = 0;
                            break;
                        case 1:
                            scrlbar.value = 1;
                            break;
                        case 2:
                            scrlbar.value = 2;
                            break;
                        case 3:
                            scrlbar.value = 3;
                            break;
                        case 4:
                            scrlbar.value = 4;
                            break;
                    }
                }
            }
        }*/
    }
   
}