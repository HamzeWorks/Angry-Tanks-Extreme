using Amulay.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankStars.Level
{
    public class Granade_Item : Item
    {
        [Header("Granade"), Space(10f)]
        [SerializeField, Range(1f, 15f)] float explotionDelay = 5f;
        [SerializeField, Range(0, 30f)] float rotaionForce = 5f; 


        protected override void InternalShoot(Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            base.InternalShoot(firePoint, aim, powerWithWeightFactor, onFinished);
            rotaionUpdate = false;
            rigidbody.AddTorque(-rotaionForce, ForceMode2D.Impulse);
            Invoker.DoCoroutine(explotionDelay, () => base.OnCollisionEnter2D(null));
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            //base.OnCollisionEnter2D(collision);
        }
    }


}