using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Spine.Unity;
using Amulay.Utility;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TankStars.Level
{
    public class Tank : MonoBehaviour
    {
        #region properties
        [SerializeField]
        SkeletonAnimation animator;
        [SerializeField]
        new TankAnimation animation;
        [Space(10f)]
        [SerializeField]
        Tank prefab;
        [SerializeField]
        GameObject explotionPrefab;
        [SerializeField]
        Transform firePoint;
        [SerializeField]
        Transform firePoint2;
        [SerializeField]
        Transform aimPivot;
        [Header("Data"), Space(10)]
        [SerializeField]
        float fuleTankSize = 1f;
        [SerializeField]
        float maxShootPower = 5f;
        [SerializeField]
        float moveSpeed = 10f;
        [SerializeField]
        float m_maxHealth = 1;
        [SerializeField, Range(-90f, 90f)]
        float minAngle = -10f;
        [SerializeField]
        Item[] defaultItems;

        protected List<Item> items = new List<Item>();
        protected Item m_selectedItem;
        protected Vector3 startScale;
        private LayerMask groundLayer;
        private bool alignmentRotaionAfterMovemetn = false;
        //t->temp
        private Quaternion t_previusRetaion;
        private Vector3 t_currentScale;
        #region Inputs
        private float aimInput;
        private float powerInput;
        private float moveInput;
        #endregion

        #region internal
        /// <summary>
        /// item
        /// firePoint
        /// aim
        /// power
        /// onFinishCallback
        /// </summary>
        internal event Action<Item, Vector3, float, float, Action> onShoot;
        /// <summary>
        /// currentHealth
        /// damage
        /// item
        /// </summary>
        internal event Action<float, float, Item> onGetDamage;
        /// <summary>
        /// call when transform or aim changed
        /// currentPosition
        /// aim
        /// power
        /// </summary>
        internal event Action<Vector3, float, float> onMovement;

        //internal NetworkPlayer connectedPlayer { get; private set; }
        internal float currentFule { get; private set; }
        internal float currentHealth { get; private set; }
        internal float maxHealth => m_maxHealth;
        internal float maxFule => fuleTankSize;//*
        internal float maxShootPowerr => maxShootPower;//*
        internal float currentShootPower => powerInput * maxShootPower;
        internal float currentShootPowerWithWeightFactor => powerInput * maxShootPower / selectedItem.weight;
        internal float currentAim => (Mathf.Sign(transform.localScale.x) > 0) ? aimPivot.eulerAngles.z : aimPivot.eulerAngles.z - 180f;
        internal Vector3 firePointPosition => firePoint.position;
        internal Item selectedItem => m_selectedItem;
        internal List<Item> itemsList => items;
        internal Item[] _defaultItems => defaultItems;
        #endregion

        #endregion


        public GameObject Tank_Obj;
        public GameObject Fire_FX, Muzzle, cam;
        bool DoAnim;

        Animator myAnimator;
        const string Fire_Anim = "Fire";
        const string Idle_Anim = "Idle";

        GameObject missile;
        public GameObject aimassisssst;
        upgradesystem upg;                 //ref to upgradesystem script

        private int indexOfTank;
        private int tankLevel;

        static public float public_health;

        public GameObject Cemetry;

        private void Awake()
        {
            upg = upgradesystem.instance;
            cam = GameObject.Find("Main Camera").transform.gameObject;
            startScale = transform.localScale;
            t_currentScale = startScale;
           // currentHealth = m_maxHealth;
            currentFule = fuleTankSize;
            powerInput = .5f;
            groundLayer = 1 << LayerMask.NameToLayer("Ground");
            RelaodItems();
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != "0_MainMenu")
            {
                GameplayManager.instance.onStartNewRound += (tank) => { if (tank == this) currentFule = fuleTankSize; };
                if (name == "tank_yellow(Clone)")
                {
                    Tank_Obj.GetComponent<Animator>().enabled = false; /// Change At Animation JOB !!
                    transform.DOScale(new Vector3(1, 1, 1), 0.1f);
                    #region stats
                    if (PlayerPrefs.GetInt("tank1") != 0)
                    {
                        print("health is " + upg.tanks[0].Items[PlayerPrefs.GetInt("tank1") - 1].health);
                        currentHealth = upg.tanks[0].Items[PlayerPrefs.GetInt("tank1") - 1].health;
                    }else
                    {
                        print("health is " + upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].health);
                        currentHealth = upg.tanks[0].Items[PlayerPrefs.GetInt("tank1")].health;
                    }
                    #endregion
                }
                else if (name == "tank_green2(Clone)")
                {
                    Tank_Obj.GetComponent<Animator>().enabled = false; /// Change At Animation JOB !!

                    transform.DOScale(new Vector3(1, 1, 1), 0.1f);
                    #region stats
                    if (PlayerPrefs.GetInt("tank3") != 0)
                    {
                        currentHealth = upg.tanks[2].Items[PlayerPrefs.GetInt("tank3") - 1].health;
                        print("healthesh : " + upg.tanks[2].Items[PlayerPrefs.GetInt("tank3") - 1].health);
                    }else
                    {
                        currentHealth = upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].health;
                        print("healthesh : " + upg.tanks[2].Items[PlayerPrefs.GetInt("tank3")].health);
                    }
                    #endregion

                }
                else if (name == "tank_green1(Clone)")
                {
                    Tank_Obj.GetComponent<Animator>().enabled = false; /// Change At Animation JOB !!

                    transform.DOScale(new Vector3(1, 1, 1), 0.1f);

                    #region stats
                    if (PlayerPrefs.GetInt("tank2") != 0)
                    {
                        currentHealth = upg.tanks[1].Items[PlayerPrefs.GetInt("tank2") - 1].health;
                    }else
                    {
                        currentHealth = upg.tanks[1].Items[PlayerPrefs.GetInt("tank2")].health;

                    }
                    #endregion
                }
                else if (name == "tank_blue(Clone)")
                {
                    Tank_Obj.GetComponent<Animator>().enabled = false; /// Change At Animation JOB !!
                    transform.DOScale(new Vector3(1, 1, 1), 0.1f);
                    #region stats
                       if (PlayerPrefs.GetInt("tank4") != 0)
                       {
                           currentHealth = upg.tanks[3].Items[PlayerPrefs.GetInt("tank4") - 1].health;
                       }else
                       {
                           currentHealth = upg.tanks[3].Items[PlayerPrefs.GetInt("tank4")].health;

                       }

                    #endregion
                }
                public_health = currentHealth;
            }
            currentFule = fuleTankSize;

            for (int i = 0; i < defaultItems.Length; i++)
                defaultItems[i].SetLevel(defaultItems[i].data.level);
        }

        private void RelaodItems()
        {
            for (int i = 0; i < defaultItems.Length; i++)
                items.Add(defaultItems[i]);
            m_selectedItem = items[0];
        }

        private void Start()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            myAnimator = Tank_Obj.GetComponent<Animator>();
            DoAnim = true;
            if (currentScene.name != "0_MainMenu")
            {
                print("health is : " + currentHealth + " speed is : " + moveSpeed + " and power is : " + maxShootPower);
            }
            if (currentScene.name == "0_MainMenu")
            {

                if (name == "tank_yellow")
                {
                    //  Tank_Obj.GetComponent<Animator>().enabled = false; /// Change At Animation JOB !!
                    transform.DOScale(new Vector3(100, 100, 100), 0.1f);

                }
                if (name == "tank_green2")
                {
                    //  Tank_Obj.GetComponent<Animator>().enabled = false; /// Change At Animation JOB !!

                    transform.DOScale(new Vector3(100, 100, 100), 0.1f);
                }
                if (name == "tank_green1")
                {
                    //   Tank_Obj.GetComponent<Animator>().enabled = false; /// Change At Animation JOB !!

                    transform.DOScale(new Vector3(100, 100, 100), 0.1f);
                }

                //InitInput();
                MoveUpdate(true);
            }

            /*
            state.setAnimation(0, "walk", true);
            state.setEmptyAnimation(1, 0);
            state.addAnimation(1, "run", true, 0).mixDuration = 1.5f;
            state.addEmptyAnimation(1, 2.5f, 3);
            */
            #region upgrade and stats

            /*

            if (name == "tank_green1(Clone)")
            {
                switch (PlayerPrefs.GetInt("tank2"))
                {
                    case 0:
                        moveSpeed = upg.tanks[1].Items[0].speed;
                        m_maxHealth = upg.tanks[1].Items[0].health;
                        maxShootPower = upg.tanks[1].Items[0].power;
                        break;
                    case 1:
                        moveSpeed = upg.tanks[1].Items[1].speed;
                        m_maxHealth = upg.tanks[1].Items[1].health;
                        maxShootPower = upg.tanks[1].Items[1].power;
                        break;
                    case 2:
                        moveSpeed = upg.tanks[1].Items[2].speed;
                        m_maxHealth = upg.tanks[1].Items[2].health;
                        maxShootPower = upg.tanks[1].Items[2].power;
                        break;
                }
            }
            if (name == "tank_green2(Clone)")
            {
                switch (PlayerPrefs.GetInt("tank3"))
                {
                    case 0:
                        moveSpeed = upg.tanks[2].Items[0].speed;
                        m_maxHealth = upg.tanks[2].Items[0].health;
                        maxShootPower = upg.tanks[2].Items[0].power;
                        break;
                    case 1:
                        moveSpeed = upg.tanks[2].Items[1].speed;
                        m_maxHealth = upg.tanks[2].Items[1].health;
                        maxShootPower = upg.tanks[2].Items[1].power;
                        break;
                    case 2:
                        moveSpeed = upg.tanks[2].Items[2].speed;
                        m_maxHealth = upg.tanks[2].Items[2].health;
                        maxShootPower = upg.tanks[2].Items[2].power;
                        break;
                }
            }
            else if (name == "tank_yellow(Clone)")
            {
                switch (PlayerPrefs.GetInt("tank1"))
                {
                    case 0:
                        moveSpeed = upg.tanks[0].Items[0].speed;
                        m_maxHealth = upg.tanks[0].Items[0].health;
                        maxShootPower = upg.tanks[0].Items[0].power;
                        break;
                    case 1:
                        moveSpeed = upg.tanks[0].Items[1].speed;
                        m_maxHealth = upg.tanks[0].Items[1].health;
                        maxShootPower = upg.tanks[0].Items[1].power;
                        break;
                    case 2:
                        moveSpeed = upg.tanks[0].Items[2].speed;
                        m_maxHealth = upg.tanks[0].Items[2].health;
                        maxShootPower = upg.tanks[0].Items[2].power;
                        break;
                }
            }
            */
            #endregion
        }

        internal void InitInput(IInputBase input = null)
        {
            if (input == null)
                input = LocalInput.instance;

            input.onAimInput += (aim, power) =>
            {
                aimInput = aim;
                powerInput = power;
                AimUpdate();
            };

            input.onMoveInput += (move) =>
            {
                if (move != 0 && currentFule != 0)
                {
                    if (moveInput != 0)
                        animator.AnimationState.SetAnimation(0, animation.drive, true);
                }
                else if (moveInput != 0)//*
                {
                    animator.AnimationState.SetAnimation(0, animation.brakeForward, false);
                }

                moveInput = move;
                bool canMove = MoveUpdate();//TODO:movement in fixed update;
                AimUpdate();
            };

            input.onShoot += () =>
            {
                Shoot();
            };

            input.onSelectItem += (item) =>
            {
                m_selectedItem = item;
            };

            input.onForceMovement += (position, rotaion, aim, power) =>
            {
                transform.position = position;
                transform.rotation = rotaion;
                aimInput = aim;
                powerInput = power;
                AimUpdate();
            };

            //GroundManager.instance.onGroundChanged += () => MoveUpdate();
        }

        private void Shoot()
        {
            var t_selectedItem = selectedItem;  //beacause - don't change when select new item
            var t_power = currentShootPowerWithWeightFactor;    //beacause - don't change when select new item

            Action callback = null;
            selectedItem.Shoot(firePoint.position, currentAim, t_power, callback);
            CameraManager.instance.Shake(.2f, .5f);

            items.Remove(selectedItem);
            if (items.Count == 0)
                RelaodItems();

            m_selectedItem = items[0];
            onShoot?.Invoke(t_selectedItem, firePoint.position, currentAim, t_power, callback);

            animator.AnimationState.SetAnimation(0, animation.shoot, false);
            StartCoroutine(ShootAnim());
        }
        IEnumerator ShootAnim()
        {
            GameObject FX, muzzle;
            FX = Instantiate(Fire_FX, firePoint.transform.position, Quaternion.identity) as GameObject;
            muzzle = Instantiate(Muzzle, firePoint.transform.position, Quaternion.identity) as GameObject;
            //  missile= GameObject.FindGameObjectWithTag("Missile").transform.gameObject as GameObject;

            //  cam.transform.DOMoveX(missile.transform.position.x, 3);
            cam.GetComponent<Camera>().DOOrthoSize(20, 1);
            yield return new WaitForSeconds(3);
            cam.GetComponent<Camera>().DOOrthoSize(30, 1);

            Destroy(FX);
            Destroy(muzzle);
        }
        private void FixedUpdate()
        {

            Alignment(true);
            AimUpdate(true);
            //if (alignmentRotaionAfterMovemetn)
            //{
            //    if (t_previusRetaion == transform.rotation)
            //        alignmentRotaionAfterMovemetn = false;
            //    var hit = Physics2D.Raycast(transform.position + transform.up, -transform.up, Mathf.Infinity, groundLayer);
            //    if (hit.collider != null)
            //    {
            //        transform.position = hit.point;
            //        transform.up = Vector2.Lerp(transform.up, hit.normal, Time.deltaTime * 3f);
            //    }
            //    t_previusRetaion = transform.rotation;
            //    AimUpdate();
            //}
        }

        private bool MoveUpdate(bool force = false)
        {
            //TODO: Use FixedUpdate
            currentFule -= (Mathf.Abs(moveInput) * Time.deltaTime);
            if (currentFule <= 0 && force == false)
            {
                currentFule = 0;
                return false;
            }

            transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0);
            //var hit = Physics2D.Raycast(transform.position + transform.up, Vector3.down, Mathf.Infinity, groundLayer);
            Alignment2(force);
            return true;

        }

        private void Alignment(bool force)
        {
            //var hit = Physics2D.Raycast(transform.position + Vector3.right * moveInput * moveSpeed + Vector3.up * 5f, -transform.up, Mathf.Infinity, groundLayer);
            var hit = Physics2D.Raycast(transform.position + transform.up, -transform.up, 50f, groundLayer);
            if (hit.collider != null)
            {
                //if (((Vector2)transform.position - hit.point).sqrMagnitude > 3f)
                //    return;
                transform.position = hit.point;
                transform.up = Vector2.Lerp(transform.up, hit.normal, Time.fixedDeltaTime * 3f); //TODO: don't use lerp;
                if (force == false)
                    onMovement?.Invoke(transform.position, currentAim, currentShootPowerWithWeightFactor);
            }
        }

        private void Alignment2(bool force)
        {
            //var hit = Physics2D.Raycast(transform.position + Vector3.right * moveInput * moveSpeed + Vector3.up * 5f, -transform.up, Mathf.Infinity, groundLayer);
            var hit = Physics2D.Raycast(transform.position + Vector3.up, Vector3.down, 50f, groundLayer);
            if (hit.collider != null)
            {
                if (((Vector2)transform.position - hit.point).sqrMagnitude > 3f)
                    return;
                transform.position = hit.point;
                transform.up = Vector2.Lerp(transform.up, hit.normal, Time.fixedDeltaTime * 3f); //TODO: don't use lerp;
                if (force == false)
                    onMovement?.Invoke(transform.position, currentAim, currentShootPowerWithWeightFactor);
            }
        }

        private void AimUpdate(bool force = false)
        {
            float delta = Mathf.DeltaAngle(aimInput, transform.eulerAngles.z);

            if (delta < -90f || delta > 90f)
            {
                if (t_currentScale.x != -startScale.x)
                {
                    t_currentScale.x = -startScale.x;

                    transform.DOKill();
                    transform.localScale = t_currentScale;
                    transform.DOPunchScale(t_currentScale.normalized * .1f, .5f, 3);

                }
                aimPivot.eulerAngles = Vector3.forward * (aimInput - 180f);

            }
            else
            {
                if (t_currentScale.x != startScale.x)
                {
                    t_currentScale.x = startScale.x;

                    transform.DOKill();
                    transform.localScale = t_currentScale;
                    transform.DOPunchScale(t_currentScale.normalized * .1f, .5f, 3);
                }
                aimPivot.eulerAngles = Vector3.forward * aimInput;
            }

            if (aimPivot.localEulerAngles.z > 180f - minAngle && aimPivot.localEulerAngles.z < minAngle + 360f)
                aimPivot.localEulerAngles = Vector3.forward * (minAngle + 360f);

            //if(updateAimPath)
            //    AimPath.instance.Draw(firePoint.transform.position, currentAim, currentShootPowerWithWeightFactor);
            if (force == false)
                onMovement?.Invoke(transform.position, currentAim, currentShootPowerWithWeightFactor);
            //if (transform.localScale.x > 0)
            //    aimPivot.eulerAngles = new Vector3(0, 0, aimInput);
            //else
            //{
            //    //aimPivot.eulerAngles = new Vector3(0, 0, aimInput);
            //    print(aimInput);
            //}

            //if (aimPivot.localEulerAngles.z > 90)
            //{
            //    transform.localEulerAngles = Vector3.forward * 80f;
            //    transform.localScale = 
            //}

            //if (aimPivot.localEulerAngles.z > 190f && aimPivot.localEulerAngles.z < 350f)
            //    aimPivot.localEulerAngles = Vector3.forward * 350f;

        }

        #region NetworkProlemFunctions
        public void ForceShoot(ShootMessage shoot)
        {
            var t_selectedItem = items.Find((m) => (m.itemName() == shoot.item.name));  //beacause - don't change when select new item
            var t_power = shoot.power;    //beacause - don't change when select new item

            Action callback = null;
            Item.ShootItem(shoot.item.name, (int)shoot.item.level, shoot.firePoint.ToVector3(), shoot.aim, shoot.power, callback);
            CameraManager.instance.Shake(.2f, .5f);

            items.Remove(selectedItem);
            if (items.Count == 0)
                RelaodItems();

            m_selectedItem = items[0];
            onShoot?.Invoke(t_selectedItem, shoot.firePoint.ToVector3(), shoot.aim, shoot.power, callback);

            animator.AnimationState.SetAnimation(0, animation.shoot, false);
            //  cam.transform.DOMoveX(t_selectedItem.transform.position.x, 1);
        }

        public void OverrideFuleTankSize(float value = 100f)
        {
            fuleTankSize = value;
            currentFule = fuleTankSize;
        }
        #endregion

        #region Internal
        internal static Tank GetPrefab(string tankName)
        {
            var tank = Resources.Load<Tank>($"Tanks/{tankName}");
            return tank;
        }

        internal void AddDamage(float damage, Item item = null)
        {
            if (damage <= 0)
                return;
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            onGetDamage?.Invoke(currentHealth, damage, item);

            //onplayer die
            if (currentHealth <= 0)
            {
                if (explotionPrefab != null)
                {
                    Invoker.Do(.2f, () =>
                    {
                        var explotion = Instantiate(explotionPrefab);
                        explotion.transform.position = transform.position;
                        CameraManager.instance.Shake();
                        gameObject.SetActive(false);
                        if (GameObject.Find("[ Aim Path ]") != null)
                        {
                            GameObject.Find("[ Aim Path ]").SetActive(false);
                        }
                    });
                   Instantiate(Cemetry, transform.position, Quaternion.identity);
                   

                }

            }
            
            MoveUpdate();//*
        }
        #endregion

        void OnMouseDown()
        {

            if (name == "tank_green" || name == "tank_yellow")
            {
                if (DoAnim)
                {
                    StartCoroutine(TankAnim1());
                }
            }
            else if (name == "tank_green2")
            {
                if (DoAnim)
                {
                    StartCoroutine(TankAnim2());
                }
            }
            else if (name == "tank_blue")
            {
                if (DoAnim)
                {
                    StartCoroutine(TankAnim4());
                }
            }
        

        }
        IEnumerator TankAnim1()
        {
            DoAnim = false;

            myAnimator.SetTrigger(Fire_Anim);
            GameObject FX, muzzle;
            FX = Instantiate(Fire_FX, firePoint.transform.position, Quaternion.identity) as GameObject;
            muzzle = Instantiate(Muzzle, firePoint.transform.position, Quaternion.identity) as GameObject;

            yield return new WaitForSeconds(1);

            myAnimator.SetTrigger(Idle_Anim);
            Destroy(FX);
            Destroy(muzzle);
            DoAnim = true;
        }
        IEnumerator TankAnim2()
        {
            DoAnim = false;

            myAnimator.SetTrigger(Fire_Anim);
            GameObject FX, muzzle;
            FX = Instantiate(Fire_FX, firePoint.transform.position, Quaternion.identity) as GameObject;
            muzzle = Instantiate(Muzzle, firePoint.transform.position, Quaternion.identity) as GameObject;

            yield return new WaitForSeconds(2.30f);

            myAnimator.SetTrigger(Idle_Anim);
            Destroy(FX);
            Destroy(muzzle);
            DoAnim = true;
        }
        IEnumerator TankAnim4()
        {
            DoAnim = false;
            myAnimator.SetTrigger(Fire_Anim);
            GameObject FX, muzzle;
            yield return new WaitForSeconds(0.2f);
            for (int i = 1; i <= 2; i++)
            {
                Instantiate(Fire_FX, firePoint.transform.position, firePoint.transform.rotation);
                Instantiate(Muzzle, firePoint2.transform.position, firePoint2.transform.rotation);
                Instantiate(Fire_FX, firePoint2.transform.position, firePoint2.transform.rotation);
                Instantiate(Muzzle, firePoint.transform.position, firePoint.transform.rotation);
            }



            yield return new WaitForSeconds(1.5f);
            myAnimator.SetTrigger(Idle_Anim);

            myAnimator.SetTrigger(Idle_Anim);

            DoAnim = true;
        }
    }

    
    [System.Serializable]
    public struct TankAnimation
    {
        [SerializeField]
        private string m_shoot;
        [SerializeField]
        private string m_idel;
        [SerializeField]
        private string m_idel2;
        [SerializeField]
        private string m_idel3d;
        [SerializeField]
        private string m_drive;
        [SerializeField]
        private string m_brakeForward;
        [SerializeField]
        private string m_brakeBackward;

        public string shoot => m_shoot;
        public string idel => m_idel;
        public string idel2 => m_idel2;
        public string idel3d => m_idel3d;
        public string drive => m_drive;
        public string brakeForward => m_brakeForward;
        public string brakeBackward => m_brakeBackward;


    }

}

   
