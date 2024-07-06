using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amulay.Utility;
using TMPro;
using TankStars.Level;

namespace TankStars.Menu
{
    public class MainMenu : Singleton<MainMenu>
    {

        [SerializeField] Button openUpgradePanelButton;
        [SerializeField] Button closeUpgradePanelButton;
        [SerializeField] CanvasGroup gameModePanel;
        [SerializeField] CanvasGroup upgradePanel;
        [SerializeField] ItemCard[] upgradeItemes;
        [SerializeField] TextMeshProUGUI coinText;
        [SerializeField] TextMeshProUGUI WinLoseCount;


        private string t_selectedTankName = "tank_green";

        private MainMenuState state = MainMenuState.SelectGameMode;
        private MainMenuState previousState = MainMenuState.SelectGameMode;

        protected override void Awake()
        {
            base.Awake();
            if (instance != this)
                return;

            openUpgradePanelButton.onClick.AddListener(() =>
            {
                SwitchState(MainMenuState.Upgrade);
            });

            closeUpgradePanelButton.onClick.AddListener(() =>
            {
                SwitchState(MainMenuState.SelectGameMode);
            });
        }




        private void SwitchState(MainMenuState newState)
        {
            OnExitState(state);
            previousState = state;
            OnEnterState(newState);
            state = newState;
        }

        private void OnExitState(MainMenuState state)
        {
            switch (state)
            {
                case MainMenuState.SelectGameMode:
                    gameModePanel.gameObject.SetActive(false);
                    break;
                case MainMenuState.Upgrade:
                    openUpgradePanelButton.gameObject.SetActive(true);
                    closeUpgradePanelButton.gameObject.SetActive(false);
                    upgradePanel.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private void OnEnterState(MainMenuState state)
        {
            //switch (state)
            //{
            //    case MainMenuState.SelectGameMode:
            //        scrollerNextButton.gameObject.SetActive(true);
            //        scrollerPreviousButton.gameObject.SetActive(true);
            //        gameModePanel.gameObject.SetActive(true);
            //        break;
            //    case MainMenuState.Upgrade:
            //        openUpgradePanelButton.gameObject.SetActive(false);
            //        closeUpgradePanelButton.gameObject.SetActive(true);
            //        scrollerNextButton.gameObject.SetActive(false);
            //        scrollerPreviousButton.gameObject.SetActive(false);
            //        upgradePanel.gameObject.SetActive(true);

            //        var tank = GameManager.instance.selectedTanks[0];
            //        for (int i = 0; i < tank._defaultItems.Length; i++)
            //            this.upgradeItemes[i].SetData(tank._defaultItems[i].data);
            //        break;
            //    default:
            //        break;
            //}
        }

        private enum MainMenuState
        {
            SelectGameMode,
            Upgrade,
        }
    }
}