using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankStars.Level
{
    public class MilitarRradio : Singleton<MilitarRradio>
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] sounds;

        private void Start()
        {
            GameplayManager.instance.onStartNewRound += GameplayManager_onStartNewRound;
        }

        private void GameplayManager_onStartNewRound(Tank obj)
        {
            audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)], audioSource.volume);
        }

        protected override void OnDestroy()
        {
            try
            {
                GameplayManager.instance.onStartNewRound -= GameplayManager_onStartNewRound;
            }
            catch
            {

            }
        }
    }
}