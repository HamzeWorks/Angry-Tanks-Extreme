using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankStars.Network;
using Colyseus;
using System;

namespace TankStars.Level
{
    public class NetworkLocalInputOutput : LocalInput
    {

        private string id = "";
        private new Tank connectedTank;
        private Room<BattleState> room;

        private const float pingInterval = .1f;
        private IntervalMessage movementIntervalController;

        internal NetworkLocalInputOutput Init(Room<BattleState> room, PlayerSchema player, Tank tank)
        {
            this.room = room;
            connectedTank = tank;
            id = player.id;
            //connectedTank.onShoot += ConnectedTank_onShoot;
            room.State.lastShoot.OnChange += LastShoot_OnChange;
            connectedTank.onMovement += ConnectedTank_onMovement;
            connectedTank.onGetDamage += ConnectedTank_onGetDamage;
            Item.onDestroyAllFlyingItems += Item_onDestroyAllFlyingItems;
            movementIntervalController = new IntervalMessage(pingInterval, (message) =>
            {
                print("Movement Message");
                room.Send("MovementMessage", message);
            });
            return this;
        }

        private void Item_onDestroyAllFlyingItems(Item item)
        {
            if (room.State.activePlayerId != room.SessionId)
                return;
            if (room.State.lastShoot.playerId != room.SessionId)
                return;
            var msg = new ExplosionMessage()
            {
                item = new ItemSchema { name = item.data.itemName, level = item.data.level },
                point = item.transform.position.ToVec2(),
            };

            print("ExplosionMessage");
            room.Send("ExplosionMessage", msg);
        }

        private void ConnectedTank_onGetDamage(float arg1, float arg2, Item arg3)
        {
            var msg = new GetDamageMessage()
            {
                currentHealth = arg1,
                damage = arg2,
            };

            room.Send("GetDamageMessage", msg);
        }

        private void ConnectedTank_onShoot(Item item, Vector3 firePoint, float aim, float power, System.Action callback)
        {
            var itemMsg = new ItemSchema
            {
                name = item.data.itemName,
                level = item.data.level,
            };

            var msg = new ShootMessage()
            {
                aim = aim,
                power = power,
                item = itemMsg,
                firePoint = firePoint.ToVec2(),
            };

            //callback += Item_onDestroyAllFlyingItems;

            room.Send("ShootMessage", msg);
        }

        private void ConnectedTank_onMovement(Vector3 position, float aim, float power)
        {
            var msg = new MovementMessage()
            {
                aim = aim,
                power = power,
                tankPosition = position.ToVec2(),
                tankRotaion = connectedTank.transform.rotation.ToVec4(),
            };
            movementIntervalController.currentValue = msg; //TODO:cache this in intervalClass
        }

        protected override void Update()
        {
            base.Update();
            movementIntervalController.CheckUpdate();
        }

        private void LastShoot_OnChange(List<Colyseus.Schema.DataChange> changes)
        {
            var shoot = room.State.lastShoot;
            if (shoot.playerId != id)
                return;

            //onForceMovement?.Invoke(shoot.movement.tankPosition.ToVector3(), shoot.movement.tankRotaion.ToQuaternion(), shoot.movement.aim, shoot.movement.power);
            //Item.ShootItem(shoot.item.name, shoot.firePoint.ToVector3(), shoot.aim, shoot.power, null);
            connectedTank.ForceShoot(shoot);
        }

        protected override void FireButtonClick()
        {
            var item = connectedTank.selectedItem;
            var itemMsg = new ItemSchema
            {
                name = item.data.itemName,
                level = item._level,
            };

            var msg = new ShootMessage()
            {
                aim = connectedTank.currentAim,
                power = connectedTank.currentShootPowerWithWeightFactor,
                item = itemMsg,
                firePoint = connectedTank.firePointPosition.ToVec2(),
            };

            //callback += Item_onDestroyAllFlyingItems;

            print("sssssshoot  " + itemMsg.level);
            room.Send("ShootMessage", msg);
        }
    }
}

public class IntervalMessage
{
    public object currentValue = null;
    public float pingInterval = .1f;
    public Action<object> action;
    private object lastSendedValue = null;
    private float lastSendedValueTime = float.MinValue;

    public IntervalMessage(float pingInterval, Action<object> action)
    {
        this.pingInterval = pingInterval;
        this.action = action;
    }

    public void CheckUpdate()
    {
        if (Time.unscaledTime > lastSendedValueTime + pingInterval)
        {
            if (currentValue != lastSendedValue)
            {
                lastSendedValueTime = Time.unscaledTime;
                lastSendedValue = currentValue;
                action?.Invoke(lastSendedValue);
            }
        }
    }
}