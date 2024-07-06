using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace TankStars.Menu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PrivateRoomPanel : Singleton<PrivateRoomPanel>, IState
    {
        private const float offset = 300f;

        [SerializeField] private Button joinButton;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button backButton;

        private CanvasGroup canvasGroup;
        private Vector3 showPostion;
        private Vector3 hidePostion;
        private bool cachedPosition = false;
        private Transform panel;

        protected override void Awake()
        {
            base.Awake();
            panel = transform.GetChild(0);
            canvasGroup = GetComponent<CanvasGroup>();

            backButton.onClick.AddListener(() => {
                GameModePanel.instance.ChangeState(BattleModePanel.instance);
            });
        }

        public void OnEnter(IState previousState, Action<IState> onCompelete = null)
        {
            if (cachedPosition == false)
            {
                showPostion = panel.localPosition;
                hidePostion = showPostion + Vector3.down * offset;
                panel.localPosition = hidePostion;
                cachedPosition = true;
            }
            panel.gameObject.SetActive(true);
            gameObject.SetActive(true);
            panel.DOKill();
            panel.DOLocalMoveY(showPostion.y, 1).SetEase(Ease.OutBack).onComplete += () =>
            {
                for (int i = 0; i < panel.childCount; i++)
                    panel.GetChild(i).DOPunchScale(Vector3.one * .2f, .3f, 3).SetDelay(i * .2f);
            };
        }

        public void OnExit(IState nextState)
        {
            panel.DOKill();
            panel.DOLocalMoveY(hidePostion.y, .5f).SetEase(Ease.OutQuad);
        }
    }
}
