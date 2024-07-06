using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using Amulay.Utility;

namespace TankStars.Level
{
    public class CameraManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static CameraManager instance { get; private set; }
        internal Camera main { get; private set; }


        private Vector3 mainCameraOffset = new Vector3(0, 0, -10);
        private float fieldOfView_defualt = 120;
        private float fieldOfView_zoom = 100;
        private float touchZoomPower = 10000;
        private float touchMovePower = 4000;
        private bool firstRouond = true;

        private void Awake()
        {
            instance = this;

            main = Camera.main;
            fieldOfView_defualt = main.fieldOfView;
            main.transform.localPosition = mainCameraOffset;
        }

        private void Start()
        {
            //init events
            {
                GameplayManager.instance.onStartNewRound += GameplayManager_onStartNewRound;

                var tanks = GameplayManager.instance.tanks;
                for (int i = 0; i < tanks.Count; i++)
                {
                    var tank = tanks[i];
                    tanks[i].onGetDamage += (a, b, c) => Tank_OnGetDamage(tank, a, b, c);
                }
            }

            Invoker.Do(.5f, ShowAllTanksOnStart);
        }
        private Vector3 touchStart;
        public float groundZ = 0;

        private void Update()
        {
            TouchZoom();
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = GetWorldPosition(groundZ);
              //  Debug.Log("Touch Start = " + touchStart);
               
            }
            if (touchStart.y < -2f) return;

