using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;
using Amulay.Utility;

namespace TankStars.Level
{
    public class GroundManager : Singleton<GroundManager>
    {
        [SerializeField] private bool destroyable = true;
        [SerializeField] private GameObject[] cloudsPrefabs;
        [SerializeField] private BoxCollider cloudBounds;
        [SerializeField] private BoxCollider2D colliderBounds;
        [SerializeField] private SpriteShapeController controller;
        [SerializeField] private GameObject background;
        [SerializeField] private float sensitivity = 1f;

        private Spline spline;
        private EdgeCollider2D edge;

        private int layerMask = -1;

        internal event System.Action onGroundChanged;
        internal Bounds groundBounds { get; private set; }


        public GameObject[] Animals; 
        protected override void Awake()
        {
            base.Awake();
            if (instance != this)
                return;
            if (controller == null)
                CreateGround();
            if (background == null)
              //  CreateBackground();

            groundBounds = new Bounds(colliderBounds.transform.position + (Vector3)colliderBounds.offset, colliderBounds.size);
            spline = controller.spline;
            edge = controller.GetComponent<EdgeCollider2D>();
            layerMask = 1 << LayerMask.NameToLayer("Ground");
        }

        private void CreateBackground()
        {
            int groundIndex = Random.Range(3, 8);
            var background = Resources.Load<GameObject>($"Backgrounds/Background_{groundIndex}");
            Instantiate(background);
            background.transform.position = Vector3.up;
            print("This is Just Back");
        }

        private void CreateGround()
        {
            int groundIndex = 0; //Random.Range(0, 3);
            if (groundIndex == 1)
                destroyable = false;

            var groundPrefab = Resources.Load<SpriteShapeController>($"Grounds/Ground_{groundIndex}");
            controller = Instantiate(groundPrefab);
        }

        private void CreateSheeps()
        {
            int count = Random.Range(2,5);
            var prefab = Animals[Random.Range(0,Animals.Length)];
            for (int i = 0; i < count; i++)
            {
                var hit = Physics2D.Raycast(new Vector2(Random.Range(-30, 30), 10), Vector2.down);
                Instantiate(prefab, (hit.point), Quaternion.EulerAngles(hit.normal));
            }
        }

        private void CreateClouds()
        {
            var clouds = new List<GameObject>();
            int cloudCount = 5;
            var t_bounds = new Bounds(cloudBounds.transform.position + cloudBounds.center, cloudBounds.size);
            var t_min = t_bounds.min;
            var t_max = t_bounds.max;

            for (int i = 0; i < cloudCount; i++)
            {
                var position = new Vector3(Random.Range(t_min.x,t_max.x), Random.Range(t_min.y, t_max.y), Random.Range(t_min.z, t_max.z));
                var cloud = Instantiate(cloudsPrefabs[Random.Range(0, cloudsPrefabs.Length)], position, Quaternion.identity);
                cloud.transform.localEulerAngles = new Vector3(0, (Random.value > .5) ? 0 : 180, 0);
                var direction = (Random.value > .5) ? Vector3.right : Vector3.left;
                var speed = Random.Range(1f, 4f);
                cloud.transform.localScale = Vector3.one * Random.Range(.9f, 2f);

                Invoker.DoUpdate(float.PositiveInfinity, () =>
                {
                    cloud.transform.localPosition += direction * (speed * Time.deltaTime);
                    
                    if (direction.x > 0 && cloud.transform.localPosition.x > t_max.x)
                        direction.x = -direction.x;
                    else
                    if(direction.x < 0 && cloud.transform.localPosition.x < t_min.x)
                        direction.x = -direction.x;
                });
                
                clouds.Add(cloud);
            }
            cloudBounds.gameObject.SetActive(false);

        }

        private void Start()
        {
            if(GameManager.currentScene != SceneEnum.Level_Online_2P)
                CreateSheeps();
          //  CreateClouds();

            //for (int i = 0; i < spline.GetPointCount(); i++)
            //{
            //    spline.SetLeftTangent(i, spline.GetLeftTangent(i).normalized*2f);
            //    spline.SetRightTangent(i, spline.GetRightTangent(i).normalized*2f);
            //}
            //for (int i = 0; i < edge.edgeCount; i++)
            //{
            //    spline.InsertPointAt(i, edge.points[i]);
            //}
        }

