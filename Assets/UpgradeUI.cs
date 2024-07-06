using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Amulay.Utility;
using TankStars.Level;
using TankStars;

namespace TankStars.Level
{
    public class UpgradeUI : MonoBehaviour
    {


        public ScrollerPlus scrollplus;
        static public string t_selectedTankName = "blue_tank";
        public GameObject[] BTNs;
        public GameObject upgrade_panel;
        public Text nameoftanks;
        public Text cost;
        public Text health;
        public Image Level;
        public Sprite[] lvls;

        int selected = 0;
        int currentlvl = 0;
        int takeindex;


        public GameObject[] Prev_Next;


        upgradesystem upg;

        static public UpgradeUI instance;

        public GameObject upgrader;
        public Slider upgSTAT;
        public Text upgcost;
        int missilelvl = 0;

        public Slider[] bulletSlider;
        public Slider selectedSlider;
        public Text[] costtexts;

        public Image Rank;

        public GameObject[] particleSys;
        private void Start()
        {

            if (Application.loadedLevelName == "0_MainMenu")
            {
                scrollplus.onChange.AddListener(() =>
                {
                    t_selectedTankName = scrollplus.currentItem.gameObject.name;
                });
                upgSTAT = upgrader.transform.Find("upgraderStat").GetComponent<Slider>();
            }
            else
            {


            }
           
            if (PlayerPrefs.HasKey("FirstTouch"))
            {

                // Not First Time

            }
            else
            {

                PlayerPrefs.SetInt("tank1", 0);
                PlayerPrefs.SetInt("tank2", 0);
                PlayerPrefs.SetInt("tank3", 0);
                PlayerPrefs.SetInt("tank4", 0);
                PlayerPrefs.SetString("FirstTouch", "No");
                PlayerPrefs.Save();
                
            }
              
            if (Application.loadedLevelName == "0_MainMenu")
            {
                if (t_selectedTankName == "tank_yellow")
                {
                    nameoftanks.text = "" + upg.tanks[0].NameOfTank;
                    cost.text = upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].cost.ToString();
                }
                else if (t_selectedTankName == "tank_green1")
                {
                    nameoftanks.text = "" + upg.tanks[1].NameOfTank;
                    cost.text = upg.tanks[1].Items[PlayerPrefs.GetInt("tank2")].cost.ToString();

                }
                else if (t_selectedTankName == "tank_green2")
                {
                    nameoftanks.text = "" + upg.tanks[2].NameOfTank;
                    cost.text = upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].cost.ToString();

                }
                else if (t_selectedTankName == "tank_blue")
                {
                    nameoftanks.text = "" + upg.tanks[3].NameOfTank;
                    cost.text = upg.tanks[3].Items[PlayerPrefs.GetInt("tank4")].cost.ToString();

                }
            }
            if (instance == null)
            {
                instance = this;
            }

