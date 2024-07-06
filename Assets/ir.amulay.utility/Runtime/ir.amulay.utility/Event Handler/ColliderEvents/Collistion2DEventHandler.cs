using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Amulay.Utility
{
    [RequireComponent(typeof(Collider2D))]
    public class Collistion2DEventHandler : MonoBehaviour
    {
        //public new Collider2D collider { get; private set; }
        public event Action<Collision2D> onCollisionEnter2D;
        public event Action<Collision2D> onCollisionStay2D;
        public event Action<Collision2D> onCollisionExit2D;

        private void OnCollisionEnter2D(Collision2D collision) => onCollisionEnter2D?.Invoke(collision);
        private void OnCollisionStay2D(Collision2D collision) => onCollisionStay2D?.Invoke(collision);
        private void OnCollisionExit2D(Collision2D collision) => onCollisionExit2D?.Invoke(collision);

    }
}