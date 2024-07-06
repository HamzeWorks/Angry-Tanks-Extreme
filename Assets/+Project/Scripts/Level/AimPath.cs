using Amulay.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankStars.Level
{
    public class AimPath : MonoBehaviour
    {
        public static AimPath instance { get; private set; }

        [SerializeField] LineRenderer line;
        [SerializeField] GameObject pointsParent;
        [SerializeField] Transform[] points;
        [SerializeField] float animationSpeed = .1f;
        //[SerializeField] float animationHertz = .1f;
        [SerializeField, Range(.001f, 1f)] float pointSizeInAnimation = .05f;
        [SerializeField, Range(.001f, 3f)] float pointStartSize = 1;
        [Space(10)]
        [SerializeField] float length = 5f;
        [SerializeField] float angle;
        [SerializeField] float velocity;

        float gravity;

        private int resolotion;
        //temp -> for more seed;
        private float t_animationTime;

        private float t_v0x; //start velocity of x
        private float t_v0y; //start velocity of y

        private void Awake()
        {
            instance = this;
            enabled = false;
            pointsParent.gameObject.SetActive(false);
            gravity = Physics2D.gravity.y;

            Invoker.DoCoroutine(0, () =>
            {
                if (GameplayManager.instance?.localPlayerTank != null)
                {
                    var localPlayerTank = GameplayManager.instance.localPlayerTank;
                    localPlayerTank.onMovement += (position, aim, power) =>
                    {
                        var from = localPlayerTank.firePointPosition;
                        Draw(from, aim, power);
                    };
                }
            });
        }

        private void Update()
        {
            t_animationTime = (t_animationTime + Time.deltaTime * animationSpeed) % 1;
            Draw(transform.position, angle, velocity);
        }

        internal void Draw(Vector3 from, float angle, float velocity)
        {
            transform.position = from;
            this.angle = angle;
            this.velocity = velocity;
            this.gravity = Physics2D.gravity.y;
            this.resolotion = points.Length;

            var res = CalculatePoints(angle, velocity);
            float lowDistanceScale = Mathf.Clamp(pointStartSize * velocity / 10f, pointStartSize / 3f, pointStartSize);
            //float lowDistance = Mathf.Clamp(t_maxDistance, .05f, 5f) / 5f;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].transform.localPosition = res[i];
                points[i].transform.localScale = Vector3.one * (lowDistanceScale - Mathf.Max(.1f, Mathf.Clamp01(res[i].magnitude * pointSizeInAnimation) * lowDistanceScale));
            }

            //line.SetPositions(res);
            pointsParent.SetActive(true);
            enabled = true;
        }

        internal void Hide()
        {
            pointsParent.SetActive(false);
            enabled = false;
        }

        private Vector3[] CalculatePoints(float angle, float velocity)
        {
            t_v0x = Mathf.Cos(angle*Mathf.Deg2Rad) * velocity;
            t_v0y = Mathf.Sin(angle*Mathf.Deg2Rad) * velocity;

            var points = new Vector3[resolotion];

            for (int i = 0; i < resolotion; i++)
            {
                points[i] = CalculatePoint((((i / (float)resolotion) + t_animationTime) % 1) * length);
            }

            return points;
        }

        private Vector3 CalculatePoint(float t)
        {
            return new Vector3()
            {
                x = t_v0x * t,
                y = .5f * gravity * t * t + t_v0y * t,
                z = 0,
            };
        }

        internal Vector3 CalculatePoint(float t, float angle, float velocity, float gravity)
        {
            var v0x = Mathf.Cos(angle * Mathf.Deg2Rad) * velocity;
            var v0y = Mathf.Sin(angle * Mathf.Deg2Rad) * velocity;

            return new Vector3()
            {
                x = v0x * t,
                y = .5f * gravity * t * t + v0y * t,
                z = 0,
            };
        }

        internal static bool CalculateAngle(Vector3 from, Vector3 to, float velocity, out float angle, bool minAngle, float gravity)
        {
            gravity = Physics2D.gravity.y;
            Vector3 toTarget = to - from;
            Vector3 gravity3 = Vector3.up * gravity;
            // Set up the terms we need to solve the quadratic equations.
            float gSquared = gravity * gravity;
            float b = velocity * velocity + Vector3.Dot(toTarget, gravity3);
            float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

            // Check whether the target is reachable at max speed or less.
            if (discriminant < 0)
            {
                // Target is too far away to hit at this speed.
                // Abort, or fire at max speed in its general direction?
                angle = float.NaN;
                return false;
            }

            float discRoot = Mathf.Sqrt(discriminant);

            // Highest shot with the given max speed:
            float T = Mathf.Sqrt((b + ((minAngle) ? -discRoot : +discRoot)) * 2f / gSquared);

            //float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));

            // Convert from time-to-hit to a launch velocity:
            Vector3 velocity2 = toTarget / T - gravity3 * T / 2f;
            angle = (Mathf.Atan2(velocity2.y, velocity2.x));

            angle *= Mathf.Rad2Deg;
            return true;

        }

        internal static bool CalculateAngle(Vector3 from, Vector3 to, float velocity, out float angle, bool minAngle = false) => CalculateAngle(from, to, velocity, out angle, minAngle, Physics2D.gravity.y);


        //private void Draww(Vector3 from, float angle, float velocity, float gravity)
        //{
        //    this.angle = angle;
        //    this.velocity = velocity;
        //    this.gravity = gravity;
        //    this.resolotion = points.Length;


        //    transform.position = from;
        //    var res = CalculatePoints();

        //    //float lowDistance = Mathf.Clamp(t_maxDistance, .05f, 5f) / 5f;
        //    for (int i = 0; i < points.Length; i++)
        //    {
        //        points[i].transform.localPosition = res[i];
        //        points[i].transform.localScale = Vector3.one * (pointStartSize - Mathf.Max(.1f, Mathf.Clamp01(res[i].magnitude * pointSizeInAnimation) * pointStartSize));
        //    }

        //    //line.SetPositions(CalculatePoints());
        //    pointsParent.SetActive(true);
        //    enabled = true;
        //}



        //private Vector3[] CalculatePoints()
        //{
        //    var points = new Vector3[resolotion + 1];
        //    t_radianAngle = this.angle * Mathf.Deg2Rad;
        //    t_cosRadianAngle = Mathf.Cos(t_radianAngle);
        //    t_tanRadianAngle = Mathf.Tan(t_radianAngle);
        //    t_maxDistance = (velocity * velocity * Mathf.Sin(2 * t_radianAngle)) / gravity;

        //    for (int i = 0; i < resolotion; i++)
        //    {
        //        float t = ((float)i / (float)resolotion + t_animationTime) % 1;
        //        points[i] = CalcultatePoint(t);
        //    }

        //    return points;
        //}

        //private Vector3 CalcultatePoint(float t)
        //{
        //    float x = t * t_maxDistance;
        //    float y = x * t_tanRadianAngle - ((gravity * x * x) / (2 * velocity * velocity * t_cosRadianAngle * t_cosRadianAngle));
        //    return new Vector3(x, y);
        //}




        //#if UNITY_EDITOR
        //        private void OnValidate()
        //        {
        //            if (Application.isPlaying == false) return;
        //            Draw(Vector3.zero, angle, velocity, gravity);
        //        }
        //#endif
    }
}