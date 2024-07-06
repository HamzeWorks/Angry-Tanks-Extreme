using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace TankStars.Menu
{
    public class LevelsPanel : Singleton<LevelsPanel>, IState
    {
        private Transform panel;
        [SerializeField] private Button closeButton;
        [SerializeField] private LevelStage_UI[] levels;


        protected override void Awake()
        {
            base.Awake();
            panel = transform.GetChild(0);
            panel.gameObject.SetActive(false);
            closeButton.onClick.AddListener(() =>
            {
                GameModePanel.instance.ChangeState(BattleModePanel.instance);
            });
            /*
            int t_unlockedLevelIndex = GameManager.unlockedLevelIndex;
                    for (int i = 0; i < levels.Length; i++)
                    {
                        int index = i;
                        bool isLock = i >= t_unlockedLevelIndex;
                        bool isFocus = i == (t_unlockedLevelIndex - 1);
                        levels[i].Init(index, (isLock || isFocus) ? 0 : 3, isLock, isFocus, () => {
                            GameManager.instance.SetGameMode(SceneEnum.Level_AI, index + 1);
                        });
                    }
            */
        }

        public void OnEnter(IState previousState, Action<IState> onCompelete = null)
        {
            panel.gameObject.SetActive(true);
        }

        public void OnExit(IState nextState)
        {
            Close();
        }

        private void Close()
        {
           // panel.gameObject.SetActive(false);
        }
        public void GoToGame()
        {
            {
                if (TankStars.Level.UpgradeUI.t_selectedTankName == "tank_yellow" && PlayerPrefs.GetString("TANK_1_Lock") == "Unlocked")
                {
                    print("Go To Game");
                   
                   
                    
                            GameManager.instance.SetGameMode(SceneEnum.Level_AI, 2);
                      
                    }
                
                else
                if (Level.UpgradeUI.t_selectedTankName == "tank_green1" && PlayerPrefs.GetString("TANK_2_Lock") == "Unlocked")
                {
                    GameManager.instance.SetGameMode(SceneEnum.Level_AI, 2);
                }
                else
                if (Level.UpgradeUI.t_selectedTankName == "tank_green2" && PlayerPrefs.GetString("TANK_3_Lock") == "Unlocked")
                {
                    GameManager.instance.SetGameMode(SceneEnum.Level_AI, 2);
                }
                else
                if (Level.UpgradeUI.t_selectedTankName == "tank_blue" && PlayerPrefs.GetString("TANK_4_Lock") == "Unlocked")
                {
                    GameManager.instance.SetGameMode(SceneEnum.Level_AI, 2);
                }
            }
        }
    }
}
