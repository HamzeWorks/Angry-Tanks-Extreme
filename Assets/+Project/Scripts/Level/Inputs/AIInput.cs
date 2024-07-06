using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TankStars.Level;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Linq;
using Amulay.Utility;


namespace TankStars.Level
{
    public class AIInput : MonoBehaviour, IInputBase
    {
        public event Action<float, float> onAimInput;
        public event Action<float> onMoveInput;
        public event Action onShoot;
        public event Action<Item> onSelectItem;
        public event Action<Vector3, Quaternion, float, float> onForceMovement;

        [SerializeField] Tank connectedTank;

        [SerializeField, Range(.0f, 1f)]  float AIFactor = .5f;
        [SerializeField, Range(.0f, 1f)]  float maxPowerOffset = 1f;
        [SerializeField, Range(.0f, 50f)] float maxPositionOffset = 15f;

        private void Awake()
        {
            if (connectedTank == null)
                connectedTank = GetComponent<Tank>();
            GameplayManager.instance.onStartNewRound += GameplayManager_onStartNewRound;
            connectedTank.onGetDamage += ConnectedTank_onGetDamage;
        }

        private void ConnectedTank_onGetDamage(float arg1, float arg2, Item arg3)
        {
            if (arg1 <= 0)
            {
                connectedTank.onGetDamage -= ConnectedTank_onGetDamage;
            }
        }

        private void GameplayManager_onStartNewRound(Tank obj)
        {
            if (obj != connectedTank)
                return;
            var enemies = GameplayManager.instance.tanks.ToList<Tank>();
            enemies.Remove(connectedTank);
            var enemy = enemies[Random.Range(0, enemies.Count)];
            enemy = GameplayManager.instance.localPlayerTank;//*
            StartCoroutine(StartAction(enemy, AIFactor, .7f));
        }

        private IEnumerator StartAction(Tank target, float AIFactor, float delay = 1)
        {
            AIFactor    = Mathf.Clamp(AIFactor  , 0, 1);
            delay       = Mathf.Clamp(delay     , 0, 1);

            yield return new WaitForSeconds(1.5f * delay);

            var delta = target.transform.position - connectedTank.transform.position;
            int direction = Math.Sign(delta.x);
            var distance = delta.magnitude;
            var offset = (1 - AIFactor) * Random.insideUnitCircle * maxPositionOffset;
            var powerOffset = (1 - AIFactor) * Random.value * maxPowerOffset;
            var power = Mathf.Clamp01(1 - powerOffset);
            var targetPosition = target.transform.position + (Vector3)offset;

            //fix tank face
            onAimInput((delta.x > 0) ? 45 : 45 + 90, 1);

            var inRange = AimPath.CalculateAngle(connectedTank.transform.position, target.transform.position, connectedTank.currentShootPowerWithWeightFactor, out float angle);

            if (inRange)
            {
                //if is too near then go far
                if (distance < 3)
                {
                    //onMoveInput(-direction);
                    float moveDutaion = 3 + 1 * delay;
                    float moveInput = Random.Range(.6f, 1f) * ((Random.value > .5f) ? -1 : 1);

                    if (GroundManager.instance.groundBounds.Contains(connectedTank.transform.position) == false)
                    {
                        moveInput = Random.Range(.6f, 1f) * Mathf.Sign(GroundManager.instance.groundBounds.center.x - connectedTank.transform.position.x);
                    }

                    Invoker.DoUpdate(moveDutaion, () => onMoveInput(moveInput));
                    yield return new WaitForSeconds(moveDutaion + 1 * delay);

                }
                else
                {
                    //random movement
                    if (Random.value > .3f)
                    {
                        float moveDutaion = 2 + 1 * delay;
                        float moveInput = Random.Range(.6f, 1f) * ((Random.value > .5f) ? -1 : 1);

                        if (GroundManager.instance.groundBounds.Contains(connectedTank.transform.position) == false)
                        {
                            moveInput = Random.Range(.6f, 1f) * Mathf.Sign(GroundManager.instance.groundBounds.center.x - connectedTank.transform.position.x);
                        }

                        Invoker.DoUpdate(moveDutaion, () => onMoveInput(moveInput));
                        yield return new WaitForSeconds(moveDutaion + 1 * delay);
                    }
                }
            }
            else
            {
                //is too far then go close
                Invoker.DoUpdate(3, () => onMoveInput(direction));
                yield return new WaitForSeconds(3f);
            }

            inRange = AimPath.CalculateAngle(connectedTank.transform.position, targetPosition, connectedTank.currentShootPowerWithWeightFactor, out angle);
            if (inRange == false)
                angle = (delta.x > 0) ? 45 : 45 + 90;

            float animationDuration = 1 * delay + 1;
            float t_angleAnimation = (delta.x > 0) ? 45 : 45 + 90;
            DOTween.To(() => t_angleAnimation, (x) => { onAimInput(x, power); }, angle, animationDuration).SetEase(Ease.OutElastic);
            yield return new WaitForSeconds(animationDuration + 1 * delay);

            onAimInput(angle, power);
            onShoot();
        }

        internal void SetAIFactor(float value)
        {
            AIFactor = Mathf.Clamp01(value);
        }
    }
}

/*
 private async void Start()
    {
        if (connectedTank == null)
            connectedTank = GetComponent<Tank>();

        target = GameplayManager.instance.localPlayerTank;

        var delta = target.transform.position - connectedTank.transform.position;
        onAimInput((delta.x > 0) ? 45 : 45 + 90, 1);

        await Task.Delay(2000);
        var inRnage = AimPath.CalculateAngle(connectedTank.transform.position, target.transform.position, connectedTank.currentShootPowerWithWeightFactor, out float angle);
        if(inRnage)
        {
            onAimInput(angle, 1);
            await Task.Delay(2000);
            onShoot();
        }
    }
 */