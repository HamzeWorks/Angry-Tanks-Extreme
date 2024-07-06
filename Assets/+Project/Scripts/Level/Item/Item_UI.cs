using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

namespace TankStars.Level
{
    public class Item_UI : MonoBehaviour
    {
        private static Item_UI prefab;
        internal Item connectedItem { get; private set; }

        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] Button selectButton;
        [SerializeField] Image icon;

        public static Item_UI Create(Item item,Transform parent,Action<Item_UI> onSelect)
        {
            if (prefab == null)
                prefab = Resources.Load<Item_UI>("Item_UI");
            var itemUI = Instantiate(prefab, parent);
            itemUI.Set(item, onSelect);
            return itemUI;
        }

        public void Set(Item item, Action<Item_UI> onSelect)
        {
            nameText.text = item.itemName();
            icon.sprite = item.icon;
            connectedItem = item;
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => onSelect?.Invoke(this));
        }



        private void OnDestroy()
        {
            prefab = null;
        }
    }
}