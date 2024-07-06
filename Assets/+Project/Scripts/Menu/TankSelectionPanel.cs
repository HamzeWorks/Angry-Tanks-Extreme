using Amulay.Utility;
using TankStars.Level;
using UnityEngine;
using UnityEngine.UI;


namespace TankStars.Menu
{
    public class TankSelectionPanel : Singleton<TankSelectionPanel>
    {
        [SerializeField] private ScrollerPlus scroller;
        private string t_selectedTankName = "blue_tank";

        // Tanks unLocker
        public GameObject[] TanksBodies;
        public GameObject[] UnlockBTNs;

        // Gem & Gold Text
        public Text gemtxt, goldtxt;
        public GameObject upgrader;
        protected override void Awake()
        {
            base.Awake();
            if (instance != this)
                return;

            GameManager.instance.SelectTank(Tank.GetPrefab(t_selectedTankName));



            if (PlayerPrefs.HasKey("FirstRun"))
            {
                /* for (int i = 1; i <= 4; i++)
                 {
                     for (int j = 1; j <= 3; j++)
                     {
                         // PlayerPrefs.SetInt("tank" + i + "missile", j);
                         print("Tanke " + i + "mooshake " + j);
                     }
                 }
              
             */

              
            }
            else
            {
                
                PlayerPrefs.SetString("TANK_1_Lock", "Unlocked");
                PlayerPrefs.SetString("TANK_2_Lock", "Locked");
                PlayerPrefs.SetString("TANK_3_Lock", "Locked");
                PlayerPrefs.SetString("TANK_4_Lock", "Locked");
                PlayerPrefs.Save();
                // Missiles PlayerPrefs
              /* 
                 for(int i = 0; i <= 4; i++)
                {
                   for( int j = 0 ; j<4;j++)
                    {
                        // PlayerPrefs.SetInt("tank" + i + "missile", j);
                        print("Tanke " + i + "mooshake " + j);
                    }
                }
                */
                //
                PlayerPrefs.SetString("FirstRun", "Done");
                PlayerPrefs.Save();
                print("First Run");
            }

            // Checking For Tanks Lock Status

            // Tank 2
            if (PlayerPrefs.GetString("TANK_2_Lock") == "Locked")
            {
                UnlockBTNs[1].SetActive(true);
                TanksBodies[1].GetComponent<PolygonCollider2D>().enabled = false;
                print(TanksBodies[1].transform.parent.name + " is locked !");
            }
            else if (PlayerPrefs.GetString("TANK_2_Lock") == "Unlocked")
            {
                UnlockBTNs[1].SetActive(false);
                TanksBodies[1].GetComponent<PolygonCollider2D>().enabled = true;
            }
            // Tank 3
            if (PlayerPrefs.GetString("TANK_3_Lock") == "Locked")
            {
                UnlockBTNs[2].SetActive(true);
                TanksBodies[2].GetComponent<BoxCollider2D>().enabled = false;
                print(TanksBodies[2].transform.parent.name + " is locked !");
            }
            else if (PlayerPrefs.GetString("TANK_3_Lock") == "Unlocked")
            {
                UnlockBTNs[2].SetActive(false);
                TanksBodies[2].GetComponent<BoxCollider2D>().enabled = true;
            }
            // Tank 4
            if (PlayerPrefs.GetString("TANK_4_Lock") == "Locked")
            {
                UnlockBTNs[3].SetActive(true);
                TanksBodies[3].GetComponent<PolygonCollider2D>().enabled = false;
                print(TanksBodies[3].transform.parent.name + " is locked !");
            }
            else if (PlayerPrefs.GetString("TANK_4_Lock") == "Unlocked")
            {
                UnlockBTNs[3].SetActive(false);
                TanksBodies[3].GetComponent<PolygonCollider2D>().enabled = true;
            }
            /*
            for (int i = 0; i <= 4; i++)
            {
                if (PlayerPrefs.GetString("TANK_" + i + "_Lock") == "Locked")
                {
                    UnlockBTNs[i-1].SetActive(true);
                    //TanksBodies[i - 1].GetComponent<Animator>().enabled = false;
                    print(TanksBodies[i-1].transform.parent.name + " is locked !");
                }else if(PlayerPrefs.GetString("TANK_" + i + "_Lock") == "Unlocked")
                {

                    //
                    //  PlayerPrefs.SetString("TANK_" + i + "_Lock", "Locked");
                    //  PlayerPrefs.Save();
                    //

                    UnlockBTNs[i-1].SetActive(false);
                    TanksBodies[i - 1].GetComponent<Animator>().enabled = true;
                    TanksBodies[i-1].SetActive(true);
                    print(TanksBodies[i-1].transform.parent.name + " is unlocked !");

                }
            }
            */
            scroller.onChange.AddListener(() =>
            {
                if (PlayerPrefs.GetString("TANK_1_Lock") == "Unlocked")
                {
                    t_selectedTankName = scroller.currentItem.gameObject.name;
                    GameManager.instance.SelectTank(Tank.GetPrefab(t_selectedTankName));

                    PlayerPrefs.SetString("tankNAME", t_selectedTankName);
                    PlayerPrefs.Save();
                }
                else if (PlayerPrefs.GetString("TANK_2_Lock") == "Unlocked")
                {
                    t_selectedTankName = scroller.currentItem.gameObject.name;
                    GameManager.instance.SelectTank(Tank.GetPrefab(t_selectedTankName));

                    PlayerPrefs.SetString("tankNAME", t_selectedTankName);
                    PlayerPrefs.Save();
                }else if(PlayerPrefs.GetString("TANK_3_Lock") == "Unlocked")
                {
                    t_selectedTankName = scroller.currentItem.gameObject.name;
                    GameManager.instance.SelectTank(Tank.GetPrefab(t_selectedTankName));

                    PlayerPrefs.SetString("tankNAME", t_selectedTankName);
                    PlayerPrefs.Save();
                }
                else if (PlayerPrefs.GetString("TANK_4_Lock") == "Unlocked")
                {
                    t_selectedTankName = scroller.currentItem.gameObject.name;
                    GameManager.instance.SelectTank(Tank.GetPrefab(t_selectedTankName));

                    PlayerPrefs.SetString("tankNAME", t_selectedTankName);
                    PlayerPrefs.Save();
                }
            }); 
        }
        public void Unlock(int index)
        {
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().Play();
            }
            switch (index)
            {
                case 2:
                    if(ShopManager.Gold >= 1000 && PlayerPrefs.GetString("TANK_2_Lock") == "Locked")
                    {
                        PlayerPrefs.SetString("TANK_2_Lock", "Unlocked");
                        PlayerPrefs.Save();

                        UnlockBTNs[1].SetActive(false);                                          // DeActive Unlock Button
                        upgrader.SetActive(true);
                        TanksBodies[1].GetComponent<PolygonCollider2D>().enabled = true;
                        ShopManager.Gold -= 1000;
                        goldtxt.text = ShopManager.Gold.ToString();
                        PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                        PlayerPrefs.Save();
                        print("1000 ta gold bekhatere tanke " + index + "pardakht shod !");
                    }
                    break;
                case 3:
                    if (ShopManager.Gold >= 2500 && PlayerPrefs.GetString("TANK_3_Lock") == "Locked")
                    {
                        PlayerPrefs.SetString("TANK_3_Lock", "Unlocked");
                        PlayerPrefs.Save();

                        UnlockBTNs[2].SetActive(false);                                          // DeActive Unlock Button
                        upgrader.SetActive(true);
                        TanksBodies[2].GetComponent<BoxCollider2D>().enabled = true;
                        ShopManager.Gold -= 2500;
                        goldtxt.text = ShopManager.Gold.ToString();
                        PlayerPrefs.SetInt("Gold", ShopManager.Gold);
                        PlayerPrefs.Save();
                        print("2500 ta gold bekhatere tanke " + index + "pardakht shod !");
                    }
                    break;
                case 4:
                    if (ShopManager.Gem >= 100 && PlayerPrefs.GetString("TANK_4_Lock") == "Locked")
                    {
                        PlayerPrefs.SetString("TANK_4_Lock", "Unlocked");
                        PlayerPrefs.Save();

                        UnlockBTNs[3].SetActive(false);                                           // DeActive Unlock Button
                        upgrader.SetActive(true);
                        TanksBodies[3].GetComponent<PolygonCollider2D>().enabled = true;
                        ShopManager.Gem -= 100;
                        gemtxt.text = ShopManager.Gem.ToString();
                        PlayerPrefs.SetInt("Gem", ShopManager.Gem);
                        PlayerPrefs.Save();

                        print("10 ta gem bekhatere tanke " + index + "pardakht shod !");
                    }
                    break;

            }
        }
    }
}