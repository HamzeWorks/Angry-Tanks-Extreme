using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Amulay.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    //if (m_instance == null)
                    //    m_instance = Instantiate(Resources.Load<T>(typeof(T).Name));
                }
                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
                m_instance = this as T;
            else if (m_instance != this as T)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_instance == this)
                m_instance = null;
        }
    }

    public class SingletonDontDestroyOnLoad<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                    if(m_instance == null)
                        return null;
                }
                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if(m_instance == null)
            {
                m_instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else if(m_instance == this as T)
            {
                DontDestroyOnLoad(this.gameObject);

            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_instance == this as T)
                m_instance = null;
        }
    }

    public class SingletonResource<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    if (m_instance == null)
                        m_instance = Instantiate(Resources.Load<T>(typeof(T).Name));
                }
                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
                m_instance = this as T;
            else if (m_instance != this)
            {
                DestroyImmediate(m_instance.gameObject);
                return;
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_instance == this)
                m_instance = null;
        }
    }

    public class SingletonForce<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    if (m_instance == null)
                        m_instance = new GameObject($"[ {typeof(T).Name} ]").AddComponent<T>();
                }
                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
                m_instance = this as T;
            else if (m_instance != this)
            {
                DestroyImmediate(m_instance.gameObject);
                return;
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_instance == this)
                m_instance = null;
        }
    }

    public class SingletonDontDestroyOnLoadForce<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    if (m_instance == null)
                        m_instance = new GameObject($"[ {typeof(T).Name} ]").AddComponent<T>();

                    DontDestroyOnLoad(m_instance.gameObject);
                }
                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                DontDestroyOnLoad(m_instance.gameObject);
            }
            else if (m_instance != this)
            {
                DestroyImmediate(this.gameObject);
                return;
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_instance == this)
                m_instance = null;
        }
    }

    public class SingletonTooForce<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        private static bool m_ShuttingDown = false;
        private static object m_Lock = new object();
        private static T m_Instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (m_ShuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed. Returning null.");
                    return null;
                }

                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        // Search for existing instance.
                        m_Instance = (T)FindObjectOfType(typeof(T));

                        // Create new instance if one doesn't already exist.
                        if (m_Instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            m_Instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"[ {typeof(T).Name} ]";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return m_Instance;
                }
            }
        }


        private void OnApplicationQuit()
        {
            m_ShuttingDown = true;
        }


        private void OnDestroy()
        {
            m_ShuttingDown = true;
        }
    }
}
