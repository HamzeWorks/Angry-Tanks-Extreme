using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using Amulay.Utility;

namespace TankStars.Level
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Item : MonoBehaviour
    {
        [SerializeField] protected new Rigidbody2D rigidbody;
        [SerializeField] ItemData m_data;
        [SerializeField] protected Item prefab;
        [SerializeField] Sprite m_icon;
        [SerializeField] protected GameObject explotionPrefab;

        [SerializeField] ContactFilter2D c;
        protected bool rotaionUpdate = false;
        private Vector3 lastPostionl;

        private int level = 0;
        #region internal
        internal static List<Item> flyingItems { get; private set; } = new List<Item>();
        internal static event Action<Item> onDestroyAllFlyingItems;
        internal event Action onFinished;
        internal ItemData data => m_data;
        internal float power => m_data.CalcuatePowerWithManualLevel(level);
        internal float weight => m_data.weight;
        internal float radius => m_data.radius;
        internal Sprite icon => m_data.icon;
        internal string itemName() {
            Debug.Log("Item Name from Item Class = " + m_data.itemName);
            return m_data.itemName; }
        internal int _level => level;
        #endregion

        protected virtual void Awake()
        {
            flyingItems.Add(this);

            //destroy item after end of lifetime
            Invoker.DoCoroutine(data.lifeTime, () =>
            {
                if (this != null)
                {
                    //OnCollisionEnter2D(null);
                    ExplotionEffect();
                    Destroy(gameObject);
                }
            });
        }

        #region Shoot 
        internal virtual void Shoot(Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            var item = Clone();
            item.InternalShoot(firePoint, aim, powerWithWeightFactor, onFinished);
            this.onFinished = onFinished;
        }

        protected virtual void InternalShoot(Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            transform.position = firePoint;
            transform.eulerAngles = Vector3.forward * aim;
            rigidbody.velocity = new Vector2(Mathf.Cos(aim * Mathf.Deg2Rad), Mathf.Sin(aim * Mathf.Deg2Rad)) * powerWithWeightFactor;
            rotaionUpdate = true;
        }
        #endregion

        #region Rotaion
        private void FixedUpdate()
        {
            if(rotaionUpdate)
                RotaionUpdate();
        }

        private void RotaionUpdate()
        {
            transform.right = transform.position - lastPostionl;
            lastPostionl = transform.position;
        }

        #endregion;

        #region Collision ExplotionEffect
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            rotaionUpdate = false;
            gameObject.SetActive(false);
            Destroy(gameObject);
            if (collision != null && collision.otherCollider.gameObject.CompareTag("Tank"))
            {
                //prefect shoot
                var tank = collision.otherCollider.GetComponent<Tank>();
                tank.AddDamage(power);
            }
            else
            {
                //TODO:just tank layer;
                var tank =  Physics2D.OverlapCircle(transform.position, radius, 1 << LayerMask.NameToLayer("Tank"));
                if (tank != null && tank.gameObject.CompareTag("Tank"))
                {
                    //TODO: don't use root position - use clostes point to item;
                    tank.GetComponent<Tank>().AddDamage(power * 1 - Mathf.Clamp01(Vector2.Distance(tank.transform.position,transform.position)/ radius));
                }
            }


            DestructGround();
            ExplotionEffect();

            CameraManager.instance.Shake();
        }

        protected virtual void ExplotionEffect()
        {
            if (explotionPrefab != null)
            {
                var fx = Instantiate(explotionPrefab);
                fx.transform.position = transform.position;
                fx.transform.localScale = new Vector3(2, 2, 1);
                Destroy(fx.gameObject, 3f);
            }
        }

        protected void DestructGround()
        {
            var col = new List<Collider2D>();
            //TODO:cast on groundLayer
            Physics2D.OverlapCircle(transform.position, radius, c, col);
            for (int i = 0; i < col.Count; i++)
                if (col[i].CompareTag("Ground"))
                    GroundManager.instance.DestroyGround(transform.position, radius);
                else if (col[i].CompareTag("Destructible"))
                    col[i].gameObject.GetComponent<TankStars.Level.DestructibleObject>().DestroyObject(transform.position, radius, data);
        }

        protected virtual void OnDestroy()
        {
            flyingItems.Remove(this);
            if (flyingItems.Count == 0)
            {
                onFinished?.Invoke();
                onDestroyAllFlyingItems?.Invoke(this);
            }
        }
        #endregion

        public Item SetLevel(int level)
        {
            level = Mathf.Max(level, 0);
            this.level = level;
            return this;
        }

        protected Item Clone()
        {
            return Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity).SetLevel(level);
        }

        #region static Load
        internal static Item GetItemFromResource(string itemName)
        {
            //Item item = Addressables.LoadAssetAsync<Item>(itemName).Result;
            var item = Resources.Load<Item>($"Items/{itemName}");
            return item;
        }

        internal static Item ShootItem(string itemName ,int level, Vector3 firePoint, float aim, float powerWithWeightFactor, Action onFinished)
        {
            //TODO: fix: shootItem != item.getItemFromRes...
            var item = GetItemFromResource(itemName);
            item.SetLevel(level);
            item.Shoot(firePoint, aim, powerWithWeightFactor, onFinished);
            return item;
        }
        #endregion

    }
}