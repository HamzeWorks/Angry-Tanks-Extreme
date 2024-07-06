using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Linq;
using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace Amulay.Utility
{
    public class ScrollerPlus : MonoBehaviour
    {
        [SerializeField] RectTransform viewport;
        [SerializeField] RectTransform content;
        [Header("Items")]
        [SerializeField] bool autoFillItemsOnAwake = true;
        [SerializeField] bool autoUpdateItems = false;
        [SerializeField] List<RectTransform> items;
        [Header("Options :")]
        [SerializeField] public bool playOnStart = true;
        [SerializeField] public int startItemIndex = 0; //*make properties
        [Space(5)]
        [SerializeField] bool clamped = false;
        [SerializeField] bool makeLoop = false;
        [SerializeField, Min(0)] int loopRange = 1;
        [Header("External Options")]
        [SerializeField] Button nextScrollButton;
        [SerializeField] Button previousScrollButton;
        [Header("Animation Setting")]
        [SerializeField] Ease ease = Ease.OutQuart;
        [SerializeField, Range(0, 3f)] float easeDuration = .5f;
        [Header("Selected Item Animation")]
        [SerializeField] Ease selectedItemEase = Ease.Linear;
        [SerializeField] Vector3 selectedItemAnimationValue = new Vector3(.1f, .1f, 0);
        [SerializeField, Range(0, 3f)] float selectedItemEaseDuration = 0;
        [Header("Events"), Space(10)]
        [SerializeField] public UnityEvent onChange;
        [SerializeField] public UnityEvent onChangeAnimationCompelet;


        protected EventTrigger contentEvent;
        public int currentItemIndex { get; protected set; } = 0;
        public int previousItemIndex { get; protected set; } = -1;
        public RectTransform currentItem => items[currentItemIndex];
        //TODO:Rename
        public RectTransform _content => content;
        public List<RectTransform> _items => items;
        public RectTransform this[int itemIndex] => items[itemIndex % items.Count];

        static public bool CanScroll = false;

        private void Awake()
        {
            Init();
        }

        private IEnumerator Start()
        {
            yield return null;
            if (playOnStart)
                ScrollTarget(startItemIndex);
        }

        private void Init()
        {
            if (viewport == null)
                viewport = CreateViewPort(transform);
            if (content == null)
                content = CreateConternt(viewport);

            nextScrollButton?.onClick.AddListener(() => { ScrollStep(1); });
            previousScrollButton?.onClick.AddListener(() => { ScrollStep(-1); });

            if (autoFillItemsOnAwake)
            {
                items = new List<RectTransform>();
                for (int i = 0; i < content.childCount; i++)
                    items.Add(content.GetChild(i) as RectTransform);
            }

            onChangeAnimationCompelet.AddListener(() =>
            {
                if (selectedItemEase == Ease.Unset || selectedItemEaseDuration <= 0)
                    return;
                currentItem.transform.DOKill();
                currentItem.transform.localScale = Vector3.one;
                currentItem.transform.DOPunchScale(selectedItemAnimationValue, selectedItemEaseDuration, 3);
            });

            {
                contentEvent = viewport.gameObject.AddComponent<EventTrigger>();
                var beginDrag = new EventTrigger.Entry();
                beginDrag.eventID = EventTriggerType.BeginDrag;
                beginDrag.callback.AddListener(ViewportBeginDrag);

                contentEvent.triggers.Add(beginDrag);
                var drag = new EventTrigger.Entry();
                drag.eventID = EventTriggerType.Drag;
                drag.callback.AddListener(ViewportDrag);

                contentEvent.triggers.Add(drag);
                var endDrag = new EventTrigger.Entry();
                endDrag.eventID = EventTriggerType.EndDrag;
                endDrag.callback.AddListener(ViewportEndDrag);
                contentEvent.triggers.Add(endDrag);
            }
            CanScroll = false;
        }

        public void ScrollStep(int Step)
        {
            if (clamped)
                if (currentItemIndex + Step >= items.Count || currentItemIndex + Step < 0)
                    return;

            int target = (currentItemIndex + Step + items.Count) % items.Count;

            if (makeLoop && items.Count >= 2)
            {
                if (currentItemIndex + Step + loopRange >= items.Count)
                {
                    var offset = items[items.Count - 1].localPosition - items[items.Count - 2].localPosition;
                    items[0].localPosition = items[items.Count - 1].localPosition + offset;
                    ShiftLeft();
                }
                else
                if (currentItemIndex + Step - loopRange <= -1)
                {
                    var offset = items[0].localPosition - items[1].localPosition;
                    items[items.Count - 1].localPosition = items[0].localPosition + offset;
                    ShiftRight();
                }
                target = Mathf.Clamp(currentItemIndex + Step, 0, items.Count);
            }

            ScrollTarget(target);
        }

        public void ScrollTarget(int target)
        {
            target = (target + items.Count) % items.Count; //if less than zero or more than items.count then automatc fix

            var delta = viewport.position - items[target].position;

            content.transform.DOKill();
            content.transform.DOMove(content.position + delta, easeDuration).SetEase(ease).onComplete += () => onChangeAnimationCompelet?.Invoke();

            previousItemIndex = currentItemIndex;
            currentItemIndex = target;

         

            onChange?.Invoke();
        }

        public void AddItem(Transform item)
        {
            item.SetParent(content);
            items.Add(item as RectTransform);
        }

        private void ShiftRight()
        {
            if (items.Count == 0)
                return;

            var lastItem = items[items.Count - 1];
            for (int i = items.Count - 1; i >= 1; i--)
                items[i] = items[i - 1];
            items[0] = lastItem;
            currentItemIndex = (currentItemIndex + 1 + items.Count) % items.Count;
        }

        private void ShiftLeft()
        {
            if (items.Count == 0)
                return;
            var firstItem = items[0];
            for (int i = 0; i < items.Count - 1; i++)
                items[i] = items[i + 1];
            items[items.Count - 1] = firstItem;
            currentItemIndex = (currentItemIndex - 1 + items.Count) % items.Count;
        }

        #region viewport drag

        [Header("Draging")]
        [SerializeField, Range(.01f, 2f)] float dragScrollingRange = .1f;
        [SerializeField] bool reverseDragX = false;
        [SerializeField] bool reverseDragY = false;

        private Vector3 beginDragMousePosition;
        private void ViewportBeginDrag(BaseEventData data)
        {
            //dragScrollingRange = Vector2.Distance(items[0].transform.position, items[1].transform.position);
            beginDragMousePosition = Input.mousePosition;
        }

        private void ViewportDrag(BaseEventData data)
        {
            if (CanScroll)
            {

                if (((Input.mousePosition - beginDragMousePosition) / Screen.height).magnitude > dragScrollingRange)
                {
                    Vector2 delta = Input.mousePosition - beginDragMousePosition;
                    int direction = 0;
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        direction = (delta.x > 0) ? 1 : -1;
                        if (reverseDragX)
                            direction = -direction;
                    }
                    else
                    {
                        direction = (delta.y > 0) ? 1 : -1;
                        if (reverseDragY)
                            direction = -direction;
                    }
                    ScrollStep(direction);
                    beginDragMousePosition = Input.mousePosition;


                }
            }
        }

        private void ViewportEndDrag(BaseEventData data)
        {

        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (playOnStart && currentItemIndex != startItemIndex && items.Count > 0)
                ScrollTarget(startItemIndex);
        }
#endif

        #region creation
        public static ScrollerPlus Create(Transform parent = null)
        {
            var rect = new GameObject("ScrollerPlus").AddComponent<RectTransform>();
            rect.SetParent(parent);
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.sizeDelta = new Vector2(200f, 200f);
            var scrollPlus = rect.gameObject.AddComponent<ScrollerPlus>();
            RectTransform viewport = CreateViewPort(rect);
            RectTransform content = CreateConternt(viewport);

            scrollPlus.content = content;
            scrollPlus.viewport = viewport;
            return scrollPlus;
        }

        private static RectTransform CreateConternt(RectTransform viewport)
        {
            var content = new GameObject("Content").AddComponent<RectTransform>();
            content.SetParent(viewport.transform);
            content.localPosition = Vector3.zero;
            content.localScale = Vector3.one;
            content.anchorMin = Vector2.zero;
            content.anchorMax = Vector2.one;
            content.sizeDelta = Vector2.zero;
            return content;
        }

        private static RectTransform CreateViewPort(Transform parent)
        {
            var viewport = new GameObject("Viewport").AddComponent<RectTransform>();
            viewport.SetParent(parent);
            viewport.localPosition = Vector3.zero;
            viewport.localScale = Vector3.one;
            viewport.anchorMin = Vector2.zero;
            viewport.anchorMax = Vector2.one;
            viewport.sizeDelta = Vector2.zero;
            viewport.gameObject.AddComponent<Image>();
            viewport.gameObject.AddComponent<Mask>();
            return viewport;
        }

        #endregion
    }
}