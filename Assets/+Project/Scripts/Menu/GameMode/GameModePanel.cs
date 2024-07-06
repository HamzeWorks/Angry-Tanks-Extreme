using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace TankStars.Menu
{
    public class GameModePanel : StateMachine<GameModeMenu>
    {
        public static GameModePanel instance { get; private set; }
        
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            ChangeState(BattleModePanel.instance);
        }
    }
}