using System.Collections;
using System.Collections.Generic;
using TankStars.Level;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TankStars
{

    [CreateAssetMenu(fileName = "itemdata", menuName = "ScriptableObjects/ItemData", order = 1)]
    public class ItemData : ScriptableObject
    {

        [SerializeField] string m_itemName = "item_";
        [SerializeField] Sprite m_icon;
        [SerializeField] AssetReference m_prefabReference;
        [SerializeField] AssetReference m_explotionPrefabReference;
        [Header("ItemLogic"), Space(10)]
        [SerializeField, Range(0, 50f)]     int defaultUpgradeCost = 10;
        [SerializeField, Range(0, 3f)]      float upgradeCost_perLevel = .2f;
        [SerializeField, Range(0, 100f)]    float defaultPower = 5;
        [SerializeField, Range(0, 3f)]      float power_perLevel = .2f;
        [Header("item"), Space(10)]
        [SerializeField, Range(3f, 15f)]    float m_lifeTime = 7f;
        [SerializeField, Range(0, 5f)]      float m_radius = 1f;
        [SerializeField, Range(0.01f, 15f)] float m_weight = 1;

        private int m_level
        {
            get => PlayerPrefs.GetInt(m_itemName, 0);
            set => PlayerPrefs.SetInt(m_itemName, value);
        }

        internal string itemName     => m_itemName;
        internal Sprite icon         => m_icon;
        internal int    level        => m_level;
        internal int    upgradeCost  => (int)CalculatePerLevel(m_level,defaultUpgradeCost, upgradeCost_perLevel);
        internal float  power        => (int)CalculatePerLevel(m_level, defaultPower, power_perLevel);
        internal float  weight       => m_weight;
        internal float  lifeTime     => m_lifeTime;
        internal float  radius       => m_radius;

        private float CalculatePerLevel(int level ,float defaultValue, float perLevel)
        {
            float result = defaultValue;
            for (int i = 0; i < level; i++)
                result += result * perLevel;

            return result;
        }

        internal bool Upgrade()
        {
            m_level++;
            return true;
        }

        internal int CalcuateUpgradeConstWithManualLevel(int level) => (int)CalculatePerLevel(level, defaultUpgradeCost, upgradeCost_perLevel);
        internal float CalcuatePowerWithManualLevel(int level) => (int)CalculatePerLevel(level, defaultPower, power_perLevel);


#if UNITY_EDITOR
        private void OnValidate()
        {
            //Debug.Log($"{itemName}: power_2 = {CalcuatePowerWithManualLevel(2)}, power_5 = {CalcuatePowerWithManualLevel(5)}, power_10 = {CalcuatePowerWithManualLevel(10)}, \ncost_2 = {CalcuateUpgradeConstWithManualLevel(2)}, cost_5 = {CalcuateUpgradeConstWithManualLevel(5)}, cost_10 = {CalcuateUpgradeConstWithManualLevel(10)}, ");
        }
#endif
    }
}