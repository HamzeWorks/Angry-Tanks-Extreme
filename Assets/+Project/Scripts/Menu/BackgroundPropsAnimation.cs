using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TankStars.Menu
{
    public class BackgroundPropsAnimation : MonoBehaviour
    {
        [SerializeField] Animator fanAnimator;
        [SerializeField] ParticleSystem fanParticle;
        [SerializeField] Animation lightAnimator;
        [SerializeField] GameObject lampLight;

        private void Start()
        {
            FanParticleAnimation();
            StartCoroutine(LightParticleAnimation());
        }

        private IEnumerator LightParticleAnimation()
        {
            do
            {
                yield return new WaitForSeconds(Random.Range(7, 15));
                lightAnimator.Play();
            } while (true);
        }

        private void FanParticleAnimation()
        {
            Invoker.DoCoroutine(Random.Range(7, 15), () => { fanParticle.Play(true); FanParticleAnimation(); });
        }
    }
}