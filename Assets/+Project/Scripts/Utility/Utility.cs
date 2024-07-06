using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using JetBrains.Annotations;

namespace TankStars
{
    public static class Utility
    {
        public static Vec4 ToVec4(this Quaternion vec) => new Vec4 { x = vec.x, y = vec.y, z = vec.z, w = vec.w };
        public static Vec3 ToVec3(this Vector3 vec) => new Vec3 { x = vec.x, y = vec.y, z = vec.z };
        public static Vec3 ToVec3(this Vector2 vec) => new Vec3 { x = vec.x, y = vec.y, z = 0 };
        public static Vec2 ToVec2(this Vector2 vec) => new Vec2 { x = vec.x, y = vec.y };
        public static Vec2 ToVec2(this Vector3 vec) => new Vec2 { x = vec.x, y = vec.y };

        public static Quaternion ToQuaternion(this Vec4 vec) => new Quaternion { x = vec.x, y = vec.y, z = vec.z, w = vec.w };
        public static Vector3 ToVector3(this Vec3 vec) => new Vector3 { x = vec.x, y = vec.y, z = vec.z };
        public static Vector3 ToVector3(this Vec2 vec) => new Vector3 { x = vec.x, y = vec.y, z = 0 };
        public static Vector2 ToVector2(this Vec2 vec) => new Vector2 { x = vec.x, y = vec.y };
        public static Vector2 ToVector2(this Vec3 vec) => new Vector2 { x = vec.x, y = vec.y };

        //public static string ToStr(this float value) => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        //public static string ToStr(this Vector3 value) => $"{value.x.ToStr()}|{value.y.ToStr()}|{value.z.ToStr()}";
        //public static string ToStr(this Quaternion value) => $"{value.x.ToStr()}|{value.y.ToStr()}|{value.z.ToStr()}|{value.w.ToStr()}";

        //public static float ToSingle(this string value) => float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        //public static Vector3 ToVector3(this string value)
        //{
        //    var vec = value.Split('|');
        //    if (vec.Length != 3)
        //        throw new System.Exception("incorrect input");
        //    return new Vector3(vec[0].ToSingle(),vec[1].ToSingle(),vec[2].ToSingle());
        //}
        //public static Quaternion ToQuaternion(this string value)
        //{
        //    var vec = value.Split('|');
        //    if (vec.Length != 4)
        //        throw new System.Exception("incorrect input");
        //    return new Quaternion(vec[0].ToSingle(), vec[1].ToSingle(), vec[2].ToSingle(), vec[3].ToSingle());
        //}
        //public struct SVector3//*
        //{
        //    public string x, y, z;
        //}
    }

    [System.Serializable]
    public class PlayerData
    {
        #region data
        public int avatar = -1;
        public string username = string.Empty;
        public string password = string.Empty;

        public int xp = -1;
        public int coin = -1;
        public int gem = -1;

        public DateTime updated;
        #endregion
        #region methods
        internal static PlayerData instance { get; private set; } = null;
        internal static event Action<PlayerData> onChanged;
        private const string savePath = "playerData";

        internal void Save()
        {
            instance = this;
            PlayerPrefs.SetString(savePath, JsonUtility.ToJson(this));
        }

        internal void Save(string path)
        {
            PlayerPrefs.SetString(path, JsonUtility.ToJson(this));
        }

        internal static PlayerData Load()
        {
            instance = new PlayerData();

            var rawData = PlayerPrefs.GetString(savePath, string.Empty);
            if (string.IsNullOrEmpty(rawData) == false)
            {
                instance = JsonUtility.FromJson<PlayerData>(rawData);
            }

            return instance;
        }

        internal static PlayerData Load(string path)
        {
            var m_PlayerData = new PlayerData();

            var rawData = PlayerPrefs.GetString(path, string.Empty);
            if (string.IsNullOrEmpty(rawData) == false)
            {
                m_PlayerData = JsonUtility.FromJson<PlayerData>(rawData);
            }

            return m_PlayerData;
        }

        internal void Modified()
        {
            onChanged?.Invoke(instance);
        }
        #endregion
    }
}