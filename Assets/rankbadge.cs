using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amulay.Utility;
using UnityEngine.UI;

namespace TankStars.Level
{
    public class rankbadge : MonoBehaviour
    {

        public Sprite[] ranks;
        upgradesystem upg;
        public GameObject upgkeeper;
        public Slider upgradeSTATS;
        void Awake()
        {
            if (name == "PlayerRank")
            {
                upg = upgkeeper.GetComponent<upgradesystem>();

                if (UpgradeUI.t_selectedTankName == "tank_yellow")
                {

                    switch (PlayerPrefs.GetInt("tank1"))
                    {
                        case 1:
                            GetComponent<Image>().sprite = ranks[0];
                            //upgradeSTATS.value = 0;
                            break;
                        case 2:
                            GetComponent<Image>().sprite = ranks[1];
                            //upgradeSTATS.value = 1;

                            break;
                        case 3:
                            GetComponent<Image>().sprite = ranks[2];
                            //upgradeSTATS.value = 2;
                            print(PlayerPrefs.GetInt("Tank1") + " levelesh ine !!!!!");

                            break;
                    }
                }
                else if (UpgradeUI.t_selectedTankName == "tank_green1")
                {

                    switch (PlayerPrefs.GetInt("tank2"))
                    {
                        case 1:
                            GetComponent<Image>().sprite = ranks[0];
                            //upgradeSTATS.value = 0;
                            break;
                        case 2:
                            GetComponent<Image>().sprite = ranks[1];
                            //upgradeSTATS.value = 1;

                            break;
                        case 3:
                            GetComponent<Image>().sprite = ranks[2];
                            //upgradeSTATS.value = 2;

                            break;
                    }
                }
                else if (UpgradeUI.t_selectedTankName == "tank_green2")
                {

                    switch (PlayerPrefs.GetInt("tank3"))
                    {
                        case 1:
                            GetComponent<Image>().sprite = ranks[0];
                            //upgradeSTATS.value = 0;

                            break;
                        case 2:
                            GetComponent<Image>().sprite = ranks[1];
                            //upgradeSTATS.value = 1;

                            break;
                        case 3:
                            GetComponent<Image>().sprite = ranks[2];
                            //upgradeSTATS.value = 2;

                            break;
                    }
                }
                else if (UpgradeUI.t_selectedTankName == "tank_blue")
                {

                    switch (PlayerPrefs.GetInt("tank4"))
                    {
                        case 1:
                            GetComponent<Image>().sprite = ranks[0];
                            //upgradeSTATS.value = 0;

                            break;
                        case 2:
                            GetComponent<Image>().sprite = ranks[1];
                            //upgradeSTATS.value = 1;

                            break;
                        case 3:
                            GetComponent<Image>().sprite = ranks[2];
                            //upgradeSTATS.value = 2;

                            break;
                    }
                }

            }
            else if (name == "lvl1")
            {
                switch (PlayerPrefs.GetInt("tank1"))
                {
                    case 1:
                        GetComponent<Image>().sprite = ranks[0];
                        break;
                    case 2:
                        GetComponent<Image>().sprite = ranks[1];
                        break;
                    case 3:
                        GetComponent<Image>().sprite = ranks[2];
                        break;
                }

            }
            else if (name == "lvl2")
            {
                switch (PlayerPrefs.GetInt("tank2"))
                {
                    case 1:
                        GetComponent<Image>().sprite = ranks[0];
                        break;
                    case 2:
                        GetComponent<Image>().sprite = ranks[1];
                        break;
                    case 3:
                        GetComponent<Image>().sprite = ranks[2];
                        break;
                }

            }
            else if (name == "lvl3")
            {
                switch (PlayerPrefs.GetInt("tank3"))
                {
                    case 1:
                        GetComponent<Image>().sprite = ranks[0];
                        break;
                    case 2:
                        GetComponent<Image>().sprite = ranks[1];
                        break;
                    case 3:
                        GetComponent<Image>().sprite = ranks[2];
                        break;
                }

            }
            else if (name == "lvl4")
            {

                switch (PlayerPrefs.GetInt("tank4"))
                {
                    case 1:
                        GetComponent<Image>().sprite = ranks[0];
                        break;
                    case 2:
                        GetComponent<Image>().sprite = ranks[1];
                        break;
                    case 3:
                        GetComponent<Image>().sprite = ranks[2];
                        break;
                }

            }else if (name=="AIRank")
            {
                GetComponent<Image>().sprite = ranks[Random.Range(0, ranks.Length)];
            }
        }
    }
}