using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Amulay.Utility;
using UnityEngine.UI;
using TMPro;
namespace TankStars.Menu
{
    public class LevelStage_UI : MonoBehaviour
    {
        [SerializeField] private Sprite lockStageSprite, focusStageSprite, normalStageSprite;
        [SerializeField] private Image stageImage;
        [SerializeField] private Button stageButton;
        [SerializeField] private Image[] stars;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image focusImage;

        public void Init(int index, int starCount, bool isLock, bool isFocus, System.Action onClick)
        {
            text.color = Color.white;
            stageImage.sprite = normalStageSprite;
         //   text.text = (index + 1).ToString();

            if (isFocus)
            {
                stageImage.sprite = focusStageSprite;
            }

            if (isLock)
            {
                stageImage.sprite = lockStageSprite;
                text.color = Color.gray;
            }

            focusImage.gameObject.SetActive(isFocus);
            stageButton.interactable = !isLock;
            stageButton.onClick.AddListener(() => onClick?.Invoke());
          //  for (int i = 0; i < stars.Length; i++)
          //      stars[i].gameObject.SetActive(i < starCount);
        }
    }
}