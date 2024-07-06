using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TankStars.Level
{ 
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] Image medalIcon;
        [SerializeField] TextMeshProUGUI usernameText;

        public void Init(Tank tank)
        {
            tank.onGetDamage += (currentHealth, damage, item) =>
            {
                 float currentHealthPerOne = currentHealth / Tank.public_health;
                 healthBar.value = currentHealthPerOne ;
                 print("health alan : " + healthBar.value + " %");
                 healthBar.transform.DOKill();
                 healthBar.transform.localScale = Vector3.one;
                 healthBar.transform.DOShakeScale(.5f, currentHealthPerOne);

                //if tand dead then turn off
                if (currentHealth <= 0)
                    gameObject.SetActive(false);
            };
        }
    
    }
}
