using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using System.Threading.Tasks;
using Amulay.Utility;

namespace TankStars.Network
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        #region variables
        [SerializeField] private string url = "ws://localhost:2567";
        public Room<BattleState> room { get; private set; }

        private Action<Room<BattleState>> roomStartedCallback;
        #region events
        //internal static event Action<bool, PlayerData> onRegister;
        //internal static event Action<bool> onConnected;
        #endregion

        #region properties
        public Client Client { get; private set; }
        public List<IRoom> rooms  { get; private set; } = new List<IRoom>();
        #endregion
        #endregion

        protected override void Awake()
        {
            base.Awake();
            if (instance != this)
                return;
            DontDestroyOnLoad(instance.gameObject);
            CreateClient(url);
        }

        public async Task<Room<T>> JoinOrCreateRoom<T>(string roomName, Dictionary<string, object> options = null, Dictionary<string, string> headers = null,Action<Room<T>> callback = null)
        {
            try
            {
                
                var room = await Client.JoinOrCreate<T>(roomName, options, headers);
              
                room.OnLeave += (code) => rooms.Remove(room);
                rooms.Add(room);
                if (room is Room<BattleState>)
                {
                    CheckRoom(room as Room<BattleState>);
                    roomStartedCallback = callback as Action<Room<BattleState>>;
                }
                return room;
            }
            catch (Exception ex)
            {
                Debug.Log("CheckNetworkConnection: " + ex.Message);
                return null;
            }
        }

        public async Task<Room<T>> Join<T>(string roomName, Dictionary<string, object> options = null, Dictionary<string, string> headers = null, Action<Room<T>> callback = null)
        {
            try
            {
                var room = await Client.Join<T>(roomName, options, headers);

                room.OnLeave += (code) => rooms.Remove(room);
                rooms.Add(room);
                if (room is Room<BattleState>)
                {
                    CheckRoom(room as Room<BattleState>);
                    roomStartedCallback = callback as Action<Room<BattleState>>;
                }
                return room;
            }
            catch (Exception ex)
            {
                Debug.Log("CheckNetworkConnection: " + ex.Message);
                return null;
            }
        }

        private void CheckRoom(Room<BattleState> room)
        {
            if (this.room != null)
                room.State.OnChange -= State_OnChange;//*

            this.room = room;
            room.State.OnChange += State_OnChange;
        }

        private void State_OnChange(List<Colyseus.Schema.DataChange> changes)
        {
            if (room.State.ready)
            {
                room.State.OnChange -= State_OnChange;
                roomStartedCallback?.Invoke(room);
                roomStartedCallback = null;
            }
        }

        private Client CreateClient(string endpoint)
        {
            Client = new Client(endpoint);
            return Client;
        }

        public void LeveAllRoom()
        {
            if (Client != null)
            {
                foreach (var room in rooms)
                {
                    room.Leave(false);
                }
            }
        }

        //private async Task AddRoom(IRoom room)
        //{
        //    room.OnLeave += (code) => rooms.Remove(room);
        //    rooms.Add(room);
        //    await room.Connect();
        //}

        private void OnApplicationQuit()
        {
            if (Client != null)
            {
                foreach (var room in rooms)
                {
                    room.Leave(false);
                }
            }
        }
    }
}