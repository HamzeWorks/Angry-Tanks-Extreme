using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using Amulay.Utility;

namespace TankStars.Level
{
    public class HealthPanel : Singleton<HealthPanel>
    {
        [Header("Health Panel")]//TODO: Create healthbar Script
        [SerializeField] TextMeshProUGUI timerText;

        [Header("Left Player")]
        [SerializeField] HealthBar leftPlayerHealthBar;
        [Header("Right Player")]
        [SerializeField] HealthBar rightPlayerHealthBar;
        


        private void Start()
        {
            InitHealthBar();
        }

        private void InitHealthBar()
        {
            if (GameplayManager.instance.tanks.Count < 2)
                return;
            Tank leftPlayer  = GameplayManager.instance.tanks[0];
            Tank rightPlayer = GameplayManager.instance.tanks[1];
            if (leftPlayer.transform.position.x > rightPlayer.transform.position.x)
            {
                var temp = rightPlayer;
                rightPlayer = leftPlayer;
                leftPlayer = temp;
            }

            //leftUsernameText.text = leftPlayer.username;
            leftPlayerHealthBar.Init(leftPlayer);

            //rightUsernameText.text = rightPlayer.username;
            rightPlayerHealthBar.Init(rightPlayer);
        }
    }
}