using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Amulay.Utility;

namespace TankStars.Level
{
    public class AI_GameplayManager : GameplayManager
    {

        [SerializeField] int _AI_TankCount = -1;
        [SerializeField, Range(0, 1f)] float _AIFactor = .7f;
        private int currentActiveTankIndex = 0;
        internal override event Action<Tank> onStartNewRound;
        internal override event Action<Tank, bool> onFinish;

        public string[] tanksname;

        public GameObject  cam;
        Camera camera;
        protected override void Awake()
        {
            base.Awake();
            currentActiveTankIndex = Random.Range(0, tanks.Count);
            Item.onDestroyAllFlyingItems += Item_onDestroyAllFlyingItems;
            camera = cam.GetComponent<Camera>();
        }

        private void Item_onDestroyAllFlyingItems(Item item)
        {
            OnStartNewRound();
        }

        private void OnStartNewRound()
        {
            currentActiveTankIndex = (currentActiveTankIndex + 1) % tanks.Count;
            try
            {
                onStartNewRound?.Invoke(tanks[currentActiveTankIndex]);
            }
            catch
            {
               // Debug.Log("OOF");
            }
        }

        private void Start()
        {
            Invoker.Do(tanks.Count * 1.2f, () => onStartNewRound?.Invoke(tanks[currentActiveTankIndex]));
        }

        protected override void InstantiateTanks()
        {
            Tank[] tanksPrefabs = GameManager.instance.selectedTanks;
            if (tanksPrefabs.Length < 1)
            {
                Debug.LogError("tanksPrefabs.Length < 1");
                tanksPrefabs = new Tank[] { Tank.GetPrefab("tank_yellow") };
            }

            localPlayerTank = Instantiate(tanksPrefabs[0]);
            localPlayerTank.InitInput();
            tanks.Add(localPlayerTank);

            int _AIEnemeisCount = (_AI_TankCount < 1) ? Random.Range(1, 1) : _AI_TankCount;
            List<Tank> prefabs = new List<Tank> { Tank.GetPrefab(tanksname[Random.Range(0,tanksname.Length)])/*, Tank.GetPrefab("tank_green")*/ };
            for (int i = 0; i < _AIEnemeisCount; i++)
            {
                var tank = Instantiate(prefabs[Random.Range(0,prefabs.Count)]);
                var ai = tank.gameObject.AddComponent<AIInput>();
                AIHealthBar.Create(tank);
                ai.SetAIFactor(.7f);
                tank.InitInput(ai);
                tanks.Add(tank);
            }

            //set tank start postions
            {
                int count = tanks.Count;
                if (count < 2)
                    throw new Exception();

                float range = 60f;
                float step = range / (count - 1);
                float startPosition = -range * .5f;
                Vector2 offset = Vector3.up * .5f;

                for (int i = 0; i < count; i++)
                {
                    var hit = Physics2D.Raycast(new Vector2(startPosition + i * step, 10), Vector2.down);
                    if (hit.collider == null)
                        throw new Exception();
                    tanks[i].transform.position = hit.point + offset;
                }
            }


            foreach (var item in tanks)
            {
                var t_tank = item;
                t_tank.onGetDamage += (a, b, c) => {
                    if (a <= 0)
                        OnTankDestroyed(t_tank);
                };
            }

            //foreach (var tank in tanks)
            //    tank.onShoot += (a, b, c, d, f) => Tank_onShoot(tank, a, b, c, d, f);
        }

        private void OnTankDestroyed(Tank tank)
        {
            tanks.Remove(tank);
            if (tank == localPlayerTank)
            {
                onFinish?.Invoke(null, false);
                onFinish = null;

            }
            else if (tanks.Count <= 1)
            {
                onFinish?.Invoke(localPlayerTank, true);
                onFinish = null;
                print(tank.name);

            }
            cam.transform.DOMoveX(tank.transform.position.x,0.1f);
            camera.DOOrthoSize(10, 0.5f);
            
            //else
            //{
            //    tanks.Remove(tank);
            //}
        }


        protected override void OnDestroy()
        {
            onStartNewRound = null;
            onFinish = null;
            base.OnDestroy();
        }
    }
}