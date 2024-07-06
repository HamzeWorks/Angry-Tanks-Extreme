using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Amulay.Utility;

namespace TankStars.Menu
{
    public class GameModeMenu : Singleton<GameModeMenu>
    {
        [SerializeField] CanvasGroup gameModeCanvas;
        [SerializeField] Button offline_pvc_button;
        [SerializeField] Button offline_wave_button;
        [SerializeField] Button online_pvp_button;
        [SerializeField] Button online_private_pvp_button;
        [SerializeField] Button online_join_private_pvp_button;
        [SerializeField] TMPro.TMP_InputField joinIDText;
        [SerializeField] Button[] offline_level_buttons;

        protected override void Awake()
        {
            base.Awake();
            gameModeCanvas.interactable = true;

            offline_pvc_button.onClick.AddListener(() =>
            {
                GameManager.instance.SetGameMode(SceneEnum.Level_AI, 0);
            });

            offline_wave_button.onClick.AddListener(() =>
            {

            });

            online_pvp_button.onClick.AddListener(async() =>
            {
                //online_pvp_button.interactable = false;
                var success = await GameManager.instance.SetGameMode(SceneEnum.Level_Online_2P);
                if (success == false)
                    online_pvp_button.interactable = true;
            });


            joinIDText.onValueChanged.AddListener((s) => { online_join_private_pvp_button.interactable = s.Length == 4; });

            online_private_pvp_button.onClick.AddListener(async () =>
            {
                //online_pvp_button.interactable = false;
                var success = await GameManager.instance.SetGameMode(SceneEnum.Level_Online_2P, -1);
                if (success == false)
                    online_private_pvp_button.interactable = true;
            });

            online_join_private_pvp_button.onClick.AddListener(async () =>
            {
                //online_pvp_button.interactable = false;
                var success = await GameManager.instance.SetGameMode(SceneEnum.Level_Online_2P, int.Parse(joinIDText.text));
                if (success == false)
                    online_private_pvp_button.interactable = true;
            });


            int t_unlockedLevelIndex = GameManager.unlockedLevelIndex;
            for (int i = 0; i < offline_level_buttons.Length; i++)
            {
                offline_level_buttons[i].interactable = (i < t_unlockedLevelIndex);
                int index = i;
                offline_level_buttons[i].onClick.AddListener(() =>
                {
                    GameManager.instance.SetGameMode(SceneEnum.Level_AI, index + 1);
                });
            }
        }


        private void Start()
        {
            transform.DOPunchScale(new Vector3(.1f, -.1f), .5f, 3);
        }
    }
}