using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TankStars.Level
{
    public class DestructibleObject : MonoBehaviour
    {
        internal virtual void DestroyObject(Vector3 position, float radius, ItemData data)
        {

        }
    }
}