using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TankStars.Level
{
    public class SplitterChain : Item
    {
        [Header("SplitterChain"), Space(10f)]
        [SerializeField, Range(1, 4)] int startInstanceCount = 2;
        [SerializeField, Range(1, 4)] int instanceCount = 2;
        [SerializeField, Range(.05f, 10f)] private float deltaAim = 2;

        internal override void Shoot(Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            for (int i = 0; i < startInstanceCount; i++)
            {
                var item = Clone() as SplitterChain;
                item.InternalShoot(firePoint, aim + deltaAim * (i - ((instanceCount - 1) / 2f)), powerWithWeightFactor, onFinished);
            }
        }
    }
}