using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Globalization;

namespace TankStars.Menu
{
    public class Treasury_UI : Singleton<Treasury_UI>
    {
        [SerializeField] Button addCoinButton;
        [SerializeField] TextMeshProUGUI coinText;
        [SerializeField] Button addGemCoinButton;
        [SerializeField] TextMeshProUGUI gemText;

        protected override void Awake()
        {
            base.Awake();
            if (instance != this)
                return;

	      //  addCoinButton.onClick.AddListener(() => ShopPanel.instance.ShowGoldPanel());
	      //  addGemCoinButton.onClick.AddListener(() => ShopPanel.instance.Show());
            TreasuryManager.instance.onCoinChanged += Instance_onCoinChanged;
            TreasuryManager.instance.onGemChanged += Instance_onGemChanged;
        }

     
        private void Start()
        {
            UpdateValue();

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).DOPunchScale(Vector3.one * .2f, .3f, 3).SetEase(Ease.OutElastic).SetDelay(i * .3f);
        }

        private void UpdateValue()
        {
            coinText.text = TreasuryManager.instance.Coin.ToString("0,0", CultureInfo.GetCultureInfo("en-US"));
            gemText.text = TreasuryManager.instance.Gem.ToString("0,0", CultureInfo.GetCultureInfo("en-US"));
        }

        private void Instance_onGemChanged(int value, int delta)
        {
            UpdateValue();
        }

        private void Instance_onCoinChanged(int value, int delta)
        {
            UpdateValue();
        }

        protected override void OnDestroy()
        {
            if (TreasuryManager.instance != null)
            {
                TreasuryManager.instance.onCoinChanged += Instance_onCoinChanged;
                TreasuryManager.instance.onGemChanged += Instance_onGemChanged;
            }
            base.OnDestroy();
        }

    }
}