using UnityEngine;

namespace Amulay.Utility
{
    public class Pint
    {
        public string name { get; private set; }
        public int defualtValue { get; private set; }

        public Pint(string name, int defualtValue = default)
        {
            this.name = name;
            this.defualtValue = defualtValue;
        }

        public int Value
        {
            get
            {
                if (PlayerPrefs.HasKey(name) == false)
                    return defualtValue;
                return PlayerPrefs.GetInt(name, defualtValue);
            }
            set
            {
                PlayerPrefs.SetInt(name, value);
            }
        }
    }

    public class Pstring
    {
        public string name { get; private set; }
        public string defualtValue { get; private set; }

        public Pstring(string name, string defualtValue = default)
        {
            this.name = name;
            this.defualtValue = defualtValue;
        }

        public string Value
        {
            get
            {
                if (PlayerPrefs.HasKey(name) == false)
                    return defualtValue;
                return PlayerPrefs.GetString(name, defualtValue);
            }
            set
            {
                PlayerPrefs.SetString(name, value);
            }
        }
    }

    public class Pbool
    {
        public string name { get; private set; }
        public bool defualtValue { get; private set; }

        public Pbool(string name, bool defualtValue = default)
        {
            this.name = name;
            this.defualtValue = defualtValue;
        }

        public bool Value
        {
            get
            {
                if (PlayerPrefs.HasKey(name) == false)
                    return defualtValue;
                return PlayerPrefs.GetInt(name, (defualtValue) ? 1 : 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(name, (value) ? 1 : 0);
            }
        }
    }

    public class Pfloat
    {
        public string name { get; private set; }
        public float defualtValue { get; private set; }

        public Pfloat(string name, float defualtValue = default)
        {
            this.name = name;
            this.defualtValue = defualtValue;
        }

        public float Value
        {
            get
            {
                if (PlayerPrefs.HasKey(name) == false)
                    return defualtValue;
                return PlayerPrefs.GetFloat(name, defualtValue);
            }
            set
            {
                PlayerPrefs.SetFloat(name, value);
            }
        }
    }
}