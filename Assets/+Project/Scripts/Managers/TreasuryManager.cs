using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TankStars
{
    public class TreasuryManager : Singleton<TreasuryManager>
    {
        private Pint coin = new Pint("TreasuryManager." + nameof(coin), 1000);
        private Pint gem = new Pint("TreasuryManager." + nameof(gem), 20);

        public int Coin { get => coin.Value; 
            set 
            {
                if (coin.Value == value)
                    return;
                int previousValue = coin.Value;
                coin.Value = value;
                onCoinChanged?.Invoke(value, value - previousValue);
            }
        }
        public int Gem
        {
            get => gem.Value;
            set
            {
                if (gem.Value == value)
                    return;
                int previousValue = gem.Value;
                gem.Value = value;
                onGemChanged?.Invoke(value, value - previousValue);
            }
        }

        /// <summary>
        /// parameter1: current value, parameter2: added value;
        /// </summary>
        public event Action<int, int> onCoinChanged;
        /// <summary>
        /// parameter1: current value, parameter2: added value;
        /// </summary>
        public event Action<int, int> onGemChanged;

        protected override void Awake()
        {
            base.Awake();
            if (instance != this)
                return;
            
        }
    }


}