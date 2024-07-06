using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace TankStars.Level
{
    public class LevelMenu : MonoBehaviour
    {
        [SerializeField] CanvasGroup panel;
        [SerializeField] Image touchBloker;
        [SerializeField] Button backToMenuButton;
        [SerializeField] Button openPanelButton;
        [SerializeField] Button resumeButton;
        [SerializeField]
        Button soundmngr;
        [SerializeField]
        Button musicmngr;
        [SerializeField]
        AudioSource soundkeeper;
        [SerializeField]
        AudioSource musickeeper;

        [SerializeField]
        Sprite[] musics;
        [SerializeField]
        Sprite[] sounds;

        static public bool soundon = true;
        bool musicon = true;

        [SerializeField]
        GameObject backblack;

        private void Awake()
        {
            openPanelButton.onClick.AddListener(() =>
            {
                touchBloker.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
                Time.timeScale = 0;
            });
            /*
            resumeButton.onClick.AddListener(() =>
            {
                touchBloker.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                Time.timeScale = 1;
            });

            backToMenuButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                GameManager.LoadLevel(SceneEnum.MainMenu);
            });
            */
        }

        public void SoundOnOff()
        {
            if (!soundon)
            {
                soundkeeper.gameObject.SetActive(true);
                soundmngr.image.sprite = sounds[0];
                soundon = true;

            }else
            {
                soundkeeper.gameObject.SetActive(false);
                soundmngr.image.sprite = sounds[1];
                soundon = false;

            }
        }
        public void MusicOnOff()
        {
            if (!musicon)
            {
                musickeeper.Play();
                musicmngr.image.sprite = musics[0];
                musicon = true;
            }else
            {
                musickeeper.Pause();
                musicmngr.image.sprite = musics[1];
                musicon = false;
            }
        }
        public void BackToMenu()
        {
            Application.LoadLevel("0_MainMenu");
        }
        public void Resume()
        {
            touchBloker.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}