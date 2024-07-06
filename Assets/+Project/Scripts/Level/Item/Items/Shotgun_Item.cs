using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TankStars.Level
{
    public class Shotgun_Item : Item
    {
        [Header("Shotgun"), Space(10)]
        [SerializeField, Range(2, 10)]      private int     instanceCount   = 5;
        [SerializeField, Range(.05f, 10f)]  private float   deltaAim        = 2;

        internal override void Shoot(Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                var item = Clone() as Shotgun_Item;
                item.InternalShoot(firePoint, aim + deltaAim * (i - ((instanceCount - 1) / 2f)), powerWithWeightFactor, onFinished);
            }
        }
    }
}