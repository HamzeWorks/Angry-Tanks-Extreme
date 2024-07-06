using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.EventSystems;
using Amulay.Utility;

namespace TankStars.Level
{
    public class LocalInput : Singleton<LocalInput>, IInputBase
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] TextMeshProUGUI aimText;
        [SerializeField] TextMeshProUGUI powerText;
        [SerializeField] Transform aimJoyStick;
        [SerializeField] Transform moveJoyStick;
        [SerializeField] Button fireButton;
        [SerializeField] Button selectItemButton;
        [SerializeField] Slider fuleSlider;
        

        private bool    isActive    = true;
        private bool    aiming      = false;
        private bool    moveing     = false;
        private float   aimJoyStickLength;
        private float   moveJoyStickLength;

        public event Action<float, float>   onAimInput;
        public event Action<float>          onMoveInput;
        public event Action                 onShoot;
        public event Action<Item>           onSelectItem;
        public event Action<Vector3, Quaternion, float, float>  onForceMovement;

        public Tank connectedTank => GameplayManager.instance?.localPlayerTank;
        
        protected override void Awake()
        {
            base.Awake();
            aimJoyStickLength = aimJoyStick.parent.GetComponent<RectTransform>().rect.width / 2f; // or sizeDelta.x
            moveJoyStickLength = moveJoyStick.parent.GetComponent<RectTransform>().rect.width / 2f;

            print("Aim" + aimJoyStickLength + "  " + moveJoyStickLength);
            fireButton.onClick.AddListener(()=> { SetActivePanel(false); FireButtonClick(); });
            InitEventTrigger();
            SetActivePanel(false);
        }

        private void InitEventTrigger()
        {
            //init aiming event trigger for pointer down and up
            var aimTrigger = aimJoyStick.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry aimEntryDown = new EventTrigger.Entry();
            aimEntryDown.eventID = EventTriggerType.PointerDown;
            aimEntryDown.callback.AddListener((p) => AimOnPointerDown());
            aimTrigger.triggers.Add(aimEntryDown);

            EventTrigger.Entry aimEntryUp = new EventTrigger.Entry();
            aimEntryUp.eventID = EventTriggerType.PointerUp;
            aimEntryUp.callback.AddListener((p) => AimOnPointerUp());
            aimTrigger.triggers.Add(aimEntryUp);

            //init move event trigger for pointer down and up
            var moveTrigger = moveJoyStick.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry moveEntryDown = new EventTrigger.Entry();
            moveEntryDown.eventID = EventTriggerType.PointerDown;
            moveEntryDown.callback.AddListener((p) => MoveOnPointerDown());
            moveTrigger.triggers.Add(moveEntryDown);

            EventTrigger.Entry moveEntryUp = new EventTrigger.Entry();
            moveEntryUp.eventID = EventTriggerType.PointerUp;
            moveEntryUp.callback.AddListener((p) => MoveOnPointerUp());
            moveTrigger.triggers.Add(moveEntryUp);
        }

        private void Start()
        {
            ItemsPanel.instance.onSelectItem += (item) => onSelectItem?.Invoke(item);
            UpdateInputUI();

            GameplayManager.instance.onStartNewRound += GameplayManager_onStartNewRound;
        }

        private void GameplayManager_onStartNewRound(Tank tank)
        {
            var activation = tank == connectedTank;
            if (activation == isActive)
                return;
            if (Item.flyingItems.Count > 0)
                Item.onDestroyAllFlyingItems += Item_onDestroyAllFlyingItems;
            else
                SetActivePanel(activation);
        }

        private void Item_onDestroyAllFlyingItems(Item item)
        {
            SetActivePanel(true);
            Item.onDestroyAllFlyingItems -= Item_onDestroyAllFlyingItems;
        }

        private void SetActivePanel(bool value, float delay = -1)
        {
            isActive = value;
            group.interactable = isActive;
            aimJoyStick.parent.gameObject.SetActive(isActive);
            moveJoyStick.parent.gameObject.SetActive(isActive);
            group.DOKill();

            if (isActive)
                group.DOFade(1f, .3f);
            else
            {
                group.DOFade(.1f, 1f);
                aiming = false;
                moveing = false;
            }

            if (connectedTank != null)
                UpdateInputUI();
        }

        protected virtual void Update()
        {
            if (aiming)
                AimUpdate();
            if (moveing)
                MoveUpdate();
        }

        private void AimUpdate()
        {
            aimJoyStick.transform.position = Input.mousePosition;
            var localPosition = Vector3.ClampMagnitude(aimJoyStick.localPosition, aimJoyStickLength);
            aimJoyStick.localPosition = localPosition;

            float aim = Mathf.Atan2(localPosition.y, localPosition.x) * Mathf.Rad2Deg;
            if (aim < 0)
                aim += 360f;
            float power = localPosition.magnitude / aimJoyStickLength;

            onAimInput?.Invoke(aim, power);
            UpdateInputUI();
        }

        private void MoveUpdate()
        {
            moveJoyStick.transform.position = Input.mousePosition;
            moveJoyStick.localPosition = Vector3.ClampMagnitude(moveJoyStick.localPosition, moveJoyStickLength);
            moveJoyStick.localPosition = new Vector3(moveJoyStick.localPosition.x, 0, 0);
            float power = moveJoyStick.localPosition.x / moveJoyStickLength;

            onMoveInput?.Invoke(power);
            UpdateInputUI();
        }

        private void UpdateInputUI()
        {
            aimText.text = ((int)connectedTank.currentAim).ToString();
            powerText.text = ((int)(connectedTank.currentShootPower / connectedTank.maxShootPowerr * 100f)).ToString();
            fuleSlider.value = connectedTank.currentFule / connectedTank.maxFule;
        }

        protected virtual void FireButtonClick()
        {
            onShoot?.Invoke();
        }

        private void AimOnPointerDown()
        {
            aiming = true;
        }

        private void AimOnPointerUp()
        {
            aiming = false;
            aimJoyStick.transform.localPosition = Vector3.zero;

        }

        private void MoveOnPointerDown()
        {
            moveing = true;
        }

        private void MoveOnPointerUp()
        {
            moveing = false;
            moveJoyStick.localPosition = Vector3.zero;
            onMoveInput?.Invoke(0);
        }

        protected override void OnDestroy()
        {
            Item.onDestroyAllFlyingItems -= Item_onDestroyAllFlyingItems;

            //Network.NetworkManager.onStartNewRound -= NetworkManager_onStartNewRound;
            base.OnDestroy();
        }

    }
}