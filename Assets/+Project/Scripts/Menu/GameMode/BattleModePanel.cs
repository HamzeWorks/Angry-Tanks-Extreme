using Amulay.Utility;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Amulay.Utility;
using TankStars.Level;

namespace TankStars.Menu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BattleModePanel : Singleton<BattleModePanel>, IState
    {
        private const float offset = 300f;
        [SerializeField]
        private Button pvpBattleButton;
        [SerializeField]
        private Button privateRoomButton;
        [SerializeField]
        private Button stageButton;
        [SerializeField]
        private Image touchcanceler;

        [SerializeField]
        private Button backButton;
        [SerializeField]
        private Button upgButton;


        private CanvasGroup canvasGroup;
        private Vector3 showPostion;
        private Vector3 hidePostion;
        private bool cachedPosition = false;
        private Transform panel;

        public GameObject GO, aboutus, Upgrade_Panel, lvl_Panel, Modes_Panel;
        public GameObject play, upgrade;
        public GameObject[] Arrows;

        string whereUX = "Modes";

        public GameObject bullets;

        public GameObject[] HPbar;


        public ScrollerPlus scrollplus;
        public string t_selectedTank;

        public Sprite[] unlockBW_playBW;
        public Sprite[] unlockColor_playColor;


        protected override void Awake()
        {
            base.Awake();
            panel = transform.GetChild(0);
            canvasGroup = GetComponent<CanvasGroup>();

            InitButtons();
            for (int i = 0; i < Arrows.Length; i++)
            {
                Arrows[i].SetActive(false);
            }
            Modes_Panel.transform.DOLocalMoveY(0f, 0.8f).SetDelay(0.5f).SetEase(Ease.OutQuart);
            // lvl_Panel.transform.DOLocalMoveY(-800f, 0.8f).SetDelay(0.5f).SetEase(Ease.OutQuart);
            whereUX = "Modes";
            bullets.SetActive(false);


            scrollplus.onChange.AddListener(() =>
            {
                t_selectedTank = scrollplus.currentItem.gameObject.name;
            });
        }


        private void InitButtons()
        {
            privateRoomButton.onClick.AddListener(() =>
            {
                GameModePanel.instance.ChangeState(PrivateRoomPanel.instance);
            });
            stageButton.onClick.AddListener(() =>
            {

            });
            pvpBattleButton.onClick.AddListener(async () =>
            {
                var success = await GameManager.instance.SetGameMode(SceneEnum.Level_Online_2P);
                if (success == false)
                    pvpBattleButton.interactable = true;

            });
            upgButton.onClick.AddListener(() =>
            {


            });
            backButton.onClick.AddListener(() =>
            {


            });
        }
        public void OnEnter(IState previousState, Action<IState> onCompelete = null)
        {
            /*     if(cachedPosition == false)
                 {
                     showPostion = panel.localPosition;
                     hidePostion = showPostion + Vector3.down * offset;
                     panel.localPosition = hidePostion;
                     cachedPosition = true;
                 }
                 panel.gameObject.SetActive(true);
                 gameObject.SetActive(true);
                 panel.DOKill();
                 panel.DOLocalMoveY(showPostion.y,1).SetEase(Ease.OutBack).onComplete += () =>
                 {
                     for (int i = 0; i < panel.childCount; i++)
                         panel.GetChild(i).DOPunchScale(Vector3.one * .2f, .3f, 3).SetDelay(i * .2f);
                 };*/
        }

        public void OnExit(IState nextState)
        {
            //  panel.DOKill();
            //  panel.DOLocalMoveY(hidePostion.y, .5f).SetEase(Ease.OutQuad);
            //aboutus.SetActive(true);

        }
        public void HideAboutUs_Btn()
        {
            aboutus.SetActive(false);
            //  Upgrade_Panel.SetActive(true);
            GetComponent<AudioSource>().Play();

        }
        public void Back()
        {
            GetComponent<AudioSource>().Play();

            if (whereUX == "UPG")
            {
                whereUX = "LVLS";
                for (int i = 0; i < Arrows.Length; i++)
                {
                    Arrows[i].SetActive(true);
                }
                bullets.SetActive(false);

                // Modes_Panel.transform.DOLocalMoveY(-1000f, 0.8f).SetEase(Ease.OutQuart);
                //  lvl_Panel.SetActive(true);
                lvl_Panel.transform.DOLocalMoveY(-660, 0.5f).SetEase(Ease.Linear);
                Upgrade_Panel.transform.DOLocalMoveY(-2000, .5f).SetEase(Ease.Linear);
                print(whereUX);
                for (int i = 0; i <= 3; i++)
                {
                    HPbar[i].SetActive(true);
                }
            }
            else if (whereUX == "LVLS")
            {
                whereUX = "Modes";
                Modes_Panel.transform.DOLocalMoveY(0f, 0.5f).SetDelay(0.1f).SetEase(Ease.Linear);
                lvl_Panel.transform.DOLocalMoveY(-1000, 0.5f).SetEase(Ease.Linear);
                for (int i = 0; i < Arrows.Length; i++)
                {
                    Arrows[i].SetActive(false);
                }
                for (int i = 0; i <= 3; i++)
                {
                    HPbar[i].SetActive(false);
                }
                aboutus.SetActive(true);
                bullets.SetActive(false);
                backButton.gameObject.SetActive(false);
                // lvl_Panel.SetActive(true);
                print(whereUX);

            }

        }
        public void GoToLevels()
        {
            GetComponent<AudioSource>().Play();

            backButton.gameObject.SetActive(true);
            //InitCheckTanks();
            whereUX = "LVLS";
            GameModePanel.instance.ChangeState(LevelsPanel.instance);
            ScrollerPlus.CanScroll = true;
            for (int i = 0; i < Arrows.Length; i++)
            {
                Arrows[i].SetActive(true);
            }
            for (int i = 0; i <= 3; i++)
            {
                HPbar[i].SetActive(true);
            }

            Modes_Panel.transform.DOLocalMoveY(-1000f, 0.8f).SetEase(Ease.Linear);
            lvl_Panel.transform.DOLocalMoveY(-660, 0.5f).SetEase(Ease.Linear);
            // lvl_Panel.SetActive(true);
            print(whereUX);
        }
        public void GoToUpgrade()
        {
            GetComponent<AudioSource>().Play();


            if (Level.UpgradeUI.t_selectedTankName == "tank_yellow" && PlayerPrefs.GetString("TANK_1_Lock") == "Unlocked")
            {

                for (int i = 0; i <= 3; i++)
                {
                    HPbar[i].SetActive(true);
                }
                whereUX = "UPG";
                Modes_Panel.transform.DOLocalMoveY(-400f, 0.8f).SetEase(Ease.Linear);
                // Upgrade_Panel.SetActive(true);
                lvl_Panel.transform.DOLocalMoveY(-1000, 0.5f).SetEase(Ease.Linear);
                Upgrade_Panel.transform.DOLocalMoveY(-800, .5f).SetDelay(0.5f).SetEase(Ease.Linear);
                print(whereUX);
                bullets.SetActive(true);
                backButton.gameObject.SetActive(true);

            }
            else if (Level.UpgradeUI.t_selectedTankName == "tank_green1" && PlayerPrefs.GetString("TANK_2_Lock") == "Unlocked")
            {
                for (int i = 0; i <= 3; i++)
                {
                    HPbar[i].SetActive(true);
                }
                whereUX = "UPG";
                Modes_Panel.transform.DOLocalMoveY(-400f, 0.8f).SetEase(Ease.Linear);
                // Upgrade_Panel.SetActive(true);
                lvl_Panel.transform.DOLocalMoveY(-1000, 0.5f).SetEase(Ease.Linear);
                Upgrade_Panel.transform.DOLocalMoveY(-800, .5f).SetDelay(0.5f).SetEase(Ease.Linear);
                print(whereUX);
                bullets.SetActive(true);
                backButton.gameObject.SetActive(true);

            }
            else if (Level.UpgradeUI.t_selectedTankName == "tank_green2" && PlayerPrefs.GetString("TANK_3_Lock") == "Unlocked")
            {
                for (int i = 0; i <= 3; i++)
                {
                    HPbar[i].SetActive(true);
                }
                whereUX = "UPG";
                Modes_Panel.transform.DOLocalMoveY(-400f, 0.8f).SetEase(Ease.Linear);
                // Upgrade_Panel.SetActive(true);
                lvl_Panel.transform.DOLocalMoveY(-1000, 0.5f).SetEase(Ease.Linear);
                Upgrade_Panel.transform.DOLocalMoveY(-800, .5f).SetDelay(0.5f).SetEase(Ease.Linear);
                print(whereUX);
                bullets.SetActive(true);
                backButton.gameObject.SetActive(true);

            }
            else if (Level.UpgradeUI.t_selectedTankName == "tank_blue" && PlayerPrefs.GetString("TANK_4_Lock") == "Unlocked")
            {
                for (int i = 0; i <= 3; i++)
                {
                    HPbar[i].SetActive(true);
                }
                whereUX = "UPG";
                Modes_Panel.transform.DOLocalMoveY(-400f, 0.8f).SetEase(Ease.Linear);
                // Upgrade_Panel.SetActive(true);
                lvl_Panel.transform.DOLocalMoveY(-1000, 0.5f).SetEase(Ease.Linear);
                Upgrade_Panel.transform.DOLocalMoveY(-800, .5f).SetDelay(0.5f).SetEase(Ease.Linear);
                print(whereUX);
                bullets.SetActive(true);
                backButton.gameObject.SetActive(true);

                print(PlayerPrefs.GetString("TANK_4_Lock") + " vaziate tanke 4");


            }
        }
        void Update()
        {
            
            scrollplus.onChange.AddListener(() =>
            {               
                    t_selectedTank = scrollplus.currentItem.gameObject.name;
            });
            
            #region check

            //
            // Locks
            //
            if (t_selectedTank == "tank_green1" && PlayerPrefs.GetString("TANK_2_Lock") == "Locked")
            {
                print(PlayerPrefs.GetString("TANK_2_Lock"));
                play.GetComponent<Button>().interactable = false;
                upgrade.GetComponent<Button>().interactable = false;
                play.GetComponent<Image>().sprite = unlockBW_playBW[0]; //play
                upgrade.GetComponent<Image>().sprite = unlockBW_playBW[1]; //Upgrade
            }
            else if (t_selectedTank == "tank_green2" && PlayerPrefs.GetString("TANK_3_Lock") == "Locked")
            {
                play.GetComponent<Button>().interactable = false;
                upgrade.GetComponent<Button>().interactable = false;
                play.GetComponent<Image>().sprite = unlockBW_playBW[0]; //play
                upgrade.GetComponent<Image>().sprite = unlockBW_playBW[1]; //Upgrade
            }
            else if (t_selectedTank == "tank_blue" && PlayerPrefs.GetString("TANK_4_Lock") == "Locked")
            {
                play.GetComponent<Button>().interactable = false;
                upgrade.GetComponent<Button>().interactable = false;
                play.GetComponent<Image>().sprite = unlockBW_playBW[0]; //play
                upgrade.GetComponent<Image>().sprite = unlockBW_playBW[1]; //Upgrade
            }

            //
            // Unlocks
            //


            if (t_selectedTank == "tank_yellow")
            {
                play.GetComponent<Button>().interactable = true;
                upgrade.GetComponent<Button>().interactable = true;
                play.GetComponent<Image>().sprite = unlockColor_playColor[0]; //play
                upgrade.GetComponent<Image>().sprite = unlockColor_playColor[1]; //Upgrade
            }
                if (t_selectedTank == "tank_green1" && PlayerPrefs.GetString("TANK_2_Lock") == "Unlocked")
            {
                play.GetComponent<Button>().interactable = true;
                upgrade.GetComponent<Button>().interactable = true;
                play.GetComponent<Image>().sprite = unlockColor_playColor[0]; //play
                upgrade.GetComponent<Image>().sprite = unlockColor_playColor[1]; //Upgrade
            }
            else if (t_selectedTank == "tank_green2" && PlayerPrefs.GetString("TANK_3_Lock") == "Unlocked")
            {
                play.GetComponent<Button>().interactable = true;
                upgrade.GetComponent<Button>().interactable = true;
                play.GetComponent<Image>().sprite = unlockColor_playColor[0]; //play
                upgrade.GetComponent<Image>().sprite = unlockColor_playColor[1]; //Upgrade
            }
            else if (t_selectedTank == "tank_blue" && PlayerPrefs.GetString("TANK_4_Lock") == "Unlocked")
            {
                play.GetComponent<Button>().interactable = true;
                upgrade.GetComponent<Button>().interactable = true;
                play.GetComponent<Image>().sprite = unlockColor_playColor[0]; //play
                upgrade.GetComponent<Image>().sprite = unlockColor_playColor[1]; //Upgrade
                //

                #endregion
            }
        }
    }
}
