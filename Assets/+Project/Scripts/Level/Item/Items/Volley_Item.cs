using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Amulay.Utility;

namespace TankStars.Level
{
    public class Volley_Item : Item
    {
        [Header("Volley"), Space(10)]
        [SerializeField, Range(2, 10)] private int instanceCount = 5;
        [SerializeField, Range(.05f, 2f)] private float delay =.3f;

        internal override void Shoot(Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            for (int i = 0; i < instanceCount; i++)
                Invoker.DoCoroutine(delay * i, () => Shooting(firePoint, aim, powerWithWeightFactor, onFinished));
        }

        private void Shooting(Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                var item = Clone() as Volley_Item;
                item.InternalShoot(firePoint, aim, powerWithWeightFactor, null);
            }
        }
    }
}