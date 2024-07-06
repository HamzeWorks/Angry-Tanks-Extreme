using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TankStars.Level;
using TankStars.Network;
using System.Threading.Tasks;
using Colyseus;
using Colyseus.Schema;
using Amulay.Utility;

namespace TankStars
{
    public class GameManager : Singleton<GameManager> 
    {
        public Tank[] selectedTanks { get; private set; }
        public static SceneEnum currentScene
        {
            get 
            {
                int value = SceneManager.GetActiveScene().buildIndex;
                value = Mathf.Min(value , Enum.GetValues(typeof(SceneEnum)).Length - 1);
                return (SceneEnum)value;
            } 
        }

        private DialogBox t_dialog;
        private Room<BattleState> t_room = null;

        //*
        private int m_coin;
        public int _coin
        {
            get => PlayerPrefs.GetInt("coin", 250);

            set
            {
                if (value == _coin)
                    return;
                PlayerPrefs.SetInt("coin", value);
                onCoinCountChanged?.Invoke(value);
            }
        }

        public static int unlockedLevelIndex
        {
            get => PlayerPrefs.GetInt("unlockedLevelIndex", 1);
            set => PlayerPrefs.SetInt("unlockedLevelIndex", value);
        }

        /// <summary>
        /// current value , added value
        /// </summary>
        public event Action<int> onCoinCountChanged;
      
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            Application.targetFrameRate = 30;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Time.timeScale = 1;

        }
        public async Task<bool> SetGameMode(SceneEnum mode, int index = 0)
        {
            var success = await LoadLevel(mode, index, selectedTanks);
            return success;
        }
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {

        }

        private void ResetMenuVariables()
        {

        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {

        }

        internal void SelectTank(params Tank[] tanks)
        {
            selectedTanks = tanks;
        }

        public static async Task<bool> LoadLevel(SceneEnum scene,int index = 0, Tank[] tanks = null, Action<bool> callback = null)
        {
            instance.selectedTanks = tanks;

            //TODO: fix this
            if (currentScene == SceneEnum.Level_Online_2P && scene==SceneEnum.MainMenu)
                Network.NetworkManager.instance.LeveAllRoom();

            switch (scene)
            {
                case SceneEnum.MainMenu:
                    SceneManager.LoadScene((int)scene);
                    break;
                case SceneEnum.Level_AI:
                    SceneManager.LoadScene("1_Level_AI_1");
                    break;
                //case SceneEnum.Level_VS:
                //    break;
                case SceneEnum.Level_Online_2P:
                    Invoker.Action_Duration autoPlayWithComputer = null;
                    DialogBox dialogBox;





                    if (index == 0)
                    {
                        dialogBox = DialogBox.Create("Matchmaking", "Wait for player", "Cancel", async (dialog) => {
                            if (instance.t_room == null || instance.t_room.State == null)
                                return;
                            if (instance.t_room.State.ready)
                                return;
                            try
                            {
                                await instance.t_room.Leave();
                                dialog.Close();
                                autoPlayWithComputer?.Stop();
                            }
                            catch
                            {
                                print("dialogBox catch");
                            }
                        });

                        var room = await NetworkManager.instance.JoinOrCreateRoom<BattleState>("battle", callback: instance.LoadNetworkRoomAfterRoomIsReady);
                        if (room == null)
                        {
                            instance.t_room = room;
                            dialogBox.Open("Matchmaking", "Matchmaking failed.\nCheck network connection", "Close", (d) => d.Close());
                            return false;
                        }
                        instance.SendSetupMessage(room);
                        instance.t_room = room;
                    }
                    else
                    if (index == -1)
                    {
                        var dic = new Dictionary<string, object>();
                        string password = UnityEngine.Random.Range(1000, 10000).ToString();
                        dic.Add("password", password);

                        dialogBox = DialogBox.Create("Matchmaking", $"Wait for player\n the password is \"{password}\"", "Cancel", async (dialog) => {
                            if (instance.t_room == null || instance.t_room.State == null)
                                return;
                            if (instance.t_room.State.ready)
                                return;
                            try
                            {
                                await instance.t_room.Leave();
                                dialog.Close();
                                autoPlayWithComputer?.Stop();
                            }
                            catch
                            {
                                print("dialogBox catch");
                            }
                        });

                      
                        var room = await NetworkManager.instance.JoinOrCreateRoom<BattleState>("battle_private", dic, callback: instance.LoadNetworkRoomAfterRoomIsReady);
                        if (room == null)
                        {
                            instance.t_room = room;
                            dialogBox.Open("Matchmaking", "Matchmaking failed.\nCheck network connection", "Close", (d) => d.Close());
                            return false;
                        }
                        instance.SendSetupMessage(room);
                        instance.t_room = room;
                    }
                    else
                    {
                        dialogBox = DialogBox.Create("Matchmaking", "Wait for player", "Cancel", async (dialog) => {
                            if (instance.t_room == null || instance.t_room.State == null)
                                return;
                            if (instance.t_room.State.ready)
                                return;
                            try
                            {
                                await instance.t_room.Leave();
                                dialog.Close();
                                autoPlayWithComputer?.Stop();
                            }
                            catch
                            {
                                print("dialogBox catch");
                            }
                        });

                        var dic = new Dictionary<string, object>();
                        string password = index.ToString();
                        dic.Add("password", password);
                        var room = await NetworkManager.instance.Join<BattleState>("battle_private", dic, callback: instance.LoadNetworkRoomAfterRoomIsReady);
                        if (room == null)
                        {
                            instance.t_room = room;
                            dialogBox.Open("Matchmaking", "Matchmaking failed.", "Close", (d) => d.Close());
                            return false;
                        }
                        instance.SendSetupMessage(room);
                        instance.t_room = room;
                    }

                    //index == 0 -> if is public online player
                    //after 20f cancel search and load ai scene
                    if (index == 0)
                        autoPlayWithComputer = Invoker.Do(20f, () =>
                        {
                            instance.t_room.Leave();
                            dialogBox.Close();
                            LoadLevel(SceneEnum.Level_AI, 0, tanks);
                        });
                    break;
                //case SceneEnum.Level_Online_4P:
                //    break;
                default:
                    break;
            }

            return true;
        }

        private void LoadNetworkRoomAfterRoomIsReady(Room<BattleState> room)
        {
            if (room == null)
                return;
            SceneManager.LoadScene((int)SceneEnum.Level_Online_2P);
        }

        private async void SendSetupMessage(Room<BattleState> room)
        {
            if (room == null)
                throw new System.NullReferenceException();

            var items = new ArraySchema<ItemSchema>();

            { 
                int i = 0;
                foreach (var tt in selectedTanks[0]._defaultItems)
                {
                    Debug.Log("Name = " + tt.itemName());
                    items[i] = new ItemSchema { name = tt.itemName(), level = tt.data.level };
                    i++;
                }
            }

            var tankData = new TankSchema
            {
                name = selectedTanks[0].name,
                level = 1,
                items = items,

                //aim = localPlayerTank.currentAim,
                //power = localPlayerTank.currentShootPowerWithWeightFactor,
                //health = 100,
                //position = localPlayerTank.transform.position.ToVec2(),
                //rotaion = localPlayerTank.transform.rotation.ToVec4(),
            };
            var msg = new SetupMessage { tankName = selectedTanks[0].name, tank = tankData };
            Debug.Log("send setupmessage");
            await room.Send("SetupMessage", msg);
        }
    }

  public enum SceneEnum
    {
        MainMenu = 0,
        Level_Online_2P = 1,
        Level_AI = 2, 
        //Level_Online_4P = 2,
        //Level_VS = 3,
    }
}