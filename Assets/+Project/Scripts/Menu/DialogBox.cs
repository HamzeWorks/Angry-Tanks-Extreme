using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

namespace TankStars
{
    public class DialogBox : MonoBehaviour
    {
        private const string resourcesPath = "[ DialogBox ]";
        [SerializeField] CanvasGroup canvas;
        //[SerializeField] AssetReference prefabRef;
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] TextMeshProUGUI mainText;
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI buttonText;

        private Action<DialogBox> callback;
        private static bool t = false;
        private void Awake()
        {
            button.onClick.AddListener(ButtonOnClick);
        }

        private void ButtonOnClick()
        {
            this.callback?.Invoke(this);
        }

        public static DialogBox Create()
        {
            var prefab = Resources.Load<DialogBox>(resourcesPath);
            var dialogbox = Instantiate(prefab);
            return dialogbox;
        }

        //public static async Task<DialogBox> CreateAsync()
        //{
        //    var gameObject = await Addressables.InstantiateAsync(resourcesPath).Task;
        //    return gameObject.GetComponent<DialogBox>();
        //}

        public static DialogBox Create(string header, string text, string buttonText, Action<DialogBox> callback)
        {
            var dialogbox = Create();
            dialogbox.Open(header, text, buttonText, callback);
            return dialogbox;
        }

        public DialogBox Open(string header, string text, string buttonText, Action<DialogBox> callback)
        {
            PlayOpenAnimation();
            headerText.text = header;
            mainText.text = text;
            this.buttonText.text = buttonText;
            this.callback = callback;
            return this;
        }

        private void PlayOpenAnimation()
        {
            canvas.DOKill();
            canvas.transform.DOKill();
            canvas.alpha = 0;
            canvas.transform.localScale = Vector3.one;
            canvas.DOFade(1, .2f);
            canvas.transform.DOPunchScale(new Vector3(.1f, .1f), .3f, 3);
        }

        public void Close()
        {
            if(gameObject!=null)
               Destroy(gameObject);
        }

    }
}