            if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - GetWorldPosition(groundZ);
                main.transform.position += direction;
                var target = main.transform.position;
                target.x = Mathf.Clamp(target.x, -80, 80);
                target.y = Mathf.Clamp(target.y, -3, 1);
                main.transform.position = target;
            }
        }
        private Vector3 GetWorldPosition(float z)
        {

            Ray mousePos = main.ScreenPointToRay(Input.mousePosition);
            Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
            float distance;
            ground.Raycast(mousePos, out distance);
            return mousePos.GetPoint(distance);
        }
        private void TouchZoom()
        {
            if (Input.touchCount == 2)
            {
                var touchA = Input.GetTouch(0);
                var touchB = Input.GetTouch(1);
                if (touchA.phase == TouchPhase.Moved || touchB.phase == TouchPhase.Moved)
                {
                    var touchA_lastFramePosition = (touchA.position - touchA.deltaPosition);
                    var touchB_lastFramePosition = (touchB.position - touchB.deltaPosition);

                    var currentDistance = ((touchA.position - touchB.position) / Screen.height).magnitude;
                    var lastFrameDistance = ((touchA_lastFramePosition - touchB_lastFramePosition) / Screen.height).magnitude;
                    var delta = lastFrameDistance - currentDistance;
                    delta = delta * touchZoomPower * Time.deltaTime;
                    print(delta);
                    if (Mathf.Abs(delta) > 2)
                        main.fieldOfView = Mathf.Clamp(main.fieldOfView + delta, fieldOfView_zoom - 10, fieldOfView_defualt);
                    else
                    {
                        var center = (touchA.position + touchB.position) / 2f;
                        var lastFramecenter = (touchA_lastFramePosition + touchB_lastFramePosition) / 2f;
                        Vector3 deltaCenter = ((center - lastFramecenter) / Screen.width);
                        deltaCenter = deltaCenter * touchMovePower * Time.deltaTime;

                        var mainCameraPosition = main.transform.position;
                        var target = mainCameraPosition - deltaCenter;
                        target.x = Mathf.Clamp(target.x, -15, 15);
                        target.y = Mathf.Clamp(target.y, -3, 1);
                        main.transform.position = target;
                    }
                }
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("on begin drag");
           // _lastPosition = eventData.position;
           // OnSwipeStart.Invoke(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
           // OnSwipeEnd.Invoke(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Vector2 direction = eventData.position - _lastPosition;
            //_lastPosition = eventData.position;
            //
           // OnSwipe.Invoke(direction);
        }
        private void ClickDrag()
        {
            //if (Input.get == 2)
            {
                var touchA = Input.GetTouch(0);
                var touchB = Input.GetTouch(1);
                if (touchA.phase == TouchPhase.Moved || touchB.phase == TouchPhase.Moved)
                {
                    var touchA_lastFramePosition = (touchA.position - touchA.deltaPosition);
                    var touchB_lastFramePosition = (touchB.position - touchB.deltaPosition);

                    var currentDistance = ((touchA.position - touchB.position) / Screen.height).magnitude;
                    var lastFrameDistance = ((touchA_lastFramePosition - touchB_lastFramePosition) / Screen.height).magnitude;
                    var delta = lastFrameDistance - currentDistance;
                    delta = delta * touchZoomPower * Time.deltaTime;
                    print(delta);
                    if (Mathf.Abs(delta) > 2)
                        main.fieldOfView = Mathf.Clamp(main.fieldOfView + delta, fieldOfView_zoom - 10, fieldOfView_defualt);
                    else
                    {
                        var center = (touchA.position + touchB.position) / 2f;
                        var lastFramecenter = (touchA_lastFramePosition + touchB_lastFramePosition) / 2f;
                        Vector3 deltaCenter = ((center - lastFramecenter) / Screen.width);
                        deltaCenter = deltaCenter * touchMovePower * Time.deltaTime;

                        var mainCameraPosition = main.transform.position;
                        var target = mainCameraPosition - deltaCenter;
                        target.x = Mathf.Clamp(target.x, -15, 15);
                        target.y = Mathf.Clamp(target.y, -3, 1);
                        main.transform.position = target;
                    }
                }
            }
        }

        private void Tank_OnGetDamage(Tank tank, float currentHealth, float damage, Item c)
        {
            //tank destroyed
            if(currentHealth <= 0)
            {
                //StopAllAnimations();
                transform.DOKill();
                transform.DOMove(tank.transform.position, .3f);
                main.DOFieldOfView(fieldOfView_zoom, .5f);

            }
        }

        private void StopAllAnimations()
        {
            main.DOKill();
            main.transform.DOKill();
            transform.DOKill();
        }

        private void GameplayManager_onStartNewRound(Tank tank)
        {
            if (firstRouond)
            {
                firstRouond = false;
                return;
            }

            ShowCenter();
        }

        private void ShowDeadTank()
        {

        }

        private void ShowCenter()
        {
            var tanks = GameplayManager.instance.tanks;
            var bounds = new Bounds();
            for (int i = 0; i < tanks.Count; i++)
                bounds.Encapsulate(tanks[i].transform.position);

            //StopAllAnimations();
            transform.DOKill();
            transform.DOMove(Vector3.right * bounds.center.x, .5f);
            main.DOFieldOfView(fieldOfView_defualt, .5f);
        }

        private void ShowAllTanksOnStart()
        {
            var tanks = GameplayManager.instance.tanks;
            float startDelay = 0;
            float animaionDurationPerObj = .7f;
            float animaionDelayPerObj = .7f;
            main.DOFieldOfView(fieldOfView_zoom, animaionDurationPerObj).SetDelay(startDelay);

            //zoom camera on tanks
            for (int i = 0; i < tanks.Count; i++)
            {
                transform.DOMove(tanks[i].transform.position, animaionDurationPerObj).SetDelay(i * animaionDelayPerObj + startDelay);
            }

            float totalDelay = tanks.Count * animaionDurationPerObj + animaionDurationPerObj + startDelay;
            main.DOFieldOfView(fieldOfView_defualt, .5f).SetDelay(totalDelay);
            Invoker.Do(totalDelay + .1f, ShowCenter);
        }

        internal void Shake(float duraion = 1, float strenght = 3, int vibrato = 10)
        {

            main.DOKill();
            main.DOShakePosition(duraion, new Vector3(strenght, strenght), vibrato).onComplete += () =>
                main.transform.DOLocalMove(mainCameraOffset, .3f);
        }
    }
}