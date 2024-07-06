using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Amulay.Utility
{
    public class MouseEventHandler : MonoBehaviour
    {
        public event Action<MouseEventHandler> onMouseDown;
        public event Action<MouseEventHandler> onMouseUp;
        public event Action<MouseEventHandler> onMouseUpAsButton;
        public event Action<MouseEventHandler> onMouseDrag;

        public void SetEnableCollider(bool enable)
        {

            if (TryGetComponent<Collider2D>(out Collider2D collider2d))
                collider2d.enabled = enable;
            else if (TryGetComponent<Collider>(out Collider collider))
                collider.enabled = enable;
        }

        private void OnMouseDown() { if (enabled) onMouseDown?.Invoke(this); }
        private void OnMouseUp() { if (enabled) onMouseUp?.Invoke(this); }
        private void OnMouseUpAsButton() { if (enabled) onMouseUpAsButton?.Invoke(this); }
        private void OnMouseDrag() { if (enabled) onMouseDrag?.Invoke(this); }
    }
}