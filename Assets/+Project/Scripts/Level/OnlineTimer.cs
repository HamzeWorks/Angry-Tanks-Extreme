using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Amulay.Utility;

namespace TankStars.Level
{
    public class OnlineTimer : Singleton<OnlineTimer>
    {
        [SerializeField] TMPro.TextMeshProUGUI text;
        private float startTime;
        private float duration = 25f;
        private int lastPritedTime;
        protected override void Awake()
        {
            if (instance != this)
                return;
            GameplayManager.instance.onStartNewRound += GameplayManager_onStartNewRound;
            Restart();
        }

        private void GameplayManager_onStartNewRound(Tank obj)
        {
            Restart();
        }

        private void Restart()
        {
            enabled = true;
            startTime = Time.time;
            lastPritedTime = (int)duration;
            text.text = lastPritedTime.ToString();

            Animaion();
        }

        private void Update()
        {
            if(lastPritedTime != Mathf.FloorToInt(duration - (Time.time - startTime)))
            {
                lastPritedTime = Mathf.FloorToInt(duration - (Time.time - startTime));
                if (lastPritedTime < 0)
                {
                    //enabled = false;
                    return;
                }

                if (lastPritedTime <= 10)
                    Animaion();

                text.text = lastPritedTime.ToString();
            }
        }

        private void Animaion()
        {
            text.transform.DOKill();
            text.transform.localScale = Vector3.one;
            text.transform.DOPunchScale(Vector3.one * .2f, .3f, 3);
        }
    }
}