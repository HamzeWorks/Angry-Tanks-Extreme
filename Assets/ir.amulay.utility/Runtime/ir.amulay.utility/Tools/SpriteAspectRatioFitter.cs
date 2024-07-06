using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Amulay.Utility
{

    //public class SpriteAspectRatioFitter : MonoBehaviour
    //{

    //}

    public static class SpriteAspectRatioFitterUtility
    {
        public static void SetSpriteWithPreviousSpriteAspect(this SpriteRenderer spriteRenderer, Sprite sprite, SpriteAspectMode mode = SpriteAspectMode.FitIn)
        {
            if (spriteRenderer.sprite == null)
                throw new System.Exception();
            SetSpriteManualAspectRatio(spriteRenderer, sprite, spriteRenderer.sprite.bounds.size, mode);
            //SetSpriteManualAspectRatio(spriteRenderer, sprite, spriteRenderer.sprite.rect.size,mode);
        }

        public static void SetSpriteWithRectTransfomAspect(this SpriteRenderer spriteRenderer, Sprite sprite, SpriteAspectMode mode = SpriteAspectMode.FitIn)
        {
            //var rect = spriteRenderer.transform as RectTransform;
            if (spriteRenderer.TryGetComponent<RectTransform>(out RectTransform rect))
                SetSpriteManualAspectRatio(spriteRenderer, sprite, rect.rect.size, mode);
            else
                throw new System.Exception("RectTransform Is Not Exist");
        }

        public static void SetSpriteManualAspectRatio(this SpriteRenderer spriteRenderer, Sprite sprite, Vector2 size, SpriteAspectMode mode = SpriteAspectMode.FitIn)
        {
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            var imgSize = sprite.bounds.size;

            float yRatio = size.y / imgSize.y;
            float xRatio = size.x / imgSize.x;
            float ratio;

            switch (mode)
            {
                case SpriteAspectMode.WidthControlsHeight:
                    ratio = xRatio;
                    break;
                case SpriteAspectMode.HeightControlsWidth:
                    ratio = yRatio;
                    break;
                case SpriteAspectMode.FitIn:
                    ratio = Mathf.Min(xRatio, yRatio);
                    break;
                default:
                    ratio = 1;
                    break;
            }

            spriteRenderer.size = imgSize * ratio;
            spriteRenderer.sprite = sprite;
        }

#if UNITY_EDITOR
        [MenuItem("CONTEXT/SpriteRenderer/SetAspectRatioFromRectTransform")]
        public static void Menu_SetAspectRatio(MenuCommand command)
        {
            var spriteRenderer = command.context as SpriteRenderer;
            spriteRenderer.SetSpriteWithRectTransfomAspect(spriteRenderer.sprite);
        }
#endif
    }

    public enum SpriteAspectMode
    {
        WidthControlsHeight = 1,
        HeightControlsWidth = 2,
        FitIn = 3,
    }
}
