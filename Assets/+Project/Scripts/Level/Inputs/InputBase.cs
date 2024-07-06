using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TankStars.Level
{
    internal interface IInputBase
    {
        /// <summary>
        /// aim and power
        /// </summary>
        event Action<float, float> onAimInput;
        event Action<float> onMoveInput;
        event Action onShoot;
        event Action<Item> onSelectItem;
        /// <summary>
        /// postiion , rotaion , aim, power
        /// </summary>
        event Action<Vector3,Quaternion,float,float> onForceMovement;
    }
}