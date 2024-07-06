using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TankStars.Level
{
    public class ItemsPanel : MonoBehaviour
    {
        public static ItemsPanel instance { get; private set; }

        [SerializeField] Transform panel;
        [SerializeField] Transform itemsContainer;
        [SerializeField] Button openPanelButton;
        [SerializeField] Button closePanelButton;
        [SerializeField] Image selectedItemIcon;

        List<Item_UI> items = new List<Item_UI>();

        internal event Action<Item> onSelectItem;


        private void Awake()
        {
            instance = this;
            openPanelButton.onClick.AddListener(OpenPanel);
            closePanelButton.onClick.AddListener(ClosePanel);
        }

        private void Start()
        {
            if (GameplayManager.instance.localPlayerTank == null)
                print("☺");
            //TODO:Active this
            GameplayManager.instance.localPlayerTank.onShoot += (item, firepoint, aim, power, action) => UpdateList();
            UpdateList();
        }

        private void ClosePanel()
        {
            panel.gameObject.SetActive(false);
        }

        private void OpenPanel()
        {
            panel.gameObject.SetActive(true);
            UpdateList();
        }

        internal void UpdateList()
        {
            var tank = GameplayManager.instance.localPlayerTank;
            var list = tank.itemsList;
            int index = 0;
            selectedItemIcon.sprite = tank.selectedItem.icon;
            for (int i = 0; i < items.Count && index < list.Count; i++,index++) //set created items
            {
                items[i].Set(list[index], OnSelectItem);
                items[i].gameObject.SetActive(true);
            }

            for (; index < list.Count;index++)  //create new items
            {
                var item_UI = Item_UI.Create(list[index], itemsContainer, OnSelectItem);
                items.Add(item_UI);
            }

            for (int i = list.Count; i < items.Count; i++)  //inactive other items
            {
                items[i].gameObject.SetActive(false);
            }

            SelectItemAnimation();
        }

        private void OnSelectItem(Item_UI item)
        {
            panel.gameObject.SetActive(false);
            selectedItemIcon.sprite = item.connectedItem.icon;
            onSelectItem?.Invoke(item.connectedItem);

            SelectItemAnimation();
        }

        private void SelectItemAnimation()
        {
            var parent = selectedItemIcon.transform.parent;
            parent.DOKill();
            parent.localScale = Vector3.one;
            parent.DOPunchScale(Vector3.one * .3f, .5f, 3).SetDelay(.05f);
        }
    }
}