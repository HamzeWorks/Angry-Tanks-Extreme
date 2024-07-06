using Amulay.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace TankStars.Level
{
    public class AIHealthBar : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] HealthBar healthBar;
        [SerializeField] PositionConstraint positionConstraint;

        public void Init(Tank tank)
        {
            healthBar.Init(tank);
            Invoker.Do(.1f, () => { canvas.worldCamera = CameraManager.instance.main; });

            {
                var c = new ConstraintSource();
                c.sourceTransform = tank.transform;
                c.weight = 1;
                positionConstraint.AddSource(c);
            }
        }

        public static AIHealthBar Create(Tank tank)
        {
            var prefab = Resources.Load<AIHealthBar>("[ AI HealthBar ]");
            var obj = Instantiate(prefab);
            
            obj.Init(tank);
            return obj;
        }
    }
}
