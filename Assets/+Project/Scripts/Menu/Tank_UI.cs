using System.Collections;
using System.Collections.Generic;
using TankStars.Level;
using UnityEngine;

namespace TankStars {
    public class Tank_UI : MonoBehaviour
    {
        [SerializeField] Tank m_tankData;
        public Tank tankData => m_tankData;
    }
}