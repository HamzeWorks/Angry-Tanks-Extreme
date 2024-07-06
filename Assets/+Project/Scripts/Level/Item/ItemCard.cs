using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace TankStars.Menu
{
    public class ItemCard : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Image icon;
        [SerializeField] Slider slide;
        [SerializeField] TextMeshProUGUI sliderText;
        [SerializeField] Button upgradeButton;
        [SerializeField] TextMeshProUGUI upgradeText;
        [SerializeField] TextMeshProUGUI levelText;

        private ItemData m_data;

        private void Awake()
        {
            upgradeButton.onClick.AddListener(() =>
            {
                if (CheckRule())
                {
                    GameManager.instance._coin -= m_data.upgradeCost;
                    m_data.Upgrade();
                    gameObject.transform.DOKill();
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.DOPunchScale(Vector3.one * .1f, 1f, 3);
                    UpdateData();
                }
            });
            GameManager.instance.onCoinCountChanged += Instance_onCoinCountChanged;
        }

        private void Instance_onCoinCountChanged(int value)
        {
            upgradeButton.interactable = CheckRule();
        }

        public void SetData(ItemData data)
        {
            m_data = data;
            icon.sprite = data.icon;
            upgradeText.text = $"${data.upgradeCost}";
            levelText.text = data.level.ToString();
            upgradeButton.interactable = CheckRule();
        }

        private void UpdateData()
        {
            SetData(m_data);
        }

        private bool CheckRule()
        {
            if (m_data == null)
                return false;
            return GameManager.instance._coin >= m_data.upgradeCost;
        }

        private void OnDestroy()
        {
            if (GameManager.instance != null)
                GameManager.instance.onCoinCountChanged -= Instance_onCoinCountChanged;
        }
    }
}