        internal void DestroyGround(Vector2 point, float radius)
        {
            if (!destroyable) 
                return;

            RaycastHit2D chit = new RaycastHit2D();
            {
                chit = Physics2D.Raycast(point, Vector2.down, 20 + radius, layerMask);
                if (chit.collider != null)
                    point = chit.point;
            }

            RaycastHit2D lhit = new RaycastHit2D();
            for (int i = 0; i < 10; i++)
            {
                lhit = Physics2D.Raycast(point + new Vector2(-radius + (radius / 10f * i), 20), Vector2.down, 20 + radius, layerMask);
                if (lhit.collider != null)
                    break;
            }

            RaycastHit2D rhit = new RaycastHit2D();
            for (int i = 0; i < 10; i++)
            {
                rhit = Physics2D.Raycast(point + new Vector2( radius - (radius / 10f * i), 20), Vector2.down, 20 + radius, layerMask);
                if (rhit.collider != null)
                    break;
            }

            if (lhit.collider == null || rhit.collider == null)
                return;

            var p1 = lhit.point;
            var p3 = rhit.point;
            int index1 = FindSplineIndex(p1);
            int index3 = FindSplineIndex(p3);
            
            //remove points between index1 , index3
            for (int i = index1; i < index3;)
            {
                spline.RemovePointAt(i);
                index3--;
            }

            if (((Vector2)spline.GetPosition(index1 - 1) - p1).magnitude < .15f)
            {
                spline.RemovePointAt(index1 - 1);
                index1--;
                index3--;
            }

            if (((Vector2)spline.GetPosition(index3 + 1) - p3).magnitude < .15f)
            {
                spline.RemovePointAt(index3+1);
            }

            //create p1 point
            spline.InsertPointAt(index1, p1);
            spline.SetTangentMode(index1, ShapeTangentMode.Broken);
            spline.SetHeight(index1, .2f);
            index3++;

            //create p3 point
            spline.InsertPointAt(index3, p3);
            spline.SetTangentMode(index3, ShapeTangentMode.Broken);
            spline.SetHeight(index3, .2f);

            //create p2 point
            var pCenter = (lhit.point + rhit.point) / 2f;
            var ep = (lhit.point - rhit.point).normalized;
            var perpendicular = Vector2.Perpendicular(ep);
            var newRadius = Vector2.Distance(lhit.point, rhit.point) / 2f;
            var p2 = pCenter + perpendicular * newRadius;

            int index2 = index1 + 1;
            spline.InsertPointAt(index2, p2);
            spline.SetTangentMode(index2, ShapeTangentMode.Continuous);
            spline.SetHeight(index2, .2f);

            spline.SetLeftTangent(index2, ep * radius);
            spline.SetRightTangent(index2, -ep * radius);
            ///
#if UNITY_EDITOR
            t_center = point;
            t_r = radius;
#endif
            Debug.DrawLine(p1, p1 + Vector2.up * 2, Color.red, 10);
            Debug.DrawLine(p3, p3 + Vector2.up * 2, Color.red, 10);

            var center = point;
            var n = (p3 - p1).normalized;
            var dis = (p3 - p1).magnitude;
            int stepCount = 7;
            float step = dis / (float)stepCount;
            int indexx = index1 + 1;
            for (int i = 1; i < stepCount - 1; i++, indexx++)
            {
                var px = GetCirclePoint(p1.x + n.x * step * i, center, radius);
                //print(px);
                Debug.DrawLine(px, px + Vector2.up * 2, Color.blue, 10);
                //spline.InsertPointAt(indexx, px);
                //spline.SetTangentMode(indexx, ShapeTangentMode.Broken);
                //spline.SetTangentMode(indexx, ShapeTangentMode.Continuous);
                //spline.SetHeight(indexx, .2f);
            }

            onGroundChanged?.Invoke();
        }

        private int FindSplineIndex(Vector3 pos)
        {
            float minDisP2 = Mathf.Infinity;
            float t_dis;
            int index = 0;

            for (int i = 0; i < spline.GetPointCount(); i++)
            {
                Vector3 vec = spline.GetPosition(i);
                t_dis = (pos - vec).sqrMagnitude;
                if (minDisP2 > t_dis)
                {
                    index = i;
                    minDisP2 = t_dis;
                }
            }

            if (spline.GetPosition(index).x <= pos.x)
                return index + 1;
            else
                return index;
        }

