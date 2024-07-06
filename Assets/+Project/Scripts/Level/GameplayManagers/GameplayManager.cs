using Amulay.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TankStars.Level
{
    public class GameplayManager : Singleton<GameplayManager>
    {

        public Tank localPlayerTank { get; protected set; }
        public List<Tank> tanks { get; protected set; } = new List<Tank>();


        /// <summary>
        /// tank is activePlayer
        /// </summary>
        internal virtual event Action<Tank> onStartNewRound;
        /// <summary>
        /// tank , win
        /// </summary>
        internal virtual event Action<Tank,bool> onFinish;

        protected override void Awake()
        {
            base.Awake();
            InstantiateTanks();
        }

        protected virtual void InstantiateTanks()
        {
            Tank[] tanksPrefabs = GameManager.instance.selectedTanks;
            localPlayerTank = Instantiate(tanksPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity);
            localPlayerTank.InitInput();
            tanks.Add(localPlayerTank);

            //create ai tanks
            {
                int _AIEnemeisCount = 1;
                List<Tank> prefabs = new List<Tank> { Tank.GetPrefab("tank_blue")/*, Tank.GetPrefab("tank_green") */};

                for (int i = 0; i < _AIEnemeisCount; i++)
                {
                    var tank = Instantiate(prefabs[0]);
                    var ai = tank.gameObject.AddComponent<AIInput>();
                    ai.SetAIFactor(.7f);
                    tank.InitInput(ai);
                    tanks.Add(tank);
                }
            }

            SetTankPositions();
        }

        protected void SetTankPositions()
        {
            //set tank start postions
            int count = tanks.Count;

            float range = 60f;
            float step = (count < 2) ? 0 : range / (count - 1);
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
    }
}