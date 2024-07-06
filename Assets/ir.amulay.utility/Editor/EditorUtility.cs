#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Xml.Serialization;

namespace Amulay.UtilityEditor 
{
    public static class EditorUtility
    {
        [MenuItem("CONTEXT/RectTransform/SetAnchors")]
        public static void SetAnchors(MenuCommand command)
        {
            RectTransform transform = command.context as RectTransform;
            RectTransform parent = transform.parent as RectTransform;

            if (transform == null || parent == null) return;

            Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / parent.rect.width,
                                                transform.anchorMin.y + transform.offsetMin.y / parent.rect.height);
            Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / parent.rect.width,
                                                transform.anchorMax.y + transform.offsetMax.y / parent.rect.height);

            transform.anchorMin = newAnchorsMin;
            transform.anchorMax = newAnchorsMax;
            transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
        }

        [MenuItem("GameObject/UI/ScrollPlus (UI)")]
        public static void ScrollPlus(MenuCommand command)
        {
            Transform t = null;
            if (command.context != null)
                t = (command.context as GameObject).transform;
            Amulay.Utility.ScrollerPlus.Create(t);
        }
    }
}
#endif