        private int FindClosestSplineIndex(Vector3 pos)
        {
            float minDisP2 = Mathf.Infinity;
            float t_dis;
            int index = 0;

            for (int i = 0; i < spline.GetPointCount(); i++)
            {
                Vector3 vec = spline.GetPosition(i);
                t_dis = (pos - vec).sqrMagnitude;
                if (minDisP2 > t_dis)
                {
                    index = i;
                    minDisP2 = t_dis;
                }
            }

            return index;
        }

        private Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found)
        {
            float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

            if (tmp == 0)
            {
                // No solution!
                found = false;
                return Vector2.zero;
            }

            float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

            found = true;

            return new Vector2(
                B1.x + (B2.x - B1.x) * mu,
                B1.y + (B2.y - B1.y) * mu
            );
        }

        private Vector2 GetNormalCirclePoint(float x ,float radius)
        {
            var point = new Vector2(x, 0);
            point.y = Mathf.Sqrt(radius * radius - x * x);
            return point;
        }

        private Vector2 GetCirclePoint(float x, Vector2 center, float radius)
        {
            var point = new Vector2(x, 0);
            float part = Mathf.Sqrt(radius * radius - Mathf.Pow(x - center.x, 2));
            if (center.y + part > center.y - part)
                point.y = center.y - part;
            else
                point.y = center.y + part;
            return point;
        }
#if UNITY_EDITOR
        private Vector2 t_center;
        private float t_r;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(t_center, t_r);
        }
#endif
    }
}



/*
 internal void DestroyGround(Vector2 point, float radius)
        {
            var points = edge.points;
            List<int> res = new List<int>();

            for (int j = 0; j < points.Length; j++)
            {
                var dis = (points[j] - point).magnitude;
                if (Mathf.Abs(dis - radius) < sensitivity)
                {
                    res.Add(j);
                    Debug.DrawLine(points[j], points[j] + Vector2.up, Color.red, 3);
                }
            }

            if (res.Count <= 0)
                return;

            Vector2 p1 = points[res[0]];
            Vector2 p3 = points[res[res.Count - 1]];
            int index1 = FindSplineIndex(spline, p1);
            int index3 = FindSplineIndex(spline, p3);

            //if index1-1 is to near then remove index1-1
            if (index1 - 1 >= 0 && ((Vector2)spline.GetPosition(index1 - 1) - p1).magnitude < .5f)
            {
                spline.RemovePointAt(index1 - 1);
                index1--;
                index3--;
            }

            //if index3+1 is to near then remove index13+1
            if (index3 + 1 < spline.GetPointCount() && ((Vector2)spline.GetPosition(index3) - p3).magnitude < .5f) 
            {
                spline.RemovePointAt(index3 + 1);
            }

            //remove points between index1 , index3
            for (int i = index1; i < index3;)
            {
                spline.RemovePointAt(i);
                index3--;
            }

            Vector2 pCenter = (p1 + p3) / 2f;
            var ep = (p1 - p3).normalized;
            var perpendicular = Vector2.Perpendicular(ep);

            //Vector2 p2 = pCenter + perpendicular * radius;
            var newRadius = Vector2.Distance(p1, p3) / 2f;
            Vector2 p2 = pCenter + perpendicular * newRadius;

            //{
            //    var index0 = index1-1;
            //    var p0 = spline.GetPosition(index0);
            //    if (p0.x > p1.x)
            //    {
            //        spline.RemovePointAt(index0);
            //        //spline.InsertPointAt(index0, new Vector3(p1.x, p0.y));
            //        index1--;
            //        index3--;
            //    }

            //    var index4 = index3 + 1;
            //    var p4 = spline.GetPosition(index4);
            //    if (p4.x < p3.x)
            //    {
            //        spline.RemovePointAt(index4);
            //        //spline.InsertPointAt(index4 , new Vector3(p3.x, p4.y));
            //    }
            //}

            spline.InsertPointAt(index1, p1);
            spline.SetTangentMode(index1, ShapeTangentMode.Broken);
            spline.SetHeight(index1, .1f);
            //spline.SetRightTangent(index1, Vector3.down);

            index3++;
            spline.InsertPointAt(index3, p3);
            spline.SetTangentMode(index3, ShapeTangentMode.Broken);
            spline.SetHeight(index3, .1f);
            //spline.SetLeftTangent(index2, Vector3.down);

            int index2 = index1 + 1;
            spline.InsertPointAt(index2, p2);
            spline.SetTangentMode(index2, ShapeTangentMode.Continuous);
            spline.SetHeight(index2, .1f);
            spline.SetLeftTangent(index2, ep * radius);
            spline.SetRightTangent(index2, -ep * radius);

            //if (spline.GetPosition(index).x <= pos.x)
            //{
            //    spline.InsertPointAt(index + 1, pos + Vector3.down * 5);
            //    spline.SetTangentMode(index + 1, UnityEngine.U2D.ShapeTangentMode.Continuous);
            //    spline.SetRightTangent(index + 1, Vector3.right);
            //    spline.SetLeftTangent(index + 1, Vector3.left);
            //}
            //else
            //{
            //    spline.InsertPointAt(index, pos + Vector3.down * 5);
            //    spline.SetTangentMode(index + 1, UnityEngine.U2D.ShapeTangentMode.Continuous);
            //    spline.SetRightTangent(index + 1, Vector3.right);
            //    spline.SetLeftTangent(index + 1, Vector3.left);
            //}

            onGroundChanged?.Invoke();
        }

        private int FindSplineIndex(Spline spline, Vector3 pos)
        {
            float minDisP2 = Mathf.Infinity;
            float t_dis;
            int index = 0;

            for (int i = 0; i < spline.GetPointCount(); i++)
            {
                Vector3 vec = spline.GetPosition(i);
                t_dis = (pos - vec).sqrMagnitude;
                if (minDisP2 > t_dis)
                {
                    index = i;
                    minDisP2 = t_dis;
                }
            }

            if (spline.GetPosition(index).x <= pos.x)
                return index + 1;
            else
                return index;
        }
     
     */

