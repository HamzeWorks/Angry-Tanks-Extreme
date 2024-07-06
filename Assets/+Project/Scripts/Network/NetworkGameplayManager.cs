using Colyseus;
using System;
using System.Collections.Generic;
using TankStars.Level;
using TankStars.Network;

namespace TankStars
{
    public class NetworkGameplayManager : GameplayManager
    {
        private Room<BattleState> room;
        public Dictionary<string, Tank> playerTanks = new Dictionary<string, Tank>();
        private int roundIndex = -1;

        internal override event Action<Tank> onStartNewRound;
        internal override event Action<Tank, bool> onFinish;

        protected override void Awake()
        {
            InitRoom();
            base.Awake();
        }

        private void InitRoom()
        {
            room = NetworkManager.instance.room;
            room.OnStateChange += Room_OnStateChange;
            room.OnJoin += Room_OnJoin;
            room.OnLeave += Room_OnLeave;
            room.Connection.OnOpen += Connection_OnOpen;
            room.Connection.OnClose += Connection_OnClose;
            room.OnError += Room_OnError;
        }

        private void Room_OnError(int code, string message)
        {
            throw new System.NotImplementedException();
        }

        protected override void InstantiateTanks()
        {
            room.State.players.ForEach((str, player) =>
            {
                string tankName = player.tank.name;
                if (string.IsNullOrEmpty(tankName))
                    tankName = "tank_green";

                var tank = Instantiate(Tank.GetPrefab(tankName));
                tanks.Add(tank);
                playerTanks.Add(player.id, tank);

                //set input
                if (IslocalPlayer(player))
                {
                    localPlayerTank = tank;
                    var input = (NetworkLocalInputOutput)LocalInput.instance;
                    input.Init(room, player, tank);
                    localPlayerTank.InitInput(input);
                    //localPlayerTank.gameObject.AddComponent<NetworkTankOutput>().Init(room, localPlayerTank);
                }
                else
                {
                    IInputBase input = tank.gameObject.AddComponent<RemoteInput>().Init(room, player, tank);
                    tank.InitInput(input);
                }
            });
            print("Tanks Created");
            SetTankPositions();
        }

        private void Room_OnStateChange(BattleState state, bool isFirstState)
        {
            if (state.started && roundIndex != state.roundIndex)
            {
                print("state.started  " + state.started + " - " + state.activePlayerId + "   state.roundIndex: "+ state.roundIndex);
                roundIndex = (int)state.roundIndex;
                var activePlayertank = playerTanks[state.activePlayerId];
                onStartNewRound?.Invoke(activePlayertank);
            }

            if (state.finished)
            {
                //because call just once time
                if (onFinish != null) 
                { 
                    bool win = (state.winnderPlayerId == room.SessionId);
                    var winTank = (win) ? localPlayerTank : tanks[(tanks.IndexOf(localPlayerTank) + 1) % tanks.Count];
                    onFinish?.Invoke(winTank, win);
                    onFinish = null;
                }
            }
        }

        private void Connection_OnOpen()
        {
            throw new System.NotImplementedException();
        }

        private void Connection_OnClose(NativeWebSocket.WebSocketCloseCode closeCode)
        {
            throw new System.NotImplementedException();
        }

        private void Room_OnJoin()
        {
            throw new System.NotImplementedException();
        }

        private void Room_OnLeave(NativeWebSocket.WebSocketCloseCode code)
        {
            throw new System.NotImplementedException();
        }

   
        bool IslocalPlayer(PlayerSchema player) => player.id == room.SessionId;
    }
}