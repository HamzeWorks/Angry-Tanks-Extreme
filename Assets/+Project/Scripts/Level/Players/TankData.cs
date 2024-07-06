using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankStars.Level;

namespace TankStars
{
    public class TankData : MonoBehaviour
    {
        [SerializeField] string id;
        [SerializeField] Tank prefab;
        [Header("Data"), Space(10)]
        [SerializeField] float fuleTankSize = 1f;
        [SerializeField] float maxShootPower = 5f;
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float m_maxHealth = 1;
        [SerializeField, Range(-90f, 90f)] float minAngle = -10f;
        [SerializeField] Item[] defaultItems;

        protected List<Item> items = new List<Item>();
        protected Item m_selectedItem;

        internal float maxHealth => m_maxHealth;
        internal float maxFule => fuleTankSize;//*
        internal float maxShootPowerr => maxShootPower;//*
        internal List<Item> itemsList => items;
    }
}