/*
  internal void DestroyGround(Vector2 point, float radius)
    {
        if (radius < 1)
            return;

        var points = edge.points;
        List<int> res = new List<int>();

        for (int j = 0; j < points.Length; j++)
        {
            var dis = (points[j] - point).magnitude;
            if (Mathf.Abs(dis - radius) < sensitivity)
            {
                res.Add(j);
                Debug.DrawLine(points[j], points[j] + Vector2.up, Color.red, 3);
            }
        }

        if (res.Count <= 0)
            return;
        Vector2 p1 = points[res[0]];
        Vector2 p3 = points[res[res.Count - 1]];

        var newRadius = (p1 - p3).magnitude / 2f;
        if (newRadius < 1)
            return;

        int index1 = FindClosestSplineIndex(spline, p1);
        Vector2 p_index1 = spline.GetPosition(index1);
        if ((p_index1 - p1).magnitude < .2)
        {
            p1 = p_index1;
            print("leftpoint is ok");
        }
        else
        {
            //if (p_index1.x > p1.x)
            //    index1++;
            spline.InsertPointAt(index1, p1);
        }

        int index3 = FindClosestSplineIndex(spline, p3);
        Vector2 p_index3 = spline.GetPosition(index3);
        if ((p_index3 - p3).magnitude < .2)
        {
            p3 = p_index3;
            print("rightpoint is ok");
        }
        else
        {
            //if (p_index3.x > p3.x)
            //    index3--;

            spline.InsertPointAt(index3, p3);
        }

        //remove points between index1 , index3
        for (int i = index1; i < index3;)
        {
            spline.RemovePointAt(i);
            index3--;
        }

        Vector2 pCenter = (p1 + p3) / 2f;
        var ep = (p1 - p3).normalized;
        var perpendicular = Vector2.Perpendicular(ep);
        Vector2 p2 = pCenter + perpendicular * newRadius;

        int index2 = index1 + 1;
        spline.InsertPointAt(index2, p2);
        spline.SetTangentMode(index2, ShapeTangentMode.Continuous);
        spline.SetHeight(index2, .1f);
        spline.SetLeftTangent(index2, ep * radius);
        spline.SetRightTangent(index2, -ep * radius);

        onGroundChanged?.Invoke();
    }
 */
