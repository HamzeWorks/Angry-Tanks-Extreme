using Colyseus;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TankStars.Level
{
    public class RemoteInput : MonoBehaviour, IInputBase
    {
        private Room<BattleState> room;
        private string id;
        private Tank connectedTank;

        public event Action<float, float> onAimInput;
        public event Action<float> onMoveInput;
        public event Action onShoot;
        public event Action<Item> onSelectItem;
        public event Action<Vector3, Quaternion, float, float> onForceMovement;

        private Vector2 t_lastMovementPosition;
        private Quaternion t_lastMovementRotaion;
        private float t_lastMovementaim = float.NaN;
        //private float lastChange

        private void Awake()
        {
            //if(string.IsNullOrEmpty(id))
            //    id = 
        }

        public RemoteInput Init(Room<BattleState> room, PlayerSchema player,Tank tank)
        {
            this.room = room;
            this.id = player.id;
            connectedTank = tank;

            room.State.lastShoot.OnChange += LastShoot_OnChange;
            room.State.players[id].tank.OnChange += Tank_OnChange;
            GameplayManager.instance.onStartNewRound += Instance_onStartNewRound;
            tank.OverrideFuleTankSize();
            return this;
        }

        private void Instance_onStartNewRound(Tank obj)
        {
            ForceUpdateTankPostion();
        }

        private void ForceUpdateTankPostion()
        {
            var tank = room.State.players[id].tank;
            t_lastMovementaim = tank.aim;
            t_lastMovementPosition = tank.position.ToVector2();
            if (t_lastMovementaim == float.NaN || t_lastMovementPosition == Vector2.zero)
                return;
            onForceMovement?.Invoke(t_lastMovementPosition, tank.rotaion.ToQuaternion(), t_lastMovementaim, tank.power);
        }

        private void Update()
        {
            MovementUpdata();
        }

        private void MovementUpdata()
        {
            if (room.State.activePlayerId != id)
                return;
            if (t_lastMovementaim == float.NaN || t_lastMovementPosition == Vector2.zero)
                return;
            if(Mathf.Abs(connectedTank.transform.position.x - t_lastMovementPosition.x) > .1f)
            {
               float direction = (t_lastMovementPosition.x - connectedTank.transform.position.x) * 1.5f;
               onMoveInput?.Invoke(direction);
            }

            if(Mathf.Abs(t_lastMovementaim - connectedTank.currentAim) > 3f)
            {
                float value = Mathf.MoveTowards(connectedTank.currentAim, t_lastMovementaim, Time.deltaTime * 360f);
                onAimInput?.Invoke(value, 1);
            }
        }

        private void Tank_OnChange(List<Colyseus.Schema.DataChange> changes)
        {

            var tank = room.State.players[id].tank;
            t_lastMovementaim = tank.aim;
            t_lastMovementPosition = tank.position.ToVector2();
            //t_lastMovementRotaion = tank.rotaion.ToQuaternion();
            //onForceMovement?.Invoke(tank.position.ToVector2(), tank.rotaion.ToQuaternion(), tank.aim, tank.power);
        }

        private void LastShoot_OnChange(List<Colyseus.Schema.DataChange> changes)
        {
            print("last shoot data changed");
            var shoot = room.State.lastShoot;
            if (shoot.playerId != id || room.SessionId == id)
                return;

            print("shoot" +"level: " + shoot.item.level);
            //onForceMovement?.Invoke(shoot.movement.tankPosition.ToVector3(), shoot.movement.tankRotaion.ToQuaternion(), shoot.movement.aim, shoot.movement.power);
            //TODO:Use connectedTank.ForceShoot(shoot);
            Item.ShootItem(shoot.item.name, (int)shoot.item.level, shoot.firePoint.ToVector3(), shoot.aim, shoot.power, null);
        }
    }
}