            /*   for(int i = 0; i <= 3; i++)
               {
                   for(int j = 0;j < 4;j++)
                   { 
                       selectedSlider = bulletSlider[j];
                       selectedSlider.value = PlayerPrefs.GetInt("tank" + i + "missile" + j);
                   }
               }
               */
            RankInfo();
        }
        private void Awake()
        {
            upg = GetComponent<upgradesystem>();
            //  TankInfo();

            // DeletePlayerPrefs();
            if (Application.loadedLevelName== "0_MainMenu")
            {
                DontDestroyOnLoad(this);
            }
        }

        void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteKey("tank1");
            PlayerPrefs.DeleteKey("tank2");
            PlayerPrefs.DeleteKey("tank3");
            PlayerPrefs.DeleteKey("tank4");
            PlayerPrefs.DeleteKey("tank1missile1");
            PlayerPrefs.DeleteKey("tank1missile2");
            PlayerPrefs.DeleteKey("tank1missile3");
            PlayerPrefs.DeleteKey("tank2missile1");
            PlayerPrefs.DeleteKey("tank2missile2");
            PlayerPrefs.DeleteKey("tank2missile3");
            PlayerPrefs.DeleteKey("tank3missile1");
            PlayerPrefs.DeleteKey("tank3missile2");
            PlayerPrefs.DeleteKey("tank3missile3");
            PlayerPrefs.DeleteKey("tank4missile1");
            PlayerPrefs.DeleteKey("tank4missile2");
            PlayerPrefs.DeleteKey("tank4missile3");
            PlayerPrefs.SetInt("tank1", 0);
            PlayerPrefs.SetInt("tank2", 0);
            PlayerPrefs.SetInt("tank3", 0);
            PlayerPrefs.SetInt("tank4", 0);
            PlayerPrefs.Save();
        }

        public void UpgradeTank()
        {
            #region scrollplus
            scrollplus.onChange.AddListener(() =>
            {
                t_selectedTankName = scrollplus.currentItem.gameObject.name;
            });
            #endregion

           // MissileInfo();
            if (t_selectedTankName == "tank_yellow")
            {
                selected = 1;
                int currlvl = PlayerPrefs.GetInt("tank1");
                if (PlayerPrefs.GetInt("tank1") <= upg.tanks[0].Items.Length-1)
                {
                    print("lenghtesh : " + upg.tanks[0].Items.Length);
                    if (ShopManager.Gold >= upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].cost)
                    {
                        ShopManager.Gold -= upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].cost;
                        ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();
                        if (PlayerPrefs.GetInt("tank1") <= 2)
                        {
                            currlvl++;
                            PlayerPrefs.SetInt("tank1", currlvl);
                            PlayerPrefs.Save();
                         
                            upgSTAT.value = upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].level;
                            upgcost.text = "" + upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].cost;
                            particleSys[0].SetActive(true);
                            if (PlayerPrefs.GetInt("tank1") == 2)
                            {
                                StartCoroutine(UpgraderDeActivator());
                            }
                            
                        }
                        else if (PlayerPrefs.GetInt("tank2") > 2)
                        {
                            PlayerPrefs.SetInt("tank1", 2);
                            upgSTAT.value = 2;
                        }
                    }
                    else
                    {
                        ShopManager.instance.OpenGoldShop();
                    }
                }

            }
            else if (t_selectedTankName == "tank_green1")
            {
                selected = 2;
                int currlvl = PlayerPrefs.GetInt("tank2");
                if (PlayerPrefs.GetInt("tank2") <= upg.tanks[1].Items.Length-1)
                {
                    if (ShopManager.Gold >= upg.tanks[1].Items[PlayerPrefs.GetInt("tank2")].cost)
                    {
                        ShopManager.Gold -= upg.tanks[1].Items[PlayerPrefs.GetInt("tank2")].cost;
                        ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                        if (PlayerPrefs.GetInt("tank2") <= 2)
                        {

                            currlvl++;
                            PlayerPrefs.SetInt("tank2", currlvl);
                            PlayerPrefs.Save();
                           
                            upgSTAT.value = upg.tanks[1].Items[PlayerPrefs.GetInt("tank2")].level;
                            upgcost.text = "" + upg.tanks[1].Items[PlayerPrefs.GetInt("tank2")].cost;
                            particleSys[1].SetActive(true);
                            if (PlayerPrefs.GetInt("tank2") == 2)
                            {
                                StartCoroutine(UpgraderDeActivator());
                            }
                        }
                        else if (PlayerPrefs.GetInt("tank2") > 2)
                        {
                            PlayerPrefs.SetInt("tank2", 2);
                            upgSTAT.value = 2;

                        }
                    }
                }
            }
            else if (t_selectedTankName == "tank_green2")
            {
                selected = 3;
                int currlvl = PlayerPrefs.GetInt("tank3");
                if (PlayerPrefs.GetInt("tank3") <= upg.tanks[2].Items.Length-1)
                {
                    if (ShopManager.Gold >= upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].cost)
                    {
                        ShopManager.Gold -= upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].cost;
                        ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                        if (PlayerPrefs.GetInt("tank3") <= 2)
                        {
                            currlvl++;
                            PlayerPrefs.SetInt("tank3", currlvl);
                            PlayerPrefs.Save();
                            particleSys[2].SetActive(true);
                           
                            upgSTAT.value = upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].level;
                            upgcost.text = "" + upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].cost;
                            if (PlayerPrefs.GetInt("tank3") == 2)
                            {
                                StartCoroutine(UpgraderDeActivator());
                            }

                        }
                        else if (PlayerPrefs.GetInt("tank2") > 2)
                        {
                            PlayerPrefs.SetInt("tank3", 2);
                            upgSTAT.value = 2;

                        }
                    }
                }
            }
            else if (t_selectedTankName == "tank_blue")
            {
                selected = 3;
                int currlvl = PlayerPrefs.GetInt("tank4");
                if (PlayerPrefs.GetInt("tank4") <= upg.tanks[3].Items.Length-1)
                {
                    if (ShopManager.Gold >= upg.tanks[3].Items[PlayerPrefs.GetInt("tank4")].cost)
                    {
                        ShopManager.Gold -= upg.tanks[3].Items[PlayerPrefs.GetInt("tank4")].cost;
                        ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                        if (PlayerPrefs.GetInt("tank4") <= 2)
                        {
                            currlvl++;
                            PlayerPrefs.SetInt("tank4", currlvl);
                            PlayerPrefs.Save();
                          
                          
                                upgSTAT.value = upg.tanks[3].Items[PlayerPrefs.GetInt("tank4")].level;
                            upgcost.text = "" + upg.tanks[3].Items[PlayerPrefs.GetInt("tank4")].cost;
                            particleSys[3].SetActive(true);
                            if (PlayerPrefs.GetInt("tank4") == 2)
                            {
                                StartCoroutine(UpgraderDeActivator());
                            }
                        }
                        else
                        {
                            PlayerPrefs.SetInt("tank4", 2);
                            upgSTAT.value = 2;
                        }
                    }
                }
            }
            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
            PlayerPrefs.Save();
            // statUI.instance.TankInfo();
           // ShowUpgradeUI();
            RankInfo();

        }

        IEnumerator UpgraderDeActivator()
        {
            yield return new WaitForSeconds(1);
            upgrader.SetActive(false);

        }
        public GameObject lvl_panel;

        public void ShowUpgradeUI()
        {
            scrollplus.onChange.AddListener(() =>
            {
                t_selectedTankName = scrollplus.currentItem.gameObject.name;
            });

            GetComponent<AudioSource>().Play();

            /*  for (int i = 0;i < BTNs.Length; i++)
              {
                  BTNs[i].transform.gameObject.SetActive(false);
              }
              */
            if (t_selectedTankName == "tank_yellow" && PlayerPrefs.GetString("TANK_1_Lock") == "Unlocked")
            {
                ScrollerPlus.CanScroll = false;
                Prev_Next[0].SetActive(false);
                Prev_Next[1].SetActive(false);
                upgrade_panel.SetActive(true);
                lvl_panel.GetComponent<Animator>().SetBool("GO_UP", false);
                upgrade_panel.GetComponent<Animator>().SetBool("GO_UP", true);
                nameoftanks.text = "" + upg.tanks[0].NameOfTank;
                if (PlayerPrefs.GetInt("tank1") == 2)
                {
                    upgrader.SetActive(false);
                }
                else
                {
                    upgrader.SetActive(true);
                    upgSTAT.value = upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].level;
                    upgcost.text = "" + upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].cost;
                    print("levele tanke 1 : " + PlayerPrefs.GetInt("tank1") + "gheymatesh : " + PlayerPrefs.GetInt("tank1"));
                }
               
            }
            else if (t_selectedTankName == "tank_green1" && PlayerPrefs.GetString("TANK_2_Lock") == "Unlocked")
            {
                ScrollerPlus.CanScroll = false;
                Prev_Next[0].SetActive(false);
                Prev_Next[1].SetActive(false);
                upgrade_panel.SetActive(true);
                lvl_panel.GetComponent<Animator>().SetBool("GO_UP", false);
                upgrade_panel.GetComponent<Animator>().SetBool("GO_UP", true);
                nameoftanks.text = "" + upg.tanks[1].NameOfTank;

                if (PlayerPrefs.GetInt("tank2") == 2)
                {
                    upgrader.SetActive(false);
                }
                else
                {
                    upgrader.SetActive(true);
                    upgSTAT.value = upg.tanks[0].Items[PlayerPrefs.GetInt("tank2")].level;
                    upgcost.text = "" + upg.tanks[1].Items[PlayerPrefs.GetInt("tank2")].cost;
                }
                
            }
            else if (t_selectedTankName == "tank_green2" && PlayerPrefs.GetString("TANK_3_Lock") == "Unlocked")
            {
                ScrollerPlus.CanScroll = false;
                Prev_Next[0].SetActive(false);
                Prev_Next[1].SetActive(false);
                upgrade_panel.SetActive(true);
                lvl_panel.GetComponent<Animator>().SetBool("GO_UP", false);
                upgrade_panel.GetComponent<Animator>().SetBool("GO_UP", true);
                nameoftanks.text = "" + upg.tanks[2].NameOfTank;

                if (PlayerPrefs.GetInt("tank3") == 2)
                {
                    upgrader.SetActive(false);
                }else
                {
                    upgrader.SetActive(true);
                    upgSTAT.value = upg.tanks[0].Items[PlayerPrefs.GetInt("tank3")].level;
                    upgcost.text = "" + upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].cost;
                }
               
            }
            else if (t_selectedTankName == "tank_blue" && PlayerPrefs.GetString("TANK_4_Lock") == "Unlocked")
            {
                print(PlayerPrefs.GetString("TANK_4_Lock") + " vaziate tanke 4");
                ScrollerPlus.CanScroll = false;
                Prev_Next[0].SetActive(false);
                Prev_Next[1].SetActive(false);
                upgrade_panel.SetActive(true);
                lvl_panel.GetComponent<Animator>().SetBool("GO_UP", false);
                upgrade_panel.GetComponent<Animator>().SetBool("GO_UP", true);
                nameoftanks.text = "" + upg.tanks[3].NameOfTank;

                if (PlayerPrefs.GetInt("tank4") == 2)
                {
                    upgrader.SetActive(false);
                }else
                {
                    upgrader.SetActive(true);
                    upgSTAT.value = upg.tanks[0].Items[PlayerPrefs.GetInt("tank4")].level;
                    upgcost.text = "" + upg.tanks[3].Items[PlayerPrefs.GetInt("tank4")].cost;

                }
                
            }
       

            //  Info();
            MissileInfo();
        }

        public void HideUpgradeUI()
        {
          //  lvl_panel.GetComponent<Animator>().SetBool("GO_UP", true);
          //  upgrade_panel.GetComponent<Animator>().SetBool("GO_UP", false);
        }
        public void BulletsUpgrade(int index)
        {
            GetComponent<AudioSource>().Play();

            if (t_selectedTankName == "tank_yellow")
            {
                missilelvl = PlayerPrefs.GetInt("tank1missile" + index);
                if (PlayerPrefs.GetInt("tank1missile" + index) <= upg.tanks[0].missilelvl.Length)
                {

                    missilelvl++;
                    PlayerPrefs.SetInt("tank1missile" + index, missilelvl);
                    PlayerPrefs.Save();
                    selectedSlider = bulletSlider[index - 1];
                    selectedSlider.value = missilelvl;

                    switch (index)
                    {
                        case 1:
                            ShopManager.Gold -= 50;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;
                        case 2:
                            ShopManager.Gold -= 100;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;
                        case 3:
                            ShopManager.Gold -= 130;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;

                    }
                    
                }
                
            }
            else if (t_selectedTankName == "tank_green1")
            {
                missilelvl = PlayerPrefs.GetInt("tank2missile" + index);
                if (PlayerPrefs.GetInt("tank2missile" + index) <= upg.tanks[1].missilelvl.Length)
                {
                    missilelvl++;
                    PlayerPrefs.SetInt("tank2missile" + index, missilelvl);
                    PlayerPrefs.Save();
                    selectedSlider = bulletSlider[index -1];
                    selectedSlider.value = missilelvl;

                    switch (index)
                    {
                        case 1:
                            ShopManager.Gold -= 150;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;
                        case 2:
                            ShopManager.Gold -= 180;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;
                        case 3:
                            ShopManager.Gold -= 200;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;

                    }
                    
                }
            }
            else if (t_selectedTankName == "tank_green2")
            {
                missilelvl = PlayerPrefs.GetInt("tank3missile" + index);
                if (PlayerPrefs.GetInt("tank3missile" + index) <= upg.tanks[2].missilelvl.Length)
                {
                    missilelvl++;
                    PlayerPrefs.SetInt("tank3missile" + index, missilelvl);
                    PlayerPrefs.Save();
                    selectedSlider = bulletSlider[index - 1];
                    selectedSlider.value = missilelvl;

                    switch (index)
                    {
                        case 1:
                            ShopManager.Gold -= 210;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;
                        case 2:
                            ShopManager.Gold -= 240;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;
                        case 3:
                            ShopManager.Gold -= 280;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();

                            break;

                    }
                    
                }
            }
            else if (t_selectedTankName == "tank_blue")
            {
                missilelvl = PlayerPrefs.GetInt("tank4missile" + index);
                if (PlayerPrefs.GetInt("tank4missile" + index) <= upg.tanks[3].missilelvl.Length)
                {
                    missilelvl++;
                    PlayerPrefs.SetInt("tank4missile" + index, missilelvl);
                    PlayerPrefs.Save();
                    selectedSlider = bulletSlider[index - 1];
                    selectedSlider.value = missilelvl;

                    switch (index)
                    {
                        case 1:
                            ShopManager.Gold -= 300;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();
                            break;
                        case 2:
                            ShopManager.Gold -= 340;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();
                            break;
                        case 3:
                            ShopManager.Gold -= 400;
                            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                            PlayerPrefs.Save();
                            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();
                            break;

                    }
                    
                }
            }
            takeindex = index;
           // MissileUPGInfo();
        }
        private void Info()
        {

            scrollplus.onChange.AddListener(() =>
            {
                t_selectedTankName = scrollplus.currentItem.gameObject.name;
            });

            if (t_selectedTankName == "tank_yellow")
            {
                for (int i = 0; i < 3; i++)
                {
                    missilelvl = PlayerPrefs.GetInt("tank1missile" + i);

                    selectedSlider = bulletSlider[i];
                    selectedSlider.value = missilelvl - 1;
                }
            }

            else if (t_selectedTankName == "tank_green1")
            {
                for (int i = 0; i < 3; i++)
                {
                    missilelvl = PlayerPrefs.GetInt("tank2missile" + i);

                    selectedSlider = bulletSlider[i];
                    selectedSlider.value = missilelvl ;
                }
            }
            else if (t_selectedTankName == "tank_green2")
            {
                for (int i = 0; i < 3; i++)
                {
                    missilelvl = PlayerPrefs.GetInt("tank3missile" + i);

                    selectedSlider = bulletSlider[i];
                    selectedSlider.value = missilelvl - 1;
                }
            }
            else if (t_selectedTankName == "tank_blue")
            {
                for (int i = 0; i < 3; i++)
                {
                    missilelvl = PlayerPrefs.GetInt("tank4missile" + i);

                    selectedSlider = bulletSlider[i ];
                    selectedSlider.value = missilelvl - 1;
                }
            }
            // print("level " + PlayerPrefs.GetInt("tank4missile1"));
        }
       void MissileInfo()
        {
          
            scrollplus.onChange.AddListener(() =>
            {
                t_selectedTankName = scrollplus.currentItem.gameObject.name;
            });

            if (t_selectedTankName == "tank_yellow")
            {
                for (int i = 0; i < 3; i++)
                {

                    missilelvl = PlayerPrefs.GetInt("tank1missile" + i);
                    selectedSlider = bulletSlider[i];
                    selectedSlider.value = missilelvl - 1;
                    switch (i)
                    {
                        case 0:
                            costtexts[0].text = "" + 50;
                            break;
                        case 1:
                            costtexts[1].text = "" + 100;
                            break;
                        case 2:
                            costtexts[2].text = "" + 130;
                            break; 
                    }
                }
            }

            else if (t_selectedTankName == "tank_green1")
            {
                for (int i = 0; i < 3; i++)
                {

                    missilelvl = PlayerPrefs.GetInt("tank2missile" + i);

                    selectedSlider = bulletSlider[i];
                    selectedSlider.value = missilelvl;

                    switch (i)
                    {
                        case 0:
                            costtexts[0].text = "" + 150;
                            break;
                        case 1:
                            costtexts[1].text = "" + 180;
                            break;
                        case 2:
                            costtexts[2].text = "" + 200;
                            break;
                    }
                }
            }
            if (t_selectedTankName == "tank_green2")
            {
                for (int i = 0; i < 3; i++)
                {

                    missilelvl = PlayerPrefs.GetInt("tank3missile" + i);

                    selectedSlider = bulletSlider[i];
                    selectedSlider.value = missilelvl - 1;
                    switch (i)
                    {
                        case 0:
                            costtexts[0].text = "" + 210;
                            break;
                        case 1:
                            costtexts[1].text = "" + 240;
                            break;
                        case 2:
                            costtexts[2].text = "" + 280;
                            break;
                    }
                }
            }
            if (t_selectedTankName == "tank_blue")
            {
                for (int i = 0; i < 3; i++)
                {
                    missilelvl = PlayerPrefs.GetInt("tank4missile" + i);

                    selectedSlider = bulletSlider[i];
                    selectedSlider.value = missilelvl - 1;
                    switch (i)
                    {
                        case 0:
                            costtexts[0].text = "" + 300;
                            break;
                        case 1:
                            costtexts[1].text = "" + 340;
                            break;
                        case 2:
                            costtexts[2].text = "" + 400;
                            break;
                    }
                }
            }
           // print("level " + PlayerPrefs.GetInt("tank4missile1"));
        }
    
        void RankInfo()
        {

            if (Application.loadedLevelName == "1_Level_AI_1")
            {
                if (PlayerPrefs.GetString("tankNAME") == "tank_yellow")
                {
                    currentlvl = PlayerPrefs.GetInt("tank1");
                    Rank.sprite = lvls[currentlvl];
                    print("levelesh" + currentlvl);
                    print("esme tank : " + PlayerPrefs.GetString("tankNAME"));
                }
                else if (PlayerPrefs.GetString("tankNAME") == "tank_green1")
                {
                    currentlvl = PlayerPrefs.GetInt("tank2");
                    Rank.sprite = lvls[currentlvl];
                }
                else if (PlayerPrefs.GetString("tankNAME") == "tank_green2")
                {
                    currentlvl = PlayerPrefs.GetInt("tank3");
                    Rank.sprite = lvls[currentlvl];
                }
                else if (PlayerPrefs.GetString("tankNAME") == "tank_blue")
                {
                    currentlvl = PlayerPrefs.GetInt("tank4");
                    Rank.sprite = lvls[currentlvl];
                }
            }
        }
    }
}


