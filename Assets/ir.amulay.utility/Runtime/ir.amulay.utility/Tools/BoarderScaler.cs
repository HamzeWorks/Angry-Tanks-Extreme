using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor;


namespace Amulay.Utility
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class BoarderScaler : Singleton<BoarderScaler>
    {
        [SerializeField] bool scaleMode = false;
        [SerializeField] bool scalFixMode = false;
        [SerializeField] bool fixScaleMode = false;

        private Camera mainCamera;
        RectTransform rectTransform;

        Vector3 t_point;
        float t_ortho = -1;

        private void Start()
        {

        }

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            rectTransform = GetComponent<RectTransform>();
            mainCamera = Camera.main;
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        private void OnEnable()
        {
            Init();

            UpdatePosition();
            UpdateBoarder();
        }

        private void UpdateBoarder()
        {
            if (t_ortho == mainCamera.orthographicSize)
                return;

            if (scaleMode)
            {
                var currentTransform = transform.parent;

                transform.parent = null;
                rectTransform.localScale = new Vector3(1, 1, 0) * (mainCamera.orthographicSize * 2 / Screen.height);
                transform.parent = currentTransform;
            }
            else
            {
                transform.localScale = Vector3.one;//*
                rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height) * (mainCamera.orthographicSize * 2 / Screen.height);
            }
        }

        private void UpdatePosition()
        {
            if (t_point.x != mainCamera.transform.position.x || t_point.y != mainCamera.transform.position.y)
            {
                t_point = mainCamera.transform.position;
                t_point.z = transform.position.z;
                transform.position = t_point;
            }
        }


        private void Update()
        {
            UpdatePosition();

            UpdateBoarder();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Init();
            UpdatePosition();

            UpdateBoarder();
        }

#endif
